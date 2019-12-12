using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TierraCreative.Startup))]
namespace TierraCreative
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
