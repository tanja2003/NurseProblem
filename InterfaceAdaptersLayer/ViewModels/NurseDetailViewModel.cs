using NurseProblem.DomainLayer.UiModelle;
using System.Windows.Input;

namespace NurseProblem.InterfaceAdaptersLayer.ViewModels
{
    internal class NurseDetailViewModel
    {
        private Nurse nurse;
        public event Action? RequestClose;
        public ICommand CancelCommand { get; }

        public NurseDetailViewModel(Nurse nurse)
        {
            this.nurse = nurse;
            CancelCommand = new RelayCommand(CloseWindow);
        }

        private void CloseWindow() => RequestClose?.Invoke();
    }
}