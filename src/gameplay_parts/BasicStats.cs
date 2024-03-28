namespace SaYSpin.src.gameplay_parts
{
    public class BasicStats
    {

        public double Luck { get; init; }
        public double DiamondsAfterStageCoefficient { get; init; }
        public int NewStageTileItemsForChoiceCount { get; init; }
        public int NewStageRelicsForChoiceCount { get; init; }
        public BasicStats(
            double luck,
            double diamondsAfterStageCoefficient,
            int newStageTileItemsForChoiceCount,
            int newStageRelicsForChoiceCount)
        {
            Luck = luck;
            DiamondsAfterStageCoefficient = diamondsAfterStageCoefficient;
            NewStageTileItemsForChoiceCount = newStageTileItemsForChoiceCount;
            NewStageRelicsForChoiceCount = newStageRelicsForChoiceCount;
        }
        public static BasicStats Default() =>
            new(1, 1, 4, 4);

    }
}
