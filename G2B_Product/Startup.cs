using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(G2B_Product.Startup))]
namespace G2B_Product
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
