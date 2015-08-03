using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Projectfolio.Startup))]
namespace Projectfolio
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
