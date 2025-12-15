using Microsoft.EntityFrameworkCore;
using NurseProblem.Datenbank;
using System.Configuration;
using System.Data;
using System.Windows;

namespace NurseProblem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Migration / DB-Erstellung beim Start
            using (var context = new ScheduleDbContext())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
