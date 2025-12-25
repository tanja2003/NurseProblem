using Microsoft.EntityFrameworkCore.Metadata;
using NurseProblem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NurseProblem.InterfaceAdaptersLayer.ViewModels
{
    public class StartViewModel
    {
        private readonly INavigationService _navigation;
        public ICommand OpenCalendarCommand { get; }
        public ICommand CalculateScheduleCommand { get; }
        public ICommand OpenNurseManagementCommand { get; }
        public ICommand OpenHistoryCommand { get; }
        public ICommand OpenNewNurseCommand { get; }


        public StartViewModel(INavigationService navigation)
        {
            _navigation = navigation;
            OpenCalendarCommand = new RelayCommand(OpenCalendar);
            CalculateScheduleCommand = new RelayCommand(CalculateSchedule);
            OpenNurseManagementCommand = new RelayCommand(OpenNurseManagement);
            OpenHistoryCommand = new RelayCommand(OpenHistory);
            OpenNewNurseCommand = new RelayCommand(OpenNewNurse);
         }

        private void OpenCalendar()
        {
            _navigation.OpenCalendar();
        }

        private void CalculateSchedule()
        {
            _navigation.OpenCalculateSchedule();
        }
        private void OpenNurseManagement()
        {
            _navigation.OpenNurseManagment();
        }

        private void OpenHistory()
        {
            _navigation.OpenHistory();
        }
        private void OpenNewNurse()
        {
            _navigation.OpenNewNurse();
        }
    }
}
