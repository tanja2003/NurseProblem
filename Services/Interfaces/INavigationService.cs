using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.Services.Interfaces
{
    public interface INavigationService
    {
        void OpenCalendar();
        void OpenCalculateSchedule();
        void OpenNurseManagment();
        void OpenHistory();

    }
}
