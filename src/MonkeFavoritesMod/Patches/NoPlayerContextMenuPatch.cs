using System.Collections.Generic;
using HarmonyLib;
using MonkeFavoritesMod.Helpers;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(NoPlayerContextMenu))]
class NoPlayerContextMenuPatch
{
    private static readonly ContextMenuCommand
    AddFavorite = (ContextMenuCommand)101,
    RemoveFavorite = (ContextMenuCommand)102;

    [HarmonyPatch("InitCommands"), HarmonyPostfix]
    static void InitCommandsPostfix(NoPlayerContextMenuPatch __instance, BasePickupItem ____item)
    {
        if (!FavoritesHelper.ShouldBeMarked(____item?.Id))
            Traverse.Create(__instance).Method("SetupCommand", AddFavorite).GetValue();
        else
            Traverse.Create(__instance).Method("SetupCommand", RemoveFavorite).GetValue();
    }

    [HarmonyPatch("OnContextCommandClick"), HarmonyPrefix]
    static void OnContextCommandClickPrefix(Dictionary<CommonButton, ContextMenuCommand> ____commandBinds, BasePickupItem ____item, CommonButton button, int clickCount)
    {
        if (____item?.Id is null)
        {
            return;
        }
        ContextMenuCommand bind = ____commandBinds[button];
        if (bind == AddFavorite)
        {
            FavoritesHelper.AddFavorite(____item.Id);
        }
        if (bind == RemoveFavorite)
        {
            FavoritesHelper.RemoveFavorite(____item.Id);
        }
        ____item.Storage.RecalculateWeight();
    }

}
