using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Planning.Startup))]
namespace Planning
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
