using NurseProblem.DomainLayer.UiModelle;

namespace NurseProblem.InterfaceAdaptersLayer.ViewModels
{
    internal class NurseDetailViewModel
    {
        private Nurse nurse;

        public NurseDetailViewModel(Nurse nurse)
        {
            this.nurse = nurse;
        }
    }
}