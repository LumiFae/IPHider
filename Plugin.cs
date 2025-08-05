using System.Text.RegularExpressions;
#if EXILED
using Exiled.API.Features;
#endif
using HarmonyLib;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Loader;

namespace IPHider
{
    public class Plugin : 
#if EXILED
        Plugin<Config, Translation>
#else
        LabApi.Loader.Features.Plugins.Plugin
#endif
    {
        public override string Name { get; } = "IPHider";
        public override string Author { get; } = "LumiFae";
#if EXILED
        public override string Prefix { get; } = "IpHider";
#else
        public override string Description { get; } = "Hide IPs from LocalAdmin console/logs";
        public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);
#endif
        public override Version Version { get; } = new(1, 1, 0);

        public static Plugin Instance { get; private set; }
        
        private Harmony _harmony;
#if LABAPI
        public Config Config { get; set; }
        public Translation Translation { get; set; }

        public override void Enable()
#else
        public override void OnEnabled()
#endif
        {
            Instance = this;
            try
            {
                _harmony = new($"iphider.lumifae.{DateTime.Now.Ticks}");
                _harmony.PatchAll();
            }
            catch (Exception e)
            {
#if EXILED
                Log.Error($"Failed to patch: {e}");
#else
                Logger.Error($"Failed to patch: {e}");
#endif
            }
#if EXILED
            base.OnEnabled();
#endif
        }
#if LABAPI
        public override void Disable()
#else
        public override void OnDisabled()
#endif
        {
            _harmony?.UnpatchAll();
            _harmony = null;
#if EXILED
            base.OnDisabled();
#endif
        }
        
#if LABAPI
        public override void LoadConfigs()
        {
            this.TryLoadConfig("config.yml", out Config config);
            Config = config ?? new Config();
            this.TryLoadConfig("translation.yml", out Translation translation);
            Translation = translation ?? new Translation();
        }
#endif
    }
}