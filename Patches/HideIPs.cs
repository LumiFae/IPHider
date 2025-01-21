using System.Text.RegularExpressions;
using Exiled.API.Features;
using HarmonyLib;
using Paths = PluginAPI.Helpers.Paths;

namespace IPHider.Patches
{
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.AddLog))]
    public class HideIPs
    {
        
        
        // ReSharper disable once InconsistentNaming
        private static bool Prefix(ServerConsole __instance, string q, ConsoleColor color,
            bool hideFromOutputs = false)
        {
            string oldQ = q;
            string qLower = q.ToLower();
            if (Plugin.CheckIP(q) && Plugin.Instance.Config.Keywords.Any(keyword => qLower.Contains(keyword)))
            {
                q = Plugin.RemoveIPs(q);
            }
            ServerConsole.PrintFormattedString(q, color);
            if (!hideFromOutputs)
                ServerConsole.PrintOnOutputs(q, color);
            if(!Plugin.Instance.Config.ShowInLogs)
                FixLocalAdminLogs(q, oldQ);
            return false;
        }

        private static void FixLocalAdminLogs(string current, string fixedString)
        {
            string path = Path.Combine(Paths.SecretLab, "LocalAdminLogs", Server.Port.ToString());
            if (!Directory.Exists(path))
                return;
            FileInfo info = new DirectoryInfo(path)
                .GetFiles()
                .OrderByDescending(f => f.LastWriteTime)
                .FirstOrDefault();
            if (info == null)
                return;
            string[] text = File.ReadAllLines(info.FullName);
            string[] lines = text.Skip(Math.Max(0, text.Length - 5)).ToArray();

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Replace(current, fixedString);
            }

            string fullText = string.Join("\n", text.Take(text.Length - 5).Concat(lines));
            File.WriteAllText(info.FullName, fullText);
        }
    }
}