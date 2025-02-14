using HarmonyLib;
using MonkeFavoritesMod.Scripts;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(ItemReceiptRequiredSlot))]
class ItemReceiptRequiredSlotPatch
{
    [HarmonyPatch(nameof(ItemReceiptRequiredSlot.Initialize), new[] { typeof(string), typeof(bool) }), HarmonyPrefix]
    static void InitializePrefix(ItemReceiptRequiredSlot __instance, string itemId, bool available)
    {
        if (!__instance.gameObject.TryGetComponent(out ItemSlotClickHandler listener))
            listener = __instance.gameObject.AddComponent<ItemSlotClickHandler>();
        listener.Initialize(__instance._itemTooltipHandler);
    }

}
