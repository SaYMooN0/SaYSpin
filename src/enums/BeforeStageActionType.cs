namespace SaYSpin.src.enums
{
    public enum BeforeStageActionType
    {
        ColumnAdded,
        RowAdded,
        TileItemChoosing,
        RelicChoosing,
        TokenChoosing,
        StatChoosing,
        OneOfActionsChoosing
    }
    public static class BeforeStageActionTypeExtensions
    {
        public static string GetFullImagePath(this BeforeStageActionType action) =>
            $"/resources/images/before_stage_actions/{action.ImageName()}.png";
        public static string ImageName(this BeforeStageActionType action) => action switch
        {
            BeforeStageActionType.ColumnAdded => "column_added",
            BeforeStageActionType.RowAdded => "row_added",
            BeforeStageActionType.TileItemChoosing => "tile_item_choosing",
            BeforeStageActionType.RelicChoosing => "relic_choosing",
            BeforeStageActionType.TokenChoosing => "token_choosing",
            BeforeStageActionType.StatChoosing => "stat_choosing",
            _ => throw new ArgumentException("Unsupported action type"),
        };
        public static string Name(this BeforeStageActionType action) => action switch
        {
            BeforeStageActionType.ColumnAdded => "Add one column",
            BeforeStageActionType.RowAdded => "Add one row",
            BeforeStageActionType.TileItemChoosing => "Choose a tile item",
            BeforeStageActionType.RelicChoosing => "Choose a relic",
            BeforeStageActionType.TokenChoosing => "Choose a token",
            BeforeStageActionType.StatChoosing => "Choose a stat to improve",
            BeforeStageActionType.OneOfActionsChoosing => "Choose one of actions",
            _ => throw new ArgumentException("Unsupported action type"),
        };
    }
}
