using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SendEmail.Startup))]
namespace SendEmail
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
