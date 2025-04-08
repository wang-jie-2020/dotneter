using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Generator;
using Newtonsoft.Json;

namespace ExcelGenerator
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var defaultConfig = Path.Combine(Directory.GetParent(typeof(Program).Assembly.Location).FullName, "solutionInfo.json");

            if (args != null && args.Length > 0)
            {
                defaultConfig = args[0];
            }

            if (!File.Exists(defaultConfig))
            {
                throw new Exception("no config");
            }

            try
            {
                InitGlobal(defaultConfig);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow());
            }
            catch (Exception ex)
            {
                MessageBox.Show("意外错误:" + ex.Message);
            }
        }

        private static void InitGlobal(string configPath)
        {
            Global.GeneratorSolution = JsonConvert.DeserializeObject<GeneratorSolution>(File.ReadAllText(configPath, Encoding.UTF8));
            if (Global.GeneratorSolution == null || string.IsNullOrWhiteSpace(Global.GeneratorSolution.CurrentSelectedItem))
            {
                throw new Exception("wrong config");
            }
        }
    }
}
