using System.Collections.Generic;
using HarmonyLib;
using MonkeFavoritesMod.Helpers;
using UnityEngine.EventSystems;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(CommonButton))]
class CommonButtonPatch
{
    [HarmonyPatch(nameof(CommonButton.OnPointerClick), new[] { typeof(PointerEventData) }), HarmonyPrefix]
    static bool OnPointerClickPrefix(CommonButton __instance, ref PointerEventData eventData)
    {
        if (!__instance.IsInteractable)
            return true;
        if(!__instance.gameObject.TryGetComponent(out MagnumPerkTooltipHandler handler))
            return true;
        MagnumPerkRecord perkRecord = handler._perkRecord;
        if (perkRecord is null || handler._isPerkPurchased)
            return true;
        HashSet<string> uniqueItems = new(perkRecord.UpgradePrice);
        if (uniqueItems.Count < 1)
            return true;
        bool add;
        switch (eventData.button)
        {
            default: return true;
            case PointerEventData.InputButton.Right:
                add = true;
                break;
            case PointerEventData.InputButton.Middle:
                add = false;
                break;
        }
        SingletonMonoBehaviour<SoundController>.Instance.PlayUiSound(SingletonMonoBehaviour<SoundsStorage>.Instance.ButtonClick);
        foreach (string item in uniqueItems)
        {
            if (add)
                FavoritesHelper.AddFavorite(item);
            else
                FavoritesHelper.RemoveFavorite(item);
        }
        handler.OnPointerExit(eventData);
        handler.OnPointerEnter(eventData);
        return false;
    }

}
