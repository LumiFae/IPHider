using HarmonyLib;

namespace IPHider.Patches
{
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.AddLog))]
    public class HideIPs
    {
        // ReSharper disable once InconsistentNaming
        private static bool Prefix(ServerConsole __instance, string q, ConsoleColor color,
            bool hideFromOutputs = false)
        {
            string qLower = q.ToLower();
            if (Plugin.CheckIP(q) && Plugin.Instance.Config.Keywords.Any(keyword => qLower.Contains(keyword)))
            {
                q = Plugin.RemoveIPs(q);
            }
            ServerConsole.PrintFormattedString(q, color);
            if (!hideFromOutputs)
                ServerConsole.PrintOnOutputs(q, color);
            return false;
        }
    }
}