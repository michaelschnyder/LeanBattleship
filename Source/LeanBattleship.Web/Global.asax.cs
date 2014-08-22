using System;
using System.Linq;
using System.Web.Http;
using System.Web.Services.Description;
using LeanBattleship.Common;
using LeanBattleship.Core.Game;
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

            var ctx = ServiceLocator.Current.GetInstance<DataContext>();

            var alltournaments = ctx.Tournaments.ToList();

            Console.WriteLine("There are {0} tournaments", alltournaments.Count);

            var gameServer = new GameServer();
            gameServer.Start();
        }
        
        private static IUnityContainer ConfigureUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IApplicationSettings, AzureCloudApplicationSettings>(new ContainerControlledLifetimeManager());
            container.RegisterType<DataContext, DataContext>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITournamentService, TournamentService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPlayerService, PlayerService>(new ContainerControlledLifetimeManager());
            

            var ctx = new DataContext(container.Resolve<IApplicationSettings>().DatabaseConnection);
            container.RegisterInstance(typeof (DataContext), ctx);


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