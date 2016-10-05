using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(FlightDataService.Startup))]

namespace FlightDataService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}