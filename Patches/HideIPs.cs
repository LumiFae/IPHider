using System.Text.RegularExpressions;
using Exiled.API.Features;
using HarmonyLib;

namespace IPHider.Patches
{
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.AddLog))]
    public class HideIPs
    {
        private static readonly string _regex = @"\b(?:\d{1,3}\.){3}\d{1,3}(?::\d+)?\b";
        
        // ReSharper disable once InconsistentNaming
        private static bool Prefix(ServerConsole __instance, string q, ConsoleColor color,
            bool hideFromOutputs = false)
        {
            string qLower = q.ToLower();
            bool containsIp = CheckIP(q);

            if (!Plugin.Instance.Config.Keywords.Any(keyword => qLower.Contains(keyword) && containsIp)) return true;
            ServerConsole.AddLog(RemoveIPs(q), color, hideFromOutputs);
            return false;
        }

        private static bool CheckIP(string input)
        {
            return Regex.IsMatch(input, _regex);
        }
        
        private static string RemoveIPs(string input)
        {
            return Regex.Replace(input, _regex, Plugin.Instance.Translation.RemoveText);
        }
    }
}