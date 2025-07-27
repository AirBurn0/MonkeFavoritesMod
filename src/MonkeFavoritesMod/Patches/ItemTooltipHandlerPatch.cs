using HarmonyLib;
using MonkeFavoritesMod.Helpers;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(ItemTooltipHandler))]
class ItemTooltipHandlerPatch
{

    [HarmonyPatch(nameof(ItemTooltipHandler.Initialize), [typeof(BasePickupItem), typeof(BasePickupItemRecord)]), HarmonyPrefix]
    static void InitializePrefix(ItemTooltipHandler __instance, BasePickupItem item, BasePickupItemRecord record)
    {
        FavoritesHelper.AddSubIconToSlot(__instance.gameObject, record?.Id);
    }

    [HarmonyPatch(nameof(ItemTooltipHandler.Initialize), [typeof(string)]), HarmonyPrefix]
    static void InitializePrefix(ItemTooltipHandler __instance, string itemId)
    {
        FavoritesHelper.AddSubIconToSlot(__instance.gameObject, itemId);
    }

}
