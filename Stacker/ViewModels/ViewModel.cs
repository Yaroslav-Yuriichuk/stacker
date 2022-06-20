using Stacker.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Stacker.ViewModels
{
    public abstract class ViewModel : IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
