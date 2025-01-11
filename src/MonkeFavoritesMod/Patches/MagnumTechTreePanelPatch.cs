using System.Collections.Generic;
using HarmonyLib;
using MonkeFavoritesMod.Helpers;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(MagnumTechTreePanel))]
class MagnumTechTreePanelPatch
{

    [HarmonyPatch(nameof(MagnumTechTreePanel.Initialize), new[] { typeof(MagnumPerkRecord), typeof(bool), typeof(bool) }), HarmonyPostfix]
    static void InitializePostfix(MagnumPerkTooltipHandler ____magnumPerkTooltipHandler, CommonButton ____button, MagnumPerkRecord perkRecord, bool isPerkPurchased, bool canUpgrade)
    {
        if (isPerkPurchased && perkRecord == null)
            return;
        HashSet<string> uniqueItems = new(perkRecord.UpgradePrice);
        if (uniqueItems.Count < 1)
            return;
        ____button.OnClick += (button, clicks) =>
        {
            if (clicks > 0)
                return;
            ____button.interactable = true;
            bool add = clicks < 0;
            foreach (string item in uniqueItems)
            {
                if (add)
                    FavoritesHelper.AddFavorite(item);
                else
                    FavoritesHelper.RemoveFavorite(item);
            }
            ____magnumPerkTooltipHandler.Initialize(perkRecord, isPerkPurchased, showStatus: true, showDefaultValues: true);
        };
    }

}
