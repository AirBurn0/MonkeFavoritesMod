using System;
using HarmonyLib;
using MonkeFavoritesMod.Helpers;

namespace MonkeFavoritesMod.Patches;

class CommonInventoryPatch
{

    internal static void DragControllerShowContextMenuCallbackPostfix(ItemSlot? obj)
    {
        Commands.SetupModCommands(obj?.Item?.Id);
    }

    internal static bool ContextMenuOnCmdSelectedPrefix(ItemSlot? _contextMenuItemSlot, int bindValue)
    {
        return Commands.ExecuteModCommands(_contextMenuItemSlot?.Item?.Id, bindValue);
    }

    internal static bool ProcessPrefix(out bool interruptProcessing, Action dragCallback)
    {
        interruptProcessing = false;

        SingletonMonoBehaviour<InputController>.Instance._keymaps.TryGetValue("", out var v);
		InputController instance = SingletonMonoBehaviour<InputController>.Instance;
		if (instance.IsKeyDown(Hotkeys.TOGGLE_FAVORITE, MonkeFavoritesMod.ModName))
		{
            DragController drag = UI.Drag;
            if (drag.IsDragging && drag.DraggableItem is BasePickupItem item)
            {
                FavoritesHelper.ToggleFavorite(item.Id);
                drag.ReturnToOriginalStorage();
                drag.ResetDragState();
                dragCallback();
            }
            else if(drag.RaycastSlotUnderCursor() is ItemSlot slot && slot.Item is BasePickupItem item1)
            {
                FavoritesHelper.ToggleFavorite(item1.Id);
            }
            else
                return true;
			interruptProcessing = true;
			return false;
		}
        return true;
    }

}

[HarmonyPatch(typeof(InventoryScreen))]
class InventoryScreenPatch
{

    [HarmonyPatch(nameof(InventoryScreen.DragControllerShowContextMenuCallback)), HarmonyPostfix]
    static void DragControllerShowContextMenuCallbackPostfix(ItemSlot obj)
    {
        CommonInventoryPatch.DragControllerShowContextMenuCallbackPostfix(obj);
    }

    [HarmonyPatch(nameof(InventoryScreen.ContextMenuOnCmdSelected)), HarmonyPrefix]
    static bool ContextMenuOnCmdSelectedPrefix(InventoryScreen __instance, int bindValue)
    {
        return CommonInventoryPatch.ContextMenuOnCmdSelectedPrefix(__instance?._contextMenuItemSlot, bindValue);
    }

    [HarmonyPatch(nameof(InventoryScreen.Process)), HarmonyPrefix]
    static bool ProcessPrefix(InventoryScreen __instance, out bool interruptProcessing)
    {
        return CommonInventoryPatch.ProcessPrefix(out interruptProcessing, __instance.DragControllerRefreshCallback);
    }

}

[HarmonyPatch(typeof(ScreenWithShipCargo))]
class ScreenWithShipCargoPatch
{

    [HarmonyPatch(nameof(ScreenWithShipCargo.DragControllerShowContextMenuCallback)), HarmonyPostfix]
    static void DragControllerShowContextMenuCallbackPostfix(ItemSlot obj)
    {
        CommonInventoryPatch.DragControllerShowContextMenuCallbackPostfix(obj);
    }

    [HarmonyPatch(nameof(ScreenWithShipCargo.ContextMenuOnCmdSelected)), HarmonyPrefix]
    static bool ContextMenuOnCmdSelectedPrefix(ScreenWithShipCargo __instance, int bindValue)
    {
        return CommonInventoryPatch.ContextMenuOnCmdSelectedPrefix(__instance?._contextMenuItemSlot, bindValue);
    }

    [HarmonyPatch(nameof(ScreenWithShipCargo.Process)), HarmonyPrefix]
    static bool ProcessPrefix(ScreenWithShipCargo __instance, out bool interruptProcessing)
    {
        return CommonInventoryPatch.ProcessPrefix(out interruptProcessing, __instance.RefreshView);
    }

}