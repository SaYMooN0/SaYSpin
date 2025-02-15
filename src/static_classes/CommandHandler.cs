﻿using SaYSpin.src.extension_classes;
using SaYSpin.src.game_logging;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.gameplay_parts.game_flow_controller;
using SaYSpin.src.gameplay_parts.inventory_related.tokens;
using SaYSpin.src.game_saving;

namespace SaYSpin.src.static_classes
{
    public static class CommandHandler
    {
        private static readonly Dictionary<string, Func<string[], GameFlowController, GameLogModel>> commandMap
          = new()
          {
              ["infoR"] = HandleInfoR,
              ["infoI"] = HandleInfoI,
              ["addI"] = HandleAddI,
              ["addR"] = HandleAddR,
              ["delI"] = HandleDelI,
              ["delR"] = HandleDelR,
              ["commands"] = (args, game) => HandleAvailableCommands(game),
              ["delAllR"] = HandleDelAllR,
              ["delAllI"] = HandleDelAllI,
              ["clearInv"] = (args, game) => HandleClearInventory(game),
              ["setCoins"] = HandleSetCurrentCoins,
              ["setDiamonds"] = HandleSetCurrentDiamonds,
              ["stats"] = (args, game) => HandleGetStats(game),
              ["refreshShop"] = (args, game) => HandleRefreshShop(game),
              ["addT"] = HandleAddToken,
              ["save"] = (args, game) => HandleSave(game),
              ["addRow"] = (args, game) => HandleAddRow(game),
              ["addColumn"] = (args, game) => HandleAddColumn(game),
          };

    

        private static readonly HashSet<string> cheatRequiredCommands = [
            "addI",
            "addR",
            "delI",
            "delR",
            "delAllR",
            "delAllI",
            "clearInv",
            "setCoins",
            "setDiamonds",
            "refreshShop",
            "addT",
            "addRow",
            "addColumn"
        ];

        public static GameLogModel HandleCommand(string command, GameFlowController game)
        {
            if (string.IsNullOrEmpty(command) || !command.StartsWith('/') || game is null)
                return GameLogModel.CommandError("Unable to handle");

            var parts = command.Remove(0, 1).Split("--");
            var cmd = parts[0].Trim();

            if (cheatRequiredCommands.Contains(cmd) && !game.AreCheatsEnabled)
                return GameLogModel.CommandError("Cheats are disabled");

            var args = parts[1..].Select(a => a.Trim()).ToArray();

            if (commandMap.TryGetValue(cmd, out var handler))
            {
                return handler(args, game);
            }

            return GameLogModel.CommandError($"Unknown command {cmd}");
        }
        private static GameLogModel NotEnoughArgs(string cmdName, int expected, int given) =>
            GameLogModel.CommandError($"Command {cmdName} requires {expected} arguments, but {given} were given");
        public static GameLogModel HandleAvailableCommands(GameFlowController game)
        {
            IEnumerable<string> availableCommands = game.AreCheatsEnabled ?
                commandMap.Keys :
                commandMap.Keys.Where(cmd => !cheatRequiredCommands.Contains(cmd));
            return GameLogModel.CommandSuccess(string.Join("\n", availableCommands));
        }
        private static GameLogModel HandleInfoR(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("infoR", 1, 0);

            Relic? r = game.RelicWithId(args[0]);
            if (r is null) return GameLogModel.CommandError($"There is no relic with ID '{args[0]}'");

            return GameLogModel.CommandSuccess(r.TextInfo());
        }
        private static GameLogModel HandleInfoI(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("infoI", 1, 0);

            TileItem? ti = game.TileItemWithId(args[0]);
            if (ti is null) return GameLogModel.CommandError($"There is no tile item with ID '{args[0]}'");

            return GameLogModel.CommandSuccess(ti.TextInfo());
        }

        private static GameLogModel HandleAddI(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("addI", 1, 0);

            TileItem? ti = game.TileItemWithId(args[0]);
            if (ti is null) return GameLogModel.CommandError($"There is no tile item with ID '{args[0]}'");

            game.AddTileItemToInventory(ti);
            return GameLogModel.CommandSuccess($"Added '{ti.Name}' to inventory");
        }

        private static GameLogModel HandleAddR(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("addR", 1, 0);

            Relic? r = game.RelicWithId(args[0]);
            if (r is null) return GameLogModel.CommandError($"There is no relic with ID '{args[0]}'");

            game.AddRelicToInventory(r);
            return GameLogModel.CommandSuccess($"Added relic '{r.Name}' to inventory");
        }

        private static GameLogModel HandleDelI(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("delI", 1, 0);

            string tileItemId = args[0];
            bool success = game.RemoveTileItemFromInventoryById(tileItemId);
            return success ? GameLogModel.CommandSuccess($"Removed '{tileItemId}' from inventory") : GameLogModel.CommandError($"Could not remove '{tileItemId}' from inventory");
        }

        private static GameLogModel HandleDelR(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("delR", 1, 0);

            string relicId = args[0];
            bool success = game.RemoveRelicFromInventoryById(relicId);
            return success ? GameLogModel.CommandSuccess($"Removed relic '{relicId}' from inventory") : GameLogModel.CommandError($"Could not remove relic '{relicId}' from inventory");
        }

        private static GameLogModel HandleDelAllR(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("delAllR", 1, 0);

            string relicId = args[0];

            int countRemoved = 0;
            bool removed;

            do
            {
                removed = game.RemoveRelicFromInventoryById(relicId);
                if (removed) countRemoved++;
            }
            while (removed);

            return countRemoved > 0
                ? GameLogModel.CommandSuccess($"Removed '{countRemoved}' relics with ID '{relicId}' from inventory")
                : GameLogModel.CommandError($"No relics with ID '{relicId}' found in inventory");
        }

        private static GameLogModel HandleDelAllI(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("delAllI", 1, 0);

            string tileItemId = args[0];

            int countRemoved = 0;
            bool removed;

            do
            {
                removed = game.RemoveTileItemFromInventoryById(tileItemId);
                if (removed) countRemoved++;
            }
            while (removed);

            return countRemoved > 0
                ? GameLogModel.CommandSuccess($"Removed {countRemoved} tile items with ID '{tileItemId}' from inventory")
                : GameLogModel.CommandError($"No tile items with ID '{tileItemId}' found in inventory");
        }

        private static GameLogModel HandleClearInventory(GameFlowController game)
        {
            game.Inventory.TileItems.Clear();
            game.Inventory.Relics.Clear();
            game.Inventory.Tokens.Clear();
            return GameLogModel.CommandSuccess("Inventory cleared");
        }
        private static GameLogModel HandleSetCurrentCoins(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("setCoins", 1, 0);

            if (uint.TryParse(args[0], out uint count))
            {
                game.SetCurrentCoinsCount((int)count);
                return GameLogModel.CommandSuccess($"Current coins set to {count}");
            }
            return GameLogModel.CommandError($"Unable to parse {args[0]}");
        }
        private static GameLogModel HandleSetCurrentDiamonds(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("setDiamonds", 1, 0);

            if (uint.TryParse(args[0], out uint count))
            {
                game.Inventory.ChangeDiamondsCount((currentCount) => currentCount = ((int)count));
                return GameLogModel.CommandSuccess($"Current diamonds set to {count}");
            }
            return GameLogModel.CommandError($"Unable to parse {args[0]}");
        }

        private static GameLogModel HandleGetStats(GameFlowController game) =>
            GameLogModel.CommandSuccess(string.Join("\n", game.StatsTracker.Values.Select(kvp => $"{kvp.Key}: {kvp.Value}")));
        private static GameLogModel HandleRefreshShop(GameFlowController game)
        {
            game.UpdateShopItems();
            return GameLogModel.CommandSuccess("Shop refreshed");
        }
        private static GameLogModel HandleAddToken(string[] args, GameFlowController game)
        {
            if (args.Length == 0) return NotEnoughArgs("addT", 1, 0);
            TokenType? type = args[0] switch
            {
                "shop_r" => TokenType.ShopRefresh,
                "new_stage_r" => TokenType.NewStageItemsRefresh,
                "inv_rem" => TokenType.InventoryItemRemoval,
                _ => null
            };
            if (type is null)
                return GameLogModel.CommandError($"Unknown token type {args[0]}. Available: shop_r, new_stage_r, inv_rem");

            game.Inventory.Tokens.AddToken((TokenType)type);
            return GameLogModel.CommandSuccess($"Successfully added {type} token");
        }
        private static GameLogModel HandleSave(GameFlowController game)
        {
            SavingController.SaveGame(game);
            return GameLogModel.CommandSuccess("Game saved");
        }
        private static GameLogModel HandleAddRow(GameFlowController game)
        {
            game.SlotMachine.IncreaseRowsCount();
            return GameLogModel.CommandSuccess("Row added");
        }
        private static GameLogModel HandleAddColumn(GameFlowController game)
        {
            game.SlotMachine.IncreaseColumnsCount();
            return GameLogModel.CommandSuccess("Column added");
        }
    }

}
