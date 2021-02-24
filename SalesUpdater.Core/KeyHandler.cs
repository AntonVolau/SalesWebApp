using System;

namespace SalesUpdater.Core
{
    public static class KeyHandler
    {

        public static void SetKey()
        {
            try
            {
                System.IO.File.AppendAllText("C:\\Logs\\key.txt", $"{DateTime.Now} {Environment.NewLine}");
            }
            catch
            {

            }
        }
        public static void DeleteKey()
        {
            try
            {
                System.IO.File.Delete("C:\\Logs\\key.txt");
            }
            catch
            {

            }
        }
    }
}
