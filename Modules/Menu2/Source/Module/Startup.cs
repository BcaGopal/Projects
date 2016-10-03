using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Module.Startup))]
namespace Module
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
