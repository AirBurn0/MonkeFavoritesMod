using System.Reflection;
using HarmonyLib;

namespace MonkeFavoritesMod;

public static class MonkeFavoritesMod
{
    public static string ModName = "MonkeFavoritesMod";
    
    [Hook(ModHookType.BeforeBootstrap)]
    public static void BeforeBootstrap(IModContext context)
    {
        Debug.Log("Mod Launched");
        new Harmony(ModName).PatchAll(Assembly.GetExecutingAssembly());
    }
    
}
