namespace SaYSpin.src.game_logging
{
    public record class GameLogModel(
        string Message,
        GameLogType Type,
        TimeOnly Time)
    {
        public static GameLogModel New(string message, GameLogType type) =>
            new(message, type, TimeOnly.FromDateTime(DateTime.Now));
        public static GameLogModel CommandSuccess(string commandResult) =>
            New(commandResult, GameLogType.ConsoleCommandSuccess);
        public static GameLogModel CommandError(string commandResult) =>
            New(commandResult, GameLogType.ConsoleCommandError);
    }
}
