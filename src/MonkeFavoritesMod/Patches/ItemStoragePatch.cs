using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MonkeFavoritesMod.Compatibility;
using MonkeFavoritesMod.Helpers;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(ItemStorage))]
class ItemStoragePatch
{
    private static bool Apply => MCMCompat.Config?.MessyFavoritesFristSort ?? false;
    
    class MonkeFavoriteType
    {    
    }

    static Type SortWithExpandByTypeAndName_GetType_Patch(Type type, BasePickupItem basePickupItem)
    {
        return (Apply && FavoritesHelper.IsFavorite(basePickupItem?.Id)) ? typeof(MonkeFavoriteType) : type;
    }

    static int SortWithExpandByTypeAndName_get_InventorySortOrder_Patch(int InventorySortOrder, Type type)
    {
        return (Apply && type.Equals(typeof(MonkeFavoriteType))) ? int.MinValue : InventorySortOrder;
    }

    static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(ItemStorage), nameof(ItemStorage.SortWithExpandByTypeAndName));
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchEndForward(
                new CodeMatch(i => i.opcode == OpCodes.Callvirt && ((MethodInfo)i.operand).Name == nameof(System.Object.GetType)) //callvirt	instance class [mscorlib]System.Type [mscorlib]System.Object::GetType()
            )
            .Advance(1)
            .InsertAndAdvance(
                new CodeInstruction(OpCodes.Ldloc_S, 7),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ItemStoragePatch), nameof(SortWithExpandByTypeAndName_GetType_Patch)))
            )
            .MatchEndForward(
                new CodeMatch(i => i.opcode == OpCodes.Callvirt && ((MethodInfo)i.operand).Name == "get_InventorySortOrder") //callvirt	instance int32 MGSC.BasePickupItemRecord::get_InventorySortOrder()
            )
            .Advance(1)
            .Insert(
                new CodeInstruction(OpCodes.Ldloc_S, 10),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ItemStoragePatch), nameof(SortWithExpandByTypeAndName_get_InventorySortOrder_Patch)))
            )
            .InstructionEnumeration();
    }

}
