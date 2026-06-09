using MonkeFavoritesMod.Helpers;
using UnityEngine.EventSystems;

namespace MonkeFavoritesMod.Scripts;

public class ItemSlotClickHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
    public ItemTooltipHandler? _itemTooltipHandler;

    public void Initialize(ItemTooltipHandler itemTooltipHandler)
    {
        _itemTooltipHandler = itemTooltipHandler;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_itemTooltipHandler?._itemRecord?.Id is not string item)
            return;
        Debug.Log(item);
        SingletonMonoBehaviour<SoundController>.Instance.PlayUiSound(SingletonMonoBehaviour<SoundsStorage>.Instance.ButtonClick);
        switch (eventData.button)
        {
            default: return;
            case PointerEventData.InputButton.Right:
                FavoritesHelper.AddFavorite(item);
                break;
            case PointerEventData.InputButton.Middle:
                FavoritesHelper.RemoveFavorite(item);
                break;
        }
    }

}
