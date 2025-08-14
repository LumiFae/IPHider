using System.Reflection.Emit;
using System.Text.RegularExpressions;
using HarmonyLib;
using LabApi.Features.Wrappers;

namespace IPHider.Patches
{
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.AddLog))]
    public static class HideIPs
    {
        private static string ReplaceText => Plugin.Instance.Translation.RemoveText;

        private static Config Config => Plugin.Instance.Config;

        private static Regex IPRegex { get; } = new(@"\b(?:\d{1,3}\.){3}\d{1,3}(?::\d+)?\b", RegexOptions.Compiled);

        private static Regex IDRegex { get; } =
            new(@"\w+@(?:steam|discord|northwood|patreon)", RegexOptions.Compiled);

        private static Regex AuthTokenRegex { get; } = new(@"[a-zA-Z0-9\/]{8}-[a-zA-Z0-9\/]{8}", RegexOptions.Compiled);

        // ReSharper disable once InconsistentNaming
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) =>
        [
            new(OpCodes.Ldarga, 0),
            new(OpCodes.Call, AccessTools.Method(typeof(HideIPs), nameof(ModifyLog))),
            ..instructions
        ];

        private static void ModifyLog(ref string q)
        {
            string qLower = q.ToLower();
            if (Config.HideIps && IPRegex.IsMatch(q) &&
                Config.Keywords.Any(keyword => qLower.Contains(keyword.ToLower())))
                q = IPRegex.ReplaceWithText(q);
            MatchCollection matches = IDRegex.Matches(q);
            foreach (Match match in matches)
            {
                // "put HideIds first for the nanoseconds" - Axwabo :troll:
                if (Config.HideIds || !Player.TryGet(match.Value, out Player player) ||  !player.ReferenceHub || !player.IsReady)
                {
                    q = q.ReplaceWithText(match.Value);
                    continue;
                }

                if (!Config.HideDntIds ||
                    (!player.DoNotTrack &&
                     !player.ReferenceHub.authManager.AuthenticationResponse.AuthToken.SyncHashed))
                    continue;

                q = q.ReplaceWithText(match.Value);
            }

            if (Config.HideAuthTokens &&
                Config.AuthTokenKeywords.Any(keyword => qLower.Contains(keyword.ToLower())))
                q = AuthTokenRegex.ReplaceWithText(q);
        }

        private static string ReplaceWithText(this Regex regex, string input) => regex.Replace(input, ReplaceText);

        private static string ReplaceWithText(this string str, string input) => str.Replace(input, ReplaceText);
    }
}