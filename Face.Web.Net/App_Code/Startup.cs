using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Face.Web.Net.Startup))]
namespace Face.Web.Net
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
