#if EXILED
using Exiled.API.Interfaces;
#endif

namespace IPHider
{
    public class Translation
#if EXILED
    : ITranslation
#endif
    {
        public string RemoveText { get; set; } = "[REDACTED]";
    }
}