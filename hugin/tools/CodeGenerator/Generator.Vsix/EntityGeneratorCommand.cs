using EnvDTE;
using EnvDTE80;
using Generator.Extensions;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Newtonsoft.Json;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using Task = System.Threading.Tasks.Task;

namespace VsAssistant
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class EntityGeneratorCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4129;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("3ada1d20-44ab-46c2-bc85-94bad5d05049");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityGeneratorCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private EntityGeneratorCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static EntityGeneratorCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in EntityGeneratorCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new EntityGeneratorCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var selectedProjectItem = GetSelectedSolutionExplorerItem(ServiceProvider);
            var selectedProject = selectedProjectItem.ContainingProject.FullName;
            var selectedItem = selectedProjectItem.Properties.Item("FullPath").Value.ToString();

            var generatorSolution = new SolutionInfo().CreateGeneratorSolution(selectedProject, selectedItem);

            var defaultConfig = Path.Combine(Directory.GetCurrentDirectory(), "solutionInfo.json");
            defaultConfig.CreateFile(JsonConvert.SerializeObject(generatorSolution));

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory(), "excelGenerator", "Generator.Excel.exe");
            processStartInfo.Arguments = " \"" + defaultConfig + "\"";
            System.Diagnostics.Process.Start(processStartInfo);
        }

        //string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
        //string title = "EntityGeneratorCommand";

        //// Show a message box to prove we were here
        //VsShellUtilities.ShowMessageBox(
        //    this.package,
        //    message,
        //    title,
        //    OLEMSGICON.OLEMSGICON_INFO,
        //    OLEMSGBUTTON.OLEMSGBUTTON_OK,
        //    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

        private ProjectItem GetSelectedSolutionExplorerItem(IAsyncServiceProvider serviceProvider)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = package.JoinableTaskFactory.Run(() => serviceProvider.GetServiceAsync(typeof(DTE)));
            if (result == null)
            {
                return null;
            }

            var ide = (DTE2)result;
            Assumes.Present(ide);

            var items = ide.ToolWindows.SolutionExplorer.SelectedItems as object[];
            if (items == null || items.Length == 0)
            {
                return null;
            }
            if (items[0] is UIHierarchyItem hierarchyItem)
            {
                return hierarchyItem.Object as ProjectItem;
            }

            return null;
        }
    }
}
