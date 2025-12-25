using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.Services.Interfaces
{
    public interface IErrorDialogService
    {
        void ShowError(string message, string title);
    }

}
