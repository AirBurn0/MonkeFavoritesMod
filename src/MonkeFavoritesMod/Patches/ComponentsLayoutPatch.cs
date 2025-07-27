using HarmonyLib;
using MonkeFavoritesMod.Helpers;
using MonkeFavoritesMod.Data;
using SimpleJSON;
using System.Collections.Generic;
using System;

namespace MonkeFavoritesMod.Patches;

[HarmonyPatch(typeof(ComponentsLayout))]
class ComponentsLayoutPatch
{

    [HarmonyPatch(nameof(ComponentsLayout.CreateGlobalComponents), [typeof(bool)]), HarmonyPostfix]
    static void CreateGlobalComponentsPostfix(State ____state, bool initContent)
    {
        Favorites favs = new();
        ____state.Resolve(favs);
        if(initContent)
            FavoritesHelper.SetFavorites(favs.Values ?? []);
    }

    [HarmonyPatch(nameof(ComponentsLayout.RemoveGlobalComponents)), HarmonyPostfix]
    static void RemoveGlobalComponentsPostfix(State ____state)
    {
        ____state.Remove<Favorites>();
    }

    [HarmonyPatch(nameof(ComponentsLayout.SerializeGlobalComponents), [typeof(JSONNode)]), HarmonyPrefix]
    static void SerializeGlobalComponentsPrefix(State ____state, JSONNode rootNode)
    {
        JSONNode asArray = rootNode["Components"].AsArray;
        asArray.Add(AlterSaveNode(SaveToJSON.CreateNode(____state.Get<Favorites>())));
    }

    private static JSONNode AlterSaveNode(JSONNode node)
    {
        node["ModType"] = node["Type"];
        node["Type"] = typeof(MGSC.NewsEvent).FullName;
        return node;
    }

    [HarmonyPatch(nameof(ComponentsLayout.DeserializeGlobalComponents), [typeof(JSONNode)]), HarmonyPostfix]
    static void DeserializeGlobalComponentsPostfix(State ____state, JSONNode jsonNode)
    {
        FavoritesHelper.SetFavorites(____state.Get<Favorites>()?.Values ?? []);
    }

    [HarmonyPatch(nameof(ComponentsLayout.DeserializeGlobalComponents), [typeof(JSONNode)]), HarmonyPrefix]
    static void DeserializeGlobalComponentsPrefix(State ____state, ref JSONNode jsonNode)
    {
        Dictionary<Type, JSONNode> typesToNodes = [];
        for (int i = jsonNode["Components"].Count - 1; i >= 0; --i)
        {
            string type = jsonNode["Components"][i]["ModType"];
            if (type is null || !type.Contains("MonkeFavoritesMod."))
                continue;
            typesToNodes.Add(typeof(MonkeFavoritesMod).Assembly.GetType(type), jsonNode["Components"][i]["Content"]);
            jsonNode["Components"].Remove(i);
        }
        LoadComponent<Favorites>();
        void LoadComponent<T>() where T : class
        {
            T val = ____state.Get<T>();
            if (val == null)
            {
                Debug.LogError($"Failed load {typeof(T)} from json, no in state.");
                return;
            }
            if (!typesToNodes.TryGetValue(typeof(T), out var value))
            {
                Debug.LogError($"Failed init {typeof(T)}, no node in json.");
                return;
            }

            val.LoadJSON(AlterLoadNode(value));
        }
    }

    private static JSONNode AlterLoadNode(JSONNode node)
    {
        node["Type"] = node["ModType"];
        return node;
    }

}
