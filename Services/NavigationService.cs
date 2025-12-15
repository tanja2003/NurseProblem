using NurseProblem.Datenbank;
using NurseProblem.Services.Interfaces;
using NurseProblem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.Services
{
    public class NavigationService: INavigationService
    {
        private readonly ScheduleDbContext _db;
        public NavigationService(ScheduleDbContext db)
        {
            _db = db;
        }
        public void OpenCalendar()
        {
            var vm = new CalendarViewModel();
            var win = new CalenderWindwow
            {
                DataContext = vm,
            };
            win.Show();
        }

        public void OpenCalculateSchedule() 
        {
            var vm = new CalculateScheduleViewModel();
            var win = new CalculateScheduleWindow
            {
                DataContext = vm,
            };
            win.Show();
        }

        public void OpenNurseManagment()
        {
            var vm = new NurseManagementViewModel();
            var win = new NurseManagmentWindow
            {
                DataContext = vm,
            };
            win.Show();
        }

        public void OpenHistory()
        {
            var vm = new HistoryViewModel();
            var win = new HistoryWindow
            {
                DataContext = vm,
            };
            win.Show();
        }
    }
}
