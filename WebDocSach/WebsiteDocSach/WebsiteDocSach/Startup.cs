using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebsiteDocSach.Startup))]
namespace WebsiteDocSach
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
