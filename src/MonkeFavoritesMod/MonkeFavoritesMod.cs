using System.Reflection;
using HarmonyLib;

namespace MonkeFavoritesMod;

public static class MonkeFavoritesMod
{
    public static string ModName = "MonkeFavoritesMod";
    
    [Hook(ModHookType.AfterBootstrap)]
    public static void AfterBootstrap(IModContext context)
    {
        Debug.Log("Mod Launched");
        new Harmony(ModName).PatchAll(Assembly.GetExecutingAssembly());
    }
    
}
