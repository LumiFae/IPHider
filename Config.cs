using System.ComponentModel;
#if EXILED
using Exiled.API.Interfaces;
#endif

namespace IPHider
{
    public class Config
#if EXILED
    : IConfig
#endif
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        public bool ShowInLogs { get; set; } = true;
        
        // ReSharper disable once UseCollectionExpression
        [Description("The keywords that will trigger the IP to be hidden. Modify these incase you have plugins that output IPs in a different way.")]
        public IEnumerable<string> Keywords { get; set; } = new[]
        {
            "player",
            "incoming connection",
            "with the ip",
            "disconnected"
        };
    }
}