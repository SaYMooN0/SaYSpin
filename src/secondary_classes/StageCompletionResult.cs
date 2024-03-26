namespace SaYSpin.src.secondary_classes
{
    public record class StageCompletionResult(
        int StageCompletedNumber,
        int CoinsNeeded,
        int CoinsRecieved,
        int ExtraCoins,
        int SpinsLeft,
        int DiamondsForExtraCoins,
        int DiamondsForExtraSpins,
        int TotalDiamondsToAdd
        );
}
