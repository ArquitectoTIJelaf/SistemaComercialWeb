using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SisComWeb.Aplication.Startup))]
namespace SisComWeb.Aplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}