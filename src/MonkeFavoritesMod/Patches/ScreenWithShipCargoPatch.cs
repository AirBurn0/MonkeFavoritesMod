using HarmonyLib;
using MonkeFavoritesMod.Helpers;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(ScreenWithShipCargo))]
class ScreenWithShipCargoPatch
{

    [HarmonyPatch("DragControllerShowContextMenuCallback"), HarmonyPostfix]
    static void DragControllerShowContextMenuCallbackPostfix(ItemSlot obj)
    {
        string? Id = obj?.Item?.Id as string;
        if(Id is null)
            return;
        CommonContextMenu contextMenu = UI.Get<CommonContextMenu>();
        if (!FavoritesHelper.ShouldBeMarked(Id)) 
            contextMenu.SetupCommand(Localization.Get($"ui.context.{nameof(Commands.AddFavorite)}"), Commands.AddFavorite);
        else
            contextMenu.SetupCommand(Localization.Get($"ui.context.{nameof(Commands.RemoveFavorite)}"), Commands.RemoveFavorite);
        contextMenu.InitSize(0);
    }

    [HarmonyPatch("ContextMenuOnCmdSelected"), HarmonyPrefix]
    static void ContextMenuOnCmdSelectedPrefix(ScreenWithShipCargo __instance, int bindValue)
    {
        string? Id = __instance?._contextMenuItemSlot?.Item?.Id as string;
        if(Id is null)
            return;
        switch (bindValue)
        {
            default: return;
            case Commands.AddFavorite: FavoritesHelper.AddFavorite(Id); break;
            case Commands.RemoveFavorite: FavoritesHelper.RemoveFavorite(Id); break;
        }
    }

}
