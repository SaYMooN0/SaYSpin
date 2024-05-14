using SaYSpin.src.enums;

namespace SaYSpin.src.gameplay_parts.run_progress
{
    public record BeforeStageActionsGroup(
        BeforeStageActionType[] Actions,
        BeforeStageActionGroupType ActionType
        );
    public enum BeforeStageActionGroupType
    {
        All, OneOf, None
    }
}
