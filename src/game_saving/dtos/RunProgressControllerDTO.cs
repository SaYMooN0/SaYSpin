using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts.run_progress;

namespace SaYSpin.src.game_saving.dtos
{
    public record class RunProgressControllerDTO
    {
        public string RoadMapString { get; init; }
        public static RunProgressControllerDTO FromRunProgressController(RunProgressController runProgressController)
        {
            var roadMapString = string.Join(";", runProgressController.BeforeStageActionsRoadMap.Select(stage =>
            {
                var actionsString = string.Join("", stage.Actions.Select(a => ((int)a).ToString()));
                return $"{(int)stage.ActionType}[{actionsString}]";
            }));

            return new RunProgressControllerDTO { RoadMapString = roadMapString };
        }

        public RunProgressController ToRunProgressController()
        {
            var roadMap = RoadMapString.Split(';', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(stageString =>
                                      {
                                          var groupType = (BeforeStageActionGroupType)int.Parse(stageString.Substring(0, 1));
                                          var actionsString = stageString.Substring(2, stageString.Length - 3); // Removes brackets and group type
                                          var actions = actionsString.Select(c => (BeforeStageActionType)int.Parse(c.ToString())).ToArray();
                                          return new BeforeStageActionsGroup(actions, groupType);
                                      })
                                      .ToList();

            return new RunProgressController(roadMap);
        }
    }
}
