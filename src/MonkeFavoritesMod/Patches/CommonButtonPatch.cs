using System.Collections.Generic;
using HarmonyLib;
using MonkeFavoritesMod.Helpers;
using UnityEngine.EventSystems;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(CommonButton))]
class CommonButtonPatch
{
    [HarmonyPatch(nameof(CommonButton.OnPointerClick), new[] { typeof(PointerEventData) }), HarmonyPrefix]
    static bool InitializePrefix(CommonButton __instance, ref PointerEventData eventData)
    {
        if (!__instance.interactable)
            return true;
        MagnumPerkTooltipHandler handler = __instance.gameObject.GetComponent<MagnumPerkTooltipHandler>();
        if (Traverse.Create(handler).Field("_perkRecord").GetValue() is not MagnumPerkRecord perkRecord)
            return true;
        if (Traverse.Create(handler).Field("_isPerkPurchased").GetValue() is not bool isPerkPurchased || isPerkPurchased)
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
