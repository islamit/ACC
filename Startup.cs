using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ACC.Startup))]
namespace ACC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
