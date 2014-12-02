using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CastService.Web.Startup))]
namespace CastService.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
