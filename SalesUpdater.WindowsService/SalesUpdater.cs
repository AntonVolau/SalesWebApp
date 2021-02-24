using SalesUpdater.Core;
using System.Configuration;
using System.ServiceProcess;

namespace SalesUpdater.WindowsService
{
    public partial class SalesUpdater : ServiceBase
    {
        private const string filtersParamKey = "filesFilter";
        private const string filePath = "directoryPath";
        string filesFilter = ConfigurationManager.AppSettings[filtersParamKey];
        string directoryPath = ConfigurationManager.AppSettings[filePath];

        static private Controller _controller;
        public SalesUpdater()
        {
            Logger.Log("SalesUpdaterCreated");
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
        }

        protected override void OnStart(string[] args)
        {
            Logger.Log("OnStart");

            _controller = new Controller(directoryPath, filesFilter);
            KeyHandler.SetKey();
            _controller.Run();
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            KeyHandler.DeleteKey();
            base.OnStop();
        }

        protected override void OnPause()
        {
            KeyHandler.DeleteKey();
            base.OnPause();
        }

        protected override void OnContinue()
        {
            KeyHandler.SetKey();
            base.OnContinue();
        }
    }
}
