using Microsoft.Owin;
using Owin;
using System.Net;

[assembly: OwinStartupAttribute(typeof(ServerContainer.Startup))]
namespace ServerContainer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //IPAddress ip = IPAddress.Parse("127.0.0.1");
            //int port = 8885;
            //ServerSocket.StartListening(ip, port);
        }
    }
}
