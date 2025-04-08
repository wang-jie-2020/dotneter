using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AbpGenerator.Controls
{
    public partial class Copyright : UserControl
    {
        public Copyright()
        {
            this.InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
