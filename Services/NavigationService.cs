using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceProvider _serviceProvider;
        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
            var vm = _serviceProvider.GetRequiredService<NurseManagementViewModel>();
            var window = new NurseManagmentWindow
            {
                DataContext = vm,
            };
            window.Show();
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

        public void OpenNewNurse()
        {
            var vm = _serviceProvider.GetRequiredService<NewNurseViewModel>();
            var window = new NewNurseWindow
            {
                DataContext = vm
            };
            vm.RequestClose += () => window.Close();
            window.ShowDialog();
        }
    }
}
