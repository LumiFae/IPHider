﻿using System.Text.RegularExpressions;
using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;

namespace IPHider
{
    public class Plugin : Plugin<Config, Translation>
    {
        public override string Name { get; } = "IPHider";
        public override string Author { get; } = "LumiFae";
        public override string Prefix { get; } = "IpHider";
        public override Version Version { get; } = new(1, 0, 0);
        
        public static Plugin Instance { get; private set; }
        
        private Harmony _harmony;

        public override void OnEnabled()
        {
            Instance = this;
            try
            {
                _harmony = new($"iphider.lumifae.{DateTime.Now.Ticks}");
                _harmony.PatchAll();
            }
            catch (Exception e)
            {
                Log.Error($"Failed to patch: {e}");
            }

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            _harmony?.UnpatchAll();
            _harmony = null;
            base.OnDisabled();
        }

        private const string Match = @"\b(?:\d{1,3}\.){3}\d{1,3}(?::\d+)?\b";

        internal static bool CheckIP(string input)
        {
            return Regex.IsMatch(input, Match);
        }
        
        internal static string RemoveIPs(string input)
        {
            return Regex.Replace(input, Match, Plugin.Instance.Translation.RemoveText);
        }
    }
}