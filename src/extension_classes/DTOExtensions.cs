using SaYSpin.src.game_saving.dtos;
using SaYSpin.src.gameplay_parts.inventory_related;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.gameplay_parts.inventory_related.tokens;
using SaYSpin.src.gameplay_parts.game_flow_controller;
using static SaYSpin.src.gameplay_parts.game_flow_controller.GameFlowController;
using System.Collections.ObjectModel;
namespace SaYSpin.src.extension_classes
{
    internal static class DTOExtensions
    {
        public static GameFlowControllerDTO GameFlowControllerToDTO(this GameFlowController gameFlowController) =>
            new GameFlowControllerDTO(
                gameFlowController.AllTileItemsCollection.Select(ti => ti.Name).ToArray(),
                gameFlowController.AllRelicsCollection.Select(r => r.Name).ToArray(),
                gameFlowController.Difficulty,
                gameFlowController.SlotMachine,
                gameFlowController.Inventory.ToDTO(),
                gameFlowController.SpinsLeft,
                gameFlowController.CurrentStage,
                gameFlowController.CoinsCount,
                gameFlowController.CoinsNeededToCompleteTheStage,
                gameFlowController.RunProgressController,
                gameFlowController.StatsTracker,
                gameFlowController.Shop,
                gameFlowController.AreCheatsEnabled
                );


        public static GameFlowController GameFlowControllerFromDTO(
            this GameFlowControllerDTO dto,
            BeforeNewStageDialogDelegate ShowBeforeStageDialog,
            Dictionary<string, Func<TileItem>> tileItemConstructors,
            Dictionary<string, Func<Relic>> relicConstructors)
        {
            tileItemConstructors = tileItemConstructors.Where(tiFunc => dto.AllTileItemsCollection.Contains(tiFunc.Key)).ToDictionary();
            relicConstructors = relicConstructors.Where(rFunc => dto.AllRelicsCollection.Contains(rFunc.Key)).ToDictionary();

            GameFlowController gameFlowController = new(
                tileItemConstructors,
                relicConstructors,
                dto.Difficulty,
                dto.SlotMachine,
                dto.Inventory.FromDTO(tileItemConstructors, relicConstructors),
                dto.SpinsLeft,
                dto.CurrentStage,
                dto.CoinsCount,
                dto.CoinsNeededToCompleteTheStage,
                dto.RunProgressController,
                dto.StatsTracker,
                dto.Shop,
                dto.AreCheatsEnabled,
                ShowBeforeStageDialog
                );
            return gameFlowController;
        }
        private static InventoryDTO ToDTO(this Inventory inventory)
        {
            return new InventoryDTO
            {
                TileItems = inventory.TileItems
                    .Select(ti => new TileItemDTO { Name = ti.Name, Counter = (ti is TileItemWithCounter tiWithCounter) ? tiWithCounter.Counter : 0 })
                    .ToList(),
                Relics = inventory.Relics
                    .Select(r => new RelicDTO { Name = r.Name, Counter = (r is RelicWithCounter rWithCounter) ? rWithCounter.Counter : 0 })
                    .ToList(),
                NewStageItemsRefreshTokensCount = inventory.Tokens.Count(TokenType.NewStageItemsRefresh),
                ShopRefreshTokensCount = inventory.Tokens.Count(TokenType.ShopRefresh),
                TileItemRemovalTokensCount = inventory.Tokens.Count(TokenType.InventoryItemRemoval),
                DiamondsCount = inventory.DiamondsCount
            };
        }

        private static Inventory FromDTO(
            this InventoryDTO inventoryDTO,
            IDictionary<string, Func<TileItem>> tileItemConstructors,
            IDictionary<string, Func<Relic>> relicConstructors)
        {
            var tokens = new TokensCollection(
                newStageItemsRefreshTokensCount: inventoryDTO.NewStageItemsRefreshTokensCount,
                shopRefreshTokensCount: inventoryDTO.ShopRefreshTokensCount,
                tileItemRemovalTokensCount: inventoryDTO.TileItemRemovalTokensCount);

            var tileItems = inventoryDTO.TileItems.Select(dto =>
            {
                TileItem ti = tileItemConstructors[dto.Name]();
                if (ti is TileItemWithCounter tiWithCounter)
                {
                    if (dto.Counter is int counter)
                        return TileItemWithCounter.WithCounterValue(tiWithCounter, counter);
                    else
                        throw new ArgumentException("Counter is not an int");
                }
                return ti;
            }).ToList();

            var relics = inventoryDTO.Relics.Select(dto =>
            {
                Relic rel = relicConstructors[dto.Name]();
                if (rel is RelicWithCounter relicWithCounter)
                {
                    if (dto.Counter is int counter)
                        return RelicWithCounter.WithCounterValue(relicWithCounter, counter);
                    else
                        throw new ArgumentException("Counter is not an int");
                }
                return rel;
            }).ToList();

            return new Inventory(tileItems, relics, tokens, inventoryDTO.DiamondsCount);
        }
    }
}
