using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AADB2C.WebAPI.BasicAuth.Startup))]
namespace AADB2C.WebAPI.BasicAuth
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use<ClientAuthMiddleware>();
        }
    }
}