using SaYSpin.src.gameplay_parts.inventory_related.tokens;
using SaYSpin.src.game_logging;
using SaYSpin.src.inventory_items;
using System.Collections.Concurrent;

public class GameLoggingService
{
    private readonly ConcurrentQueue<GameLogModel> _logs = new ConcurrentQueue<GameLogModel>();

    public void Log(GameLogModel logEntry)
    {
        if (_logs.Count >= 100)
            _logs.TryDequeue(out _);
        

        _logs.Enqueue(logEntry);
    }


    public void LogItemAdded(BaseInventoryItem item) =>
        Log(GameLogModel.New($"{item.Name} has been added to inventory", GameLogType.ItemAdded));
    public void LogItemDestroyed(BaseInventoryItem item) =>
        Log(GameLogModel.New($"{item.Name} has been destroyed", GameLogType.ItemDestroyed));
    public void LogTokenUsed(TokenType tokenType)=>
        Log(GameLogModel.New($"{tokenType} has been used", GameLogType.TokenUsed));

    public IEnumerable<GameLogModel> Logs() => _logs.Reverse();

    public void Clear()=>
        _logs.Clear();
}
