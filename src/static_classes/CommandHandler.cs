

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
        private static readonly Dictionary<string, Func<string[], GameFlowController, GameLogModel>> commandMap
            = new()
            {
                ["infoR"] = HandleInfoR,
                ["infoI"] = HandleInfoI,
                ["addI"] = (args, game) => WithCheatsOnly(game, () => HandleAddI(args, game)),
                ["addR"] = (args, game) => WithCheatsOnly(game, () => HandleAddR(args, game)),
                ["delI"] = (args, game) => WithCheatsOnly(game, () => HandleDelI(args, game)),
                ["delR"] = (args, game) => WithCheatsOnly(game, () => HandleDelR(args, game)),
            };

        public static GameLogModel HandleCommand(string command, GameFlowController game)
        {
            if (string.IsNullOrEmpty(command) || !command.StartsWith("/") || game is null)
                return GameLogModel.CommandError("Unable to handle");

            var parts = command.Remove(0, 1).Split("--");
            var cmd = parts[0].Trim();
            var args = parts[1..].Select(a => a.Trim()).ToArray();

            if (commandMap.TryGetValue(cmd, out var handler))
            {
                return handler(args, game);
            }

            return GameLogModel.CommandError($"Unknown command {cmd}");
        }
        private static GameLogModel HandleInfoR(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("infoR", 1, 0);

            Relic? r = game.RelicWithId(args[0]);
            if (r is null) return GameLogModel.CommandError($"There is no relic with id {args[0]}");

            return GameLogModel.CommandSuccess(r.TextInfo());
        }

        private static GameLogModel HandleInfoI(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("infoI", 1, 0);

            TileItem? ti = game.TileItemWithId(args[0]);
            if (ti is null) return GameLogModel.CommandError($"There is no tile item with id {args[0]}");

            return GameLogModel.CommandSuccess(ti.TextInfo());
        }

        private static GameLogModel HandleAddI(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("addI", 1, 0);

            TileItem? ti = game.TileItemWithId(args[0]);
            if (ti is null) return GameLogModel.CommandError($"There is no tile item with id {args[0]}");

            game.AddTileItemToInventory(ti);
            return GameLogModel.CommandSuccess($"Added {ti.Name} to inventory");
        }

        private static GameLogModel HandleAddR(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("addR", 1, 0);

            Relic? r = game.RelicWithId(args[0]);
            if (r is null) return GameLogModel.CommandError($"There is no relic with id {args[0]}");

            game.AddRelicToInventory(r);
            return GameLogModel.CommandSuccess($"Added relic {r.Name} to inventory");
        }

        private static GameLogModel HandleDelI(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("delI", 1, 0);

            string tileItemId = args[0];
            bool success = game.RemoveTileItemFromInventory(tileItemId);
            return success ? GameLogModel.CommandSuccess($"Removed {tileItemId} from inventory") : GameLogModel.CommandError($"Could not remove {tileItemId} from inventory");
        }

        private static GameLogModel HandleDelR(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("delR", 1, 0);

            string relicId = args[0];
            bool success = game.RemoveRelicFromInventory(relicId);
            return success ? GameLogModel.CommandSuccess($"Removed relic {relicId} from inventory") : GameLogModel.CommandError($"Could not remove relic {relicId} from inventory");
        }


        private static GameLogModel NotEnoughArgs(string cmdName, int expected, int given) =>
            GameLogModel.CommandError($"Command {cmdName} requires {expected} arguments, but {given} were given");

        private static GameLogModel WithCheatsOnly(GameFlowController game, Func<GameLogModel> func) =>
            game.AreCheatsEnabled ? func() : GameLogModel.CommandError("Cheats are disabled");
    }

}
