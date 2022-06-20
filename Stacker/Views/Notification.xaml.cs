using System.Windows;

namespace Stacker.Views
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : Window
    {
        public Notification()
        {
            InitializeComponent();

            int Margin = 10;

            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width - Margin;
            this.Top = desktopWorkingArea.Bottom - this.Height - Margin;
        }
    }
}
