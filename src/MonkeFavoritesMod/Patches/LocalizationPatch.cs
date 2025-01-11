using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using SimpleJSON;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(Localization))]
class LocalizationPatch
{
    [HarmonyPatch("LoadDB"), HarmonyPostfix]
    static void LoadDBPostfix(Dictionary<Localization.Lang, Dictionary<string, string>> ___db)
    {
        foreach (Localization.Lang lang in Enum.GetValues(typeof(Localization.Lang)))
        {
            if (LoadTextFile(lang.ToString()) is not JSONNode loc) {
                continue;
            }
            foreach (var pair in loc)
            {
                if(lang != Localization.Lang.EnglishUS) {
                    ___db[lang][pair.Key] = pair.Value;
                    continue;
                }
                foreach (Localization.Lang lang2 in Enum.GetValues(typeof(Localization.Lang))) {
                    if(!___db[lang2].ContainsKey(pair.Key))
                        ___db[lang2][pair.Key] = pair.Value;
                }
            }
        }
    }

    static JSONNode? LoadTextFile(string lang)
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(MonkeFavoritesMod)).Location), "lang", $"{lang}.json");
        Debug.Log("LoadTextFile " + path);
        if (!File.Exists(path))
        {
            Debug.LogWarning("No file " + path);
            return null;
        }
        try
        {
            using StreamReader r = new(path);
            return JSON.Parse(r.ReadToEnd());
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed read file " + ex.Message);
        }
        return null;
    }

}
