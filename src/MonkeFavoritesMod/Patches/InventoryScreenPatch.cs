using HarmonyLib;
using MonkeFavoritesMod.Helpers;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(InventoryScreen))]
class InventoryScreenPatch
{

    [HarmonyPatch("DragControllerShowContextMenuCallback"), HarmonyPostfix]
    static void DragControllerShowContextMenuCallbackPostfix(ItemSlot obj)
    {
        Commands.SetupModCommands(obj?.Item?.Id);
    }

    [HarmonyPatch("ContextMenuOnCmdSelected"), HarmonyPrefix]
    static bool ContextMenuOnCmdSelectedPrefix(InventoryScreen __instance, int bindValue)
    {
        return Commands.ExecuteModCommands(__instance?._contextMenuItemSlot?.Item?.Id, bindValue);
    }

}
