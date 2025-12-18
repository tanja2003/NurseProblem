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

            using var db = new ScheduleDbContext();
            db.Database.EnsureCreated();
        }
    }
}
