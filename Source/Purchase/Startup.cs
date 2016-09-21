using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Purchase.Startup))]
namespace Purchase
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
