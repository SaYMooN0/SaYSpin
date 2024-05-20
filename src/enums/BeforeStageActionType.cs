namespace SaYSpin.src.enums
{
    public enum BeforeStageActionType
    {
        AddColumn,
        AddRow,
        TileItemChoosing,
        RelicChoosing,
        TokenChoosing,
        StatChoosing,
        CloneItem,
        OneOfActionsChoosing,
        
    }
    public static class BeforeStageActionTypeExtensions
    {
        public static string GetFullImagePath(this BeforeStageActionType action) =>
            $"/resources/images/before_stage_actions/{action.ImageName()}.png";
        public static string ImageName(this BeforeStageActionType action) => action switch
        {
            BeforeStageActionType.AddColumn => "add_column",
            BeforeStageActionType.AddRow => "add_row",
            BeforeStageActionType.TileItemChoosing => "tile_item_choosing",
            BeforeStageActionType.RelicChoosing => "relic_choosing",
            BeforeStageActionType.TokenChoosing => "token_choosing",
            BeforeStageActionType.StatChoosing => "stat_choosing",
            BeforeStageActionType.CloneItem => "clone_item",
            _ => throw new ArgumentException("Unsupported action type"),
        };
        public static string Name(this BeforeStageActionType action) => action switch
        {
            BeforeStageActionType.AddColumn => "Add one column",
            BeforeStageActionType.AddRow => "Add one row",
            BeforeStageActionType.TileItemChoosing => "Choose a tile item",
            BeforeStageActionType.RelicChoosing => "Choose a relic",
            BeforeStageActionType.TokenChoosing => "Choose a token",
            BeforeStageActionType.StatChoosing => "Choose stat to improve",
            BeforeStageActionType.OneOfActionsChoosing => "Choose one of actions",
            BeforeStageActionType.CloneItem => "Clone non-unique inventory item",
            _ => throw new ArgumentException("Unsupported action type"),
        };
    }
}
