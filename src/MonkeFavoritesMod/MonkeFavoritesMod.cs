using System.Linq;
using System.Reflection;
using HarmonyLib;
using MonkeFavoritesMod.Helpers;

namespace MonkeFavoritesMod;

public static class MonkeFavoritesMod
{
    public static string ModName = "MonkeFavoritesMod";

    
    [Hook(ModHookType.BeforeBootstrap)]
    public static void BeforeBootstrap(IModContext context)
    {
        new Harmony(ModName).PatchAll(Assembly.GetExecutingAssembly());
        Debug.Log($"{ModName} Loaded");
    }

    [Hook(ModHookType.AfterConfigsLoaded)]
    public static void AfterConfigsLoaded(IModContext context)
    {
        // foreach(var rr in MGSC.Data.Keybinding.Records.ToArray().Skip(1)) MGSC.Data.Keybinding.RemoveRecord(rr.Id);
        GameKeyRecord r = new()
        {
            Id = Hotkeys.TOGGLE_FAVORITE,
            Layout = ModName, // ["", "UI", "Hotkeys"] or my own tab
            AxisName = "",
            Bind1 = [KeyCode.None],
            Bind2 = [KeyCode.None],
            ControllerBind1 = [ControllerAction.None],
            ControllerBind2 = [ControllerAction.None],
            ExclusiveInputMode = [InputController.InputMode.KeyboardAndMouse],
            ForbiddenKeysToBind = [],
            OtherKeyIdToPress = ""
        };
        MGSC.Data.Keybinding.AddRecord(r.Id, r);
    }
    
}
