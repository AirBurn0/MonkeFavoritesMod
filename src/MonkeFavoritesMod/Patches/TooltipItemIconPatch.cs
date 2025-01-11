using HarmonyLib;
using MonkeFavoritesMod.Helpers;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(TooltipItemIcon))]
class TooltipItemIconPatch
{

    [HarmonyPatch(nameof(TooltipItemIcon.Initialize), new[] { typeof(string), typeof(bool) }), HarmonyPrefix]
    static void InitializePrefix(TooltipItemIcon __instance, string itemId, bool lockedByReputation)
    {
        FavoritesHelper.AddSubIconToSlot(__instance.gameObject, itemId);
    }

}
