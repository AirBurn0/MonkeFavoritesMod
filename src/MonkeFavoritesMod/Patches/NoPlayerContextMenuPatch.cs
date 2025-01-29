using HarmonyLib;
using MonkeFavoritesMod.Helpers;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(NoPlayerContextMenu))]
class NoPlayerContextMenuPatch
{

    [HarmonyPatch("InitCommands"), HarmonyPostfix]
    static void InitCommandsPostfix(NoPlayerContextMenu __instance, BasePickupItem ____item)
    {
        if (!FavoritesHelper.ShouldBeMarked(____item?.Id))
            Traverse.Create(__instance).Method("SetupCommand", (ContextMenuCommand)ContextMenuPatch.AddFavorite).GetValue();
        else
            Traverse.Create(__instance).Method("SetupCommand", (ContextMenuCommand)ContextMenuPatch.RemoveFavorite).GetValue();
    }

    [HarmonyPatch("ProcessBind"), HarmonyPrefix]
    static void ProcessBindPrefix(BasePickupItem ____item, int bindValue, bool hideContextMenu)
    {
        if (____item?.Id is null)
        {
            return;
        }
        switch (bindValue)
        {
            default: return;
            case ContextMenuPatch.AddFavorite: FavoritesHelper.AddFavorite(____item.Id); break;
            case ContextMenuPatch.RemoveFavorite: FavoritesHelper.RemoveFavorite(____item.Id); break;
        }
        ____item.Storage.RecalculateWeight();
    }

}
