using NurseProblem.Models.UiModelle;

namespace NurseProblem.ViewModels
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