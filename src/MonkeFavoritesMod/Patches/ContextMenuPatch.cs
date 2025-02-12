using HarmonyLib;
using MonkeFavoritesMod.Helpers;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(MGSC.ContextMenu))]
class ContextMenuPatch
{
    public const int
    AddFavorite = 101,
    RemoveFavorite = 102;

    [HarmonyPatch("InitCommands"), HarmonyPostfix]
    static void InitCommandsPostfix(MGSC.ContextMenu __instance, BasePickupItem ____item)
    {
        if (!FavoritesHelper.ShouldBeMarked(____item?.Id))
            __instance.SetupCommand((ContextMenuCommand)ContextMenuPatch.AddFavorite);
        else
            __instance.SetupCommand((ContextMenuCommand)ContextMenuPatch.RemoveFavorite);
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
            case AddFavorite: FavoritesHelper.AddFavorite(____item.Id); break;
            case RemoveFavorite: FavoritesHelper.RemoveFavorite(____item.Id); break;
        }
        ____item.Storage.RecalculateWeight();
    }

}
