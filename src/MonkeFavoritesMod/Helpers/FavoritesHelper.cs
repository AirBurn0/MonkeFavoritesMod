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
    public static List<string> Favorites
    {
        get => _favorites;
        set => _favorites = value;
    }
    private static List<string> _favorites = [];
    private static readonly List<GameObject> _items = [];

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
        image.color = new(1f, 1f, 0f, 1f);
        RectTransform transform = subIcon.GetComponent<RectTransform>();
        transform.SetParent(parent);
        transform.anchoredPosition = new(-5f, -5f);
        transform.anchorMin = transform.anchorMax = Vector2.one;
        transform.sizeDelta = new(16f, 16f);
        subIcon.SetActive(ShouldBeMarked(item));
        _items.Add(slut);
    }

    public static void SetSubIconActive(GameObject slut, string? item)
    {
        slut.GetComponent<RectTransform>().Find("SubIcon").gameObject?.SetActive(ShouldBeMarked(item));
    }

    public static bool ShouldBeMarked(string? item)
    {
        return item is not null && (_favorites?.Contains(item) ?? false);
    }

    public static void AddFavorite(string item)
    {
        if (!_favorites?.Contains(item) ?? false)
        {
            _favorites?.Add(item);
            Refresh(item);
        }
    }

    public static void RemoveFavorite(string item)
    {
        if (_favorites?.Contains(item) ?? false)
        {
            _favorites?.Remove(item);
            Refresh(item);
        }
    }

    private static void Refresh(string itemName)
    {
        for (int i = _items.Count - 1; i >= 0; --i)
        {
            GameObject slut = _items[i];
            if (slut is null || !slut)
            {
                _items.RemoveAt(i);
                continue;
            }
            if (slut.TryGetComponent(out ItemSlot slot) && slot.Item?.Id == itemName
                || slut.TryGetComponent(out ItemTooltipHandler handler) && (handler._item?.Id == itemName || handler._itemRecord?.Id == itemName)
            )
            {
                SetSubIconActive(slut, itemName);
                continue;
            }
        }
    }

}
