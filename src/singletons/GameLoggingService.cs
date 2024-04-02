using SaYSpin.src.in_game_logging_related;
using SaYSpin.src.inventory_items;
using System.Collections.Concurrent;

public class GameLoggingService
{
    private readonly ConcurrentQueue<GameLogModel> _logs = new ConcurrentQueue<GameLogModel>();

    public void Log(GameLogModel logEntry)
    {
        if (_logs.Count >= 100)
        {
            _logs.TryDequeue(out _);
        }

        _logs.Enqueue(logEntry);
    }


    public void LogItemAdded(BaseInventoryItem item) =>
        Log(GameLogModel.New($"{item.Name} has been added to inventory", GameLogType.ItemAdded));
    public void LogItemDestroyed(BaseInventoryItem item) =>
        Log(GameLogModel.New($"{item.Name} has been destroyed", GameLogType.ItemDestroyed));

    public IEnumerable<GameLogModel> Logs() => _logs.Reverse();

    public void Clear()=>
        _logs.Clear();
}
