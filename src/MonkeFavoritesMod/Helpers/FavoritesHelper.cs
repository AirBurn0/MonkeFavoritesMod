using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace MonkeFavoritesMod.Helpers;

public static class FavoritesHelper
{

    public static Sprite? Icon
    {
        get // icon_stun story_versus
        {
            return _icon ??= Resources.FindObjectsOfTypeAll<Sprite>().Where(s => s.name.Contains("icon_stun")).First();
        }
    }

    private static Sprite? _icon;

    private static List<string> _favorites;

    public static void AddSubIconToSlot(GameObject slut, string? item)
    {
        var parent = slut.GetComponent<RectTransform>();
        if (parent.Find("SubIcon") != null)
        {
            SetSubIconActive(slut, item);
            return;
        }
        GameObject subIcon = new("SubIcon");
        Image image = subIcon.AddComponent<Image>();
        image.sprite = Icon;
        image.color = new Color(1f, 1f, 0f, 1f);
        RectTransform transform = subIcon.GetComponent<RectTransform>();
        transform.SetParent(parent);
        transform.anchoredPosition = new Vector2(-5f, -5f);
        transform.anchorMin = transform.anchorMax = Vector2.one;
        transform.sizeDelta = new Vector2(16f, 16f);
        subIcon.SetActive(ShouldBeMarked(item));
    }

    public static void SetSubIconActive(GameObject slut, string? item)
    {
        var subIcon = slut.GetComponent<RectTransform>().Find("SubIcon").gameObject;
        if (subIcon == null)
        {
            return;
        }
        subIcon.SetActive(ShouldBeMarked(item));
    }

    public static bool ShouldBeMarked(string? item)
    {
        return item is not null && (_favorites?.Contains(item) ?? false);
    }

    public static void AddFavorite(string item)
    {
        if(!_favorites?.Contains(item) ?? false) {
            _favorites?.Add(item);
        }
    }

    public static void RemoveFavorite(string item)
    {
        if(_favorites?.Contains(item) ?? false) {
            _favorites?.Remove(item);
        }
    }

    public static void SetFavorites(List<string> favorites) {
        _favorites = favorites;
    }

}
