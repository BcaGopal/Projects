using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Customise.Startup))]
namespace Customise
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
