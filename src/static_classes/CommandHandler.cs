

using SaYSpin.src.extension_classes;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.in_game_logging_related;
using SaYSpin.src.inventory_items;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.static_classes
{
    public static class CommandHandler
    {
        static public GameLogModel HandleCommand(string command, GameFlowController game)
        {
            if (string.IsNullOrEmpty(command) || !command.StartsWith("/") || game is null)
                return GameLogModel.CommandError("Unable to handle");

            var parts = command.Remove(0, 1).Split("--");


            return ProcessCommand(parts[0].Trim(), parts[1..].Select(a=>a.Trim()).ToArray(), game);
        }
        static public GameLogModel ProcessCommand(string cmd, string[] args, GameFlowController game)
        {
            switch (cmd)
            {
                case "infoR":
                    {
                        if (args.Length == 0) return NotEnoughArgs(cmd, 1, 0);

                        Relic? r = game.RelicWithId(args[0]);
                        if (r is null) 
                            return GameLogModel.CommandError($"There is no relic id {args[0]}");

                        return GameLogModel.CommandSuccess(r.TextInfo());
                    }
                case "infoI":
                    {
                        if (args.Length == 0) return NotEnoughArgs(cmd, 1, 0);

                        TileItem? ti= game.TileItemWithId(args[0]);
                        if (ti is null)
                            return GameLogModel.CommandError($"There is no tile item id {args[0]}");
                        return GameLogModel.CommandSuccess(ti.TextInfo());
                    }
                default:
                    return GameLogModel.CommandError($"Unknown command {cmd}");
            }
        }
        private static GameLogModel NotEnoughArgs(string cmdName, int expected, int given) =>
            GameLogModel.CommandError($"Command {cmdName} requires {expected} arguments, but {given} were given");
    }
}
