using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Calculations.Startup))]
namespace Calculations
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
