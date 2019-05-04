using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutoPartsMVC.Startup))]
namespace AutoPartsMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
