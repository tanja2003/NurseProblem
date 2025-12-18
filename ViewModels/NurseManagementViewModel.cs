using NurseProblem.Converter;
using NurseProblem.Models.UiModelle;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.ViewModels
{
    public class NurseManagementViewModel
    {
        public ObservableCollection<Nurse> Nurses { get; set; } = [];
        public string Name { get; set; } = "";
        public int Id { get; set; } = new();

        public NurseManagementViewModel() {
            Nurses = new ObservableCollection<Nurse>();
            var nurse2 = Dictionarys.NurseNames;

            foreach (var nurse in nurse2) {
                var NewNurse = new Nurse();
                NewNurse.Id = nurse.Key;
                NewNurse.Name = nurse.Value;
                Nurses.Add(NewNurse);
            }
        }

    }
}
