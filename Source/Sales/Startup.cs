using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sales.Startup))]
namespace Sales
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
