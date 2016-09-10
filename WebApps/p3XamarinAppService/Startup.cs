using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(p3XamarinAppService.Startup))]

namespace p3XamarinAppService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}