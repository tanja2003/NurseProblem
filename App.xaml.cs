using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NurseProblem.ApplicationLayer;
using NurseProblem.FrameworkLayer.Datenbank;
using NurseProblem.Infrastructure;
using NurseProblem.InterfaceAdaptersLayer.ViewModels;
using NurseProblem.Services;
using NurseProblem.Services.Interfaces;
using NurseProblem.UseCases;
using NurseProblem.UseCases.Interfaces;
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
        public static IServiceProvider Services { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            services.AddDbContext<ScheduleDbContext>(options =>
        options.UseSqlite("Data Source=schedule.db"));
            // ViewModels
            services.AddTransient<CreateNurseViewModel>();
            services.AddTransient<NurseManagementViewModel>();
            services.AddTransient<StartViewModel>();
            services.AddTransient<NurseDetailViewModel>();

            // Services
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IErrorDialogService, ErrorDialogService>();


            // UseCases / Infra
            services.AddTransient<CreateNurseUseCase>();
            services.AddTransient<ManageNurseUseCase>();
            services.AddTransient<INurseRepository, NurseRepository>();

            Services = services.BuildServiceProvider();


            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ScheduleDbContext>();
            db.Database.EnsureCreated();
            base.OnStartup(e);

            
        }
    }
}
