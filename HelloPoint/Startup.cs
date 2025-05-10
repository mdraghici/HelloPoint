using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HelloPoint.Startup))]
namespace HelloPoint
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
