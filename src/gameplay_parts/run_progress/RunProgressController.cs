using SaYSpin.src.enums;

namespace SaYSpin.src.gameplay_parts.run_progress
{
    public class RunProgressController
    {
        public RunProgressController(Difficulty difficulty)
        {
            BeforeStageActionsRoadMap = DefaultRoadMap();
            // TODO: change BeforeStageActionsRoadMap according to difficulty
        }
        public List<BeforeStageActionsGroup> BeforeStageActionsRoadMap { get; init; }

        public BeforeStageActionsGroup GetNewStageActionsGroup(int newStageNumber) =>
            newStageNumber >= BeforeStageActionsRoadMap.Count ?
            new([], BeforeStageActionGroupType.None) :
            BeforeStageActionsRoadMap[newStageNumber];

        private List<BeforeStageActionsGroup> DefaultRoadMap()
        {
            BeforeStageActionsGroup emptyGroup = new([], BeforeStageActionGroupType.None);
            BeforeStageActionsGroup itemsAndRelicsGroup = new BeforeStageActionsGroup([BeforeStageActionType.TileItemChoosing, BeforeStageActionType.RelicChoosing], BeforeStageActionGroupType.All);


            List<BeforeStageActionsGroup> map = [emptyGroup, emptyGroup];
            map.AddRange(Enumerable.Repeat(itemsAndRelicsGroup, 50));

            for (int i = 1; i < 5; i++)
            {
                map[i * 10] = new BeforeStageActionsGroup([i % 2 == 0 ? BeforeStageActionType.AddRow : BeforeStageActionType.AddColumn], BeforeStageActionGroupType.All);
            }
            for (int i = 1; i < 3; i++)
            {
                map[i * 13] = new BeforeStageActionsGroup([
                    BeforeStageActionType.RelicChoosing,
                    i%2==0 ? BeforeStageActionType.AddRow : BeforeStageActionType.AddColumn,
                    BeforeStageActionType.StatChoosing], BeforeStageActionGroupType.OneOf);
            }
            return map;
        }
    }
}
