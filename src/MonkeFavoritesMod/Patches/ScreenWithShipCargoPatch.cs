using HarmonyLib;
using MonkeFavoritesMod.Helpers;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(ScreenWithShipCargo))]
class ScreenWithShipCargoPatch
{

    [HarmonyPatch(nameof(ScreenWithShipCargo.DragControllerShowContextMenuCallback)), HarmonyPostfix]
    static void DragControllerShowContextMenuCallbackPostfix(ItemSlot obj)
    {
        Commands.SetupModCommands(obj?.Item?.Id);
    }

    [HarmonyPatch(nameof(ScreenWithShipCargo.ContextMenuOnCmdSelected)), HarmonyPrefix]
    static bool ContextMenuOnCmdSelectedPrefix(ScreenWithShipCargo __instance, int bindValue)
    {
        return Commands.ExecuteModCommands(__instance?._contextMenuItemSlot?.Item?.Id, bindValue);
    }

}
