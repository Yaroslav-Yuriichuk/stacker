using Stacker.Views;
using System.Collections.Generic;
using System.Windows;

namespace Stacker.Services
{
    public class ApplicationWindowsService : IApplicationWindowsService
    {
        private Dictionary<WindowType, Window> _windows = new Dictionary<WindowType, Window>();

        #region Methods

        public void Open(WindowType window)
        {
            if (_windows.ContainsKey(window)) return;

            Window newWindow = null;

            switch (window)
            {
                case WindowType.HeightSettings:
                    newWindow = new HeightSettings();
                    break;
            }

            _windows.Add(window, newWindow);
            newWindow.Show();
        }

        public void Close(WindowType window)
        {
            if (!_windows.ContainsKey(window)) return;

            Window windowToClose = _windows[window];
            _windows.Remove(window);
            windowToClose.Close();
        }

        #endregion
    }
}
