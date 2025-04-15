using HarmonyLib;
using MonkeFavoritesMod.Helpers;

namespace MonkeFavoritesMod.Patches;
// AmmoSlot
[HarmonyPatch(typeof(ItemSlot))]
class ItemSlotPatch
{
    [HarmonyPatch(nameof(ItemSlot.Initialize), new[] { typeof(BasePickupItem), typeof(ItemStorage), typeof(ItemSlot.DisplayMode) }), HarmonyPrefix]
    static void InitializePrefix(ItemSlot __instance, BasePickupItem item, ItemStorage itemStorage, ItemSlot.DisplayMode displayMode)
    {
        FavoritesHelper.AddSubIconToSlot(__instance.gameObject, item?.Id);
    }

    [HarmonyPatch(nameof(ItemSlot.InitializeEmpty), new[] { typeof(ItemStorage) }), HarmonyPrefix]
    static void InitializeEmptyPrefix(ItemSlot __instance, ItemStorage storage)
    {
        FavoritesHelper.AddSubIconToSlot(__instance.gameObject, null);
    }

}
