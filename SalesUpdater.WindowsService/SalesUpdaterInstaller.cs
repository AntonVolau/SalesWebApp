using System.ComponentModel;

namespace SalesUpdater.WindowsService
{
    [RunInstaller(true)]
    public partial class SalesUpdaterInstaller : System.Configuration.Install.Installer
    {
        public SalesUpdaterInstaller()
        {
            InitializeComponent();
        }
    }
}
