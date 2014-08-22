using System;
using System.Linq;
using System.Web.Http;
using LeanBattleship.Common;
using LeanBattleship.Core.Services;
using LeanBattleship.Data;
// using LeanBattleship.Data.Migrations;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace LeanBattleship.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>()); 

            var locator = new UnityServiceLocator(ConfigureUnityContainer());
            ServiceLocator.SetLocatorProvider(() => locator);

            var ctx = new DataContext(ServiceLocator.Current.GetInstance<IApplicationSettings>().DatabaseConnection);

            var alltournaments = ctx.Tournaments.ToList();

            Console.WriteLine("There are {0} tournaments", alltournaments.Count);
        }

        private static IUnityContainer ConfigureUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IApplicationSettings, AzureCloudApplicationSettings>(new ContainerControlledLifetimeManager());
            container.RegisterType<DataContext, DataContext>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITournamentService, TournamentService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPlayerService, PlayerService>(new ContainerControlledLifetimeManager());
            
            return container;
        }

    }

    internal class AzureCloudApplicationSettings : IApplicationSettings
    {
        public string DatabaseConnection {
            get
            {
                if (RoleEnvironment.IsAvailable)
                {
                    return CloudConfigurationManager.GetSetting("DatabaseConnection");
                }

                return @"Server=.\SQLEXPRESS;Database=LeanBattleship;Trusted_Connection=True;";
            }
        }
    }
}