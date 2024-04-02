namespace SaYSpin.src.in_game_logging_related
{
    public record class GameLogModel(
        string Message,
        GameLogType Type,
        TimeOnly Time)
    {
        public static GameLogModel New(string message, GameLogType type) =>
            new(message, type, TimeOnly.FromDateTime(DateTime.Now));
    }
}
