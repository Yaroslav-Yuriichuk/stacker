using System.Windows;
using System.Windows.Input;

namespace Stacker.Views
{
    /// <summary>
    /// Interaction logic for HeightSettings.xaml
    /// </summary>
    public partial class HeightSettings : Window
    {
        public HeightSettings()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}
