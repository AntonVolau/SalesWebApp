using SalesUpdater.Core;
using System.Configuration;
using System.ServiceProcess;

namespace SalesUpdater.WindowsService
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        private const string keyPath = "keyPath";
        static void Main()
        {
            var key = ConfigurationManager.AppSettings[keyPath];
            if (System.IO.File.Exists(key))
            {
                // myApp is already running...
                Logger.Log("Another instance of an app is already running");
                return;
            }
            else
            {
                ServiceBase[] servicesToRun;
                Logger.Log("App started");
                servicesToRun = new ServiceBase[]
                {
                    new SalesUpdater()
                };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
