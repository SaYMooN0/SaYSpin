using SaYSpin.src.abstract_classes;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.gameplay_parts.inventory_related;

namespace SaYSpin.src.singletons
{
    public class AppMainController
    {
        public AppMainController()
        {

            OrdinaryTileItem t1 = new("1", "medal 1", "1.png", Rarity.Common, 1);
            OrdinaryTileItem t2 = new("2", "medal 2", "2.png", Rarity.Common, 2);
            OrdinaryTileItem t3 = new("3", "medal 3", "3.png", Rarity.Common, 3);
            OrdinaryTileItem t4 = new("4", "medal 3", "3.png", Rarity.Common, 3);
            OrdinaryTileItem t5 = new("5", "medal 3", "3.png", Rarity.Rare, 3);

            AllTileItemsCollection = [t1, t2, t3, t4, t5];

            AllRelicsCollection = [];

            Difficulty normalDifficulty = new("normal", [], [], "normal.png", 1, 1, 10, 3, 1, 1);
            Difficulty hardDifficulty = new("hard",
                ["Receive 1,2 x more rubies after run", "Receive 1,2 x more exp after run"],
                ["More coins are needed to complete each stage", "Increased prices in the shop"],
                "normal.png", 1.2, 1.2, 10, 3, 1.2, 1.2);
            PossibleDifficulties = [normalDifficulty, hardDifficulty];
        }

        public GameplayController? Game { get; private set; }
        public BaseTileItem[] AllTileItemsCollection { get; init; }
        public BaseRelic[] AllRelicsCollection { get; init; }
        public Difficulty[] PossibleDifficulties { get; init; }

        public bool IsGameRunning() => Game is not null;
        public void StartNewGame(List<BaseTileItem> startingItems, List<BaseRelic> startingRelics, Difficulty difficulty)
        {
            Game = new(startingItems, startingRelics, difficulty);
        }
        
        public void GameEnded()
        {
            Logger.Log("game ended");
            Game = null;
        }
        public List<GameStarterKit> GenerateStarterKits(Difficulty difficulty)
        {
            return new();
        }
    }
}
