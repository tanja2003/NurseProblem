using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem
{
    using NurseProblem.DomainLayer.UiModelle;
    using System;
    using System.Windows.Input;

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;
        private Action<Nurse> openNurseDetails;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<Nurse> openNurseDetails)
        {
            this.openNurseDetails = openNurseDetails;
        }

        public bool CanExecute(object? parameter)
            => _canExecute == null || _canExecute();

        public void Execute(object? parameter)
            => _execute();

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

}
