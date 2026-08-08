using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(KeybindingPage))]
class KeybindingPagePatch
{
    private static GameObject? _modBlockHeader = null;
    private static bool flag3 = false;

    // insert into beginning of function
    static void InitKeysPrefix()
    {
        flag3 = false;
    }

    // insert in loop body after "Hotkeys" label 'if'
    static void InitKeysPatch(KeybindingPage __instance, GameKey gameKey)
    {
        if(gameKey.Record.Layout != MonkeFavoritesMod.ModName || flag3)
            return;
        if(_modBlockHeader == null || !_modBlockHeader)
        {
            var header = UnityEngine.Object.Instantiate(__instance._hotkeysBlockHeader.gameObject);
            header.name = $"{MonkeFavoritesMod.ModName}_ModBlockHeader";
            try
            {
                LocalizableLabel label = header.transform.GetChild(0).GetComponent<LocalizableLabel>();
                label.ChangeLabel($"ui.controls.{MonkeFavoritesMod.ModName}");
            }
            catch (Exception)
            {
                Debug.LogError($"Failed to add {MonkeFavoritesMod.ModName} config label.");
                return;
            }
            header.transform.SetParent(__instance._hotkeysBlockHeader.transform.parent, false);
            _modBlockHeader = header;
        }
        _modBlockHeader.transform.SetAsLastSibling();
        flag3 = true;
    }

    static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(KeybindingPage), nameof(KeybindingPage.InitKeys));
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .Start()
            .Insert(
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(KeybindingPagePatch), nameof(InitKeysPrefix)))
            )
            .MatchStartForward(
                new CodeMatch(OpCodes.Ldfld), //ldfld	class MGSC.Pool MGSC.KeybindingPage::_gameKeyPanelsPool
                new CodeMatch(i => i.opcode == OpCodes.Callvirt && ((MethodInfo)i.operand).Name == nameof(MGSC.Pool.Take)) //callvirt	instance class [UnityEngine.CoreModule]UnityEngine.GameObject MGSC.Pool::Take()
            )
            .Insert(
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldloc_S, Convert.ToByte(6)),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(KeybindingPagePatch), nameof(InitKeysPatch)))
            )
            .InstructionEnumeration();
    }

}
