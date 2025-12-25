using NurseProblem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NurseProblem.Services
{
    public class ErrorDialogService : IErrorDialogService
    {
        public void ShowError(string message, string title)
        {
            MessageBox.Show(
                message,
                title,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

}
