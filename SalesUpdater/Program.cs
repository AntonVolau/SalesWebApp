using Microsoft.Win32;
using SalesUpdater.Core;
using System;
using System.Configuration;
using System.IO;

namespace SalesUpdater
{
    class Program
    {
        private const string filtersParamKey = "filesFilter";
        private const string filesPath = "filesPath";
        private const string keyPath = "keyPath";
        static void Main()
        {
            try
            {
                var key = ConfigurationManager.AppSettings[keyPath];
                if (System.IO.File.Exists(key))
                {
                    // myApp is already running...
                    Logger.Log("Another instanse of an application is already running");
                    return;
                }
                else
                {
                    var directoryPath = Path.Combine(Environment.CurrentDirectory, ConfigurationManager.AppSettings[filesPath]);
                    var filesFilter = ConfigurationManager.AppSettings[filtersParamKey];

                    using (var controller = new Controller(directoryPath, filesFilter))
                    {
                        controller.Run();
                        KeyHandler.SetKey();

                        Console.ReadKey();

                        controller.Stop();
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                KeyHandler.DeleteKey();
            }
        }
    }
}
