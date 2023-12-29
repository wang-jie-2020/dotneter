using System;
using System.ComponentModel;
using System.Windows;

namespace AbpGenerator
{
    public partial class PropertySelectorWindow : Window
    {
        public Action<CallBackType> CloseCallBack { get; set; }

        private CallBackType BackType { get; set; }

        public PropertySelectorWindow(Action<CallBackType> closeCallBack)
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Closing += PropertySelectorWindow_Closing;
            CloseCallBack = closeCallBack;

            DataContext = new PropertySelectorWindowViewModel();
        }

        private void PropertySelectorWindow_Closing(object sender, CancelEventArgs e)
        {
            CloseCallBack?.Invoke(BackType);
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            BackType = CallBackType.Prev;
            Close();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            BackType = CallBackType.Enter;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            BackType = CallBackType.Cancel;
            Close();
        }
    }
}
