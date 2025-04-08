using System.Windows;
using Generator;

namespace AbpGenerator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            DataContext = new MainWindowViewModel();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (!Global.Options.HasChecked())
            {
                MessageBox.Show(" 请至少选择一项!", "警告", MessageBoxButton.OK);
                return;
            }

            Visibility = Visibility.Hidden;

            if (!Global.Options.UseApplication)
            {
                StartCodeGenerator();
                Close();
            }

            new PropertySelectorWindow(delegate (CallBackType backType)
            {
                switch (backType)
                {
                    case CallBackType.Default:
                        break;
                    case CallBackType.Prev:
                        Visibility = Visibility.Visible;
                        return;
                    case CallBackType.Cancel:
                        Close();
                        return;
                    case CallBackType.Enter:
                        StartCodeGenerator();
                        Close();
                        break;
                    default:
                        return;
                }
            }).Show();
        }

        private void StartCodeGenerator()
        {
            var generator = new CodeGenerator();
            generator.Start();
        }
    }
}
