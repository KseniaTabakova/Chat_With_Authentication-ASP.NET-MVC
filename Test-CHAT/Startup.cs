using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Test_CHAT.Startup))]
namespace Test_CHAT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
