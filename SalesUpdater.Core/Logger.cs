using System;
using System.Configuration;

namespace SalesUpdater.Core
{
    public static class Logger
    {
        private static string logLocation = "logsLocation";
        public static void Log(string message)
        {
            string logslocation = ConfigurationManager.AppSettings[logLocation];
            try
            {
                System.IO.File.AppendAllText(logslocation, $"{DateTime.Now} {message} {Environment.NewLine}");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
