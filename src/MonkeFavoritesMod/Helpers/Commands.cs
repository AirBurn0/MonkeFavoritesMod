namespace MonkeFavoritesMod.Helpers;

class Commands
{
    
    public const int
        AddFavorite = 101,
        RemoveFavorite = 102;

    public static void SetupModCommands(string? Id)
    {
        if(Id is null)
            return;
        CommonContextMenu contextMenu = UI.Get<CommonContextMenu>();
        if (!FavoritesHelper.ShouldBeMarked(Id)) 
            contextMenu.SetupCommand(Localization.Get($"ui.context.{nameof(AddFavorite)}"), AddFavorite);
        else
            contextMenu.SetupCommand(Localization.Get($"ui.context.{nameof(RemoveFavorite)}"), RemoveFavorite);
        if(!UI.IsShowing<CommonContextMenu>()) {
            UI.Chain<CommonContextMenu>().Show(true).SetBackgroundOrder(-1).SetBackOnBackgroundClick(true);
            return;
        }
        contextMenu.InitSize(0);
    }

    public static bool ExecuteModCommands(string? Id, int bindValue)
    {
        if(Id is null)
            return true;
        switch (bindValue)
        {
            default: return true;
            case AddFavorite: FavoritesHelper.AddFavorite(Id); return false;
            case RemoveFavorite: FavoritesHelper.RemoveFavorite(Id); return false;
        }
    }

}