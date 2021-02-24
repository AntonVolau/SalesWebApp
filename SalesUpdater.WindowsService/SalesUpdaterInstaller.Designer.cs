
namespace SalesUpdater.WindowsService
{
    partial class SalesUpdaterInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SalesUpdaterProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.SalesUpdateInstaller = new System.ServiceProcess.ServiceInstaller();
          
            // SalesUpdaterProcessInstaller
          
            this.SalesUpdaterProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.SalesUpdaterProcessInstaller.Password = null;
            this.SalesUpdaterProcessInstaller.Username = null;
           
            // SalesUpdateInstaller
          
            this.SalesUpdateInstaller.ServiceName = "SalesUpdater";
          
            // ProjectInstaller
           
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.SalesUpdaterProcessInstaller,
            this.SalesUpdateInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller SalesUpdaterProcessInstaller;
        private System.ServiceProcess.ServiceInstaller SalesUpdateInstaller;
    }
}