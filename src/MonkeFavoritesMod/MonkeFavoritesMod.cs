using System.Reflection;
using HarmonyLib;

namespace MonkeFavoritesMod;

public static class MonkeFavoritesMod
{
    public static string ModName = "MonkeFavoritesMod";
    
    [Hook(ModHookType.AfterConfigsLoaded)]
    public static void AfterConfigLoaded(IModContext context)
    {
        new Harmony(ModName).PatchAll(Assembly.GetExecutingAssembly());
    }
    
}
