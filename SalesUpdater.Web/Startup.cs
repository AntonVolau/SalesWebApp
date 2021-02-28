using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SalesUpdater.Web.Startup))]
namespace SalesUpdater.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
