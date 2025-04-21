using HarmonyLib;
using MonkeFavoritesMod.Helpers;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(ScreenWithShipCargo))]
class ScreenWithShipCargoPatch
{

    [HarmonyPatch("DragControllerShowContextMenuCallback"), HarmonyPostfix]
    static void DragControllerShowContextMenuCallbackPostfix(ItemSlot obj)
    {
        Commands.SetupModCommands(obj?.Item?.Id as string);
    }

    [HarmonyPatch("ContextMenuOnCmdSelected"), HarmonyPrefix]
    static bool ContextMenuOnCmdSelectedPrefix(ScreenWithShipCargo __instance, int bindValue)
    {
        return Commands.ExecuteModCommands(__instance?._contextMenuItemSlot?.Item?.Id as string, bindValue);
    }

}
