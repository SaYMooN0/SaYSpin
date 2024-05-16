using SaYSpin.src.enums;
using SaYSpin.src.extension_classes;
using SaYSpin.src.extension_classes.factories;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.gameplay_parts.inventory_related.tokens;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.static_classes;

namespace SaYSpin.src.inventory_items_storages
{
    internal class RelicsStorage : IInventoryItemStorage<Relic>
    {
        Dictionary<string, Func<Relic>> _storedItems { get; set; }
        HashSet<string> _availableRelics;

        public RelicsStorage()
        {
            _storedItems = new Dictionary<string, Func<Relic>>
            {
                ["Fruit Basket"] = FruitBasket,
                ["Bird Guide"] = BirdGuide,
                ["Treasure Map"] = TreasureMap,
                ["Golden Key"] = GoldenKey,
                ["Apple Tree"] = AppleTree,
                ["Apple Tree Sapling"] = AppleTreeSapling,
                ["Random Token"] = RandomToken,
                ["Diamond Token"] = DiamondToken,
                ["Ufo"] = UFO,
                ["Globe"] = Globe,
                ["Alarm"] = Alarm,
                ["Four Leaf Clover"] = FourLeafClover,
                ["Milk"] = Milk,
                ["Orange Juice"] = OrangeJuice,
                ["Telescope"] = Telescope,
                ["Bowl Of Candies"] = BowlOfCandies,
                ["Diamond Pass"] = DiamondPass,
                ["Waffle Iron"] = WaffleIron,
                ["Galactic Assembly Medal"] = GalacticAssemblyMedal,
                ["Hourglass"] = Hourglass,
                ["Zoo Tickets"] = ZooTickets,
                ["Piggy Bank"] = PiggyBank,
                ["Bag Of Sour Worms"] = BagOfSourWorms,
                ["Olympic Flag"] = OlympicFlag,
                ["Intergalactic Bubblegum"] = IntergalacticBubbleGum,
                ["Storybook"] = Storybook,
                ["Shipping Containers"] = ShippingContainers,
                ["Safe"] = Safe,
                ["Compass"] = Compass,

            };
            _availableRelics = new HashSet<string>(_storedItems.Keys);
        }



        public Dictionary<string, Func<Relic>> GetAvailableItems() =>
            _storedItems
                .Where(nameItemPair => _availableRelics.Contains(nameItemPair.Key))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

        public void Update()
        {
            throw new NotImplementedException();
        }

        public HashSet<string> InitAvailable()
        {
            throw new NotImplementedException();
        }

        public void SaveToFile()
        {
            throw new NotImplementedException();
        }
        private Relic FruitBasket()
        {
            const int fruitBonus = 1;
            return new Relic("Fruit Basket", Rarity.Common)
                .WithCoinsCalculationRelicEffect($"All fruits give +{fruitBonus} coin", ModifierType.Plus, fruitBonus, i => i.HasTag("fruit"));
        }
        private Relic BirdGuide()
        {
            const int birdBonus = 1;
            return new Relic("Bird Guide", Rarity.Common)
                .WithCoinsCalculationRelicEffect($"All birds give +{birdBonus} coin", ModifierType.Plus, birdBonus, i => i.HasTag("bird"));
        }
        private Relic TreasureMap()
        {
            const int chestChance = 50; // 50%
            const int goldenChestChance = 10; // 10%
            return new Relic("Treasure Map", Rarity.Rare)
                .WithAfterStageRewardRelicEffect("After every stage completion have a chance to receive a chest or golden chest tile item",
                    (stageNumber, game) => [
                Randomizer.Percent(chestChance) ? game.TileItemWithId("chest") : null,
                Randomizer.Percent(goldenChestChance) ? game.TileItemWithId("golden_chest") : null
                    ]);
        }
        private Relic GoldenKey()
        {
            const int openChance = 30; // 30%
            return new Relic("Golden Key", Rarity.Rare)
                .WithAfterSpinRelicEffect($"After each spin have a {openChance}% chance to open a chest in a slot machine field",
                    game =>
                    {
                        for (int row = 0; row < game.SlotMachine.RowsCount; row++)
                        {
                            for (int col = 0; col < game.SlotMachine.ColumnsCount; col++)
                            {
                                var tileItem = game.SlotMachine.TileItems[row, col];
                                if (tileItem.IsChest() && Randomizer.Percent(openChance))
                                {
                                    game.DestroyTileItem(tileItem, row, col);
                                }
                            }
                        }
                    });
        }
        private Relic AppleTree()
        {
            const int appleBonus = 1;
            const int appleChance = 60; // 40%
            return new Relic("Apple Tree", Rarity.Legendary, isSpecial: true)
                .WithCoinsCalculationRelicEffect($"Apple tile item gives +{appleBonus} coin", ModifierType.Plus, appleBonus, i => i.Id == "apple")
                .WithAfterStageRewardRelicEffect($"After stage have a {appleChance}% chance to receive an apple tile item",
                    (stageNumber, game) => [Randomizer.Percent(appleChance) ? game.TileItemWithId("apple") : null])
                .WithAfterStageRewardRelicEffect("After every fifth stage receive a golden apple tile item",
                    (stageNumber, game) => [stageNumber % 5 == 0 ? game.TileItemWithId("golden_apple") : null]);
        }
        private Relic AppleTreeSapling()
        {
            const int appleBonus = 1;
            const int counterValueToTransform = 15;
            RelicWithCounter appleTreeSapling = new RelicWithCounter("Apple Tree Sapling", Rarity.Rare, startingCounterValue: 0, isUnique: true)
                .WithCoinsCalculationRelicEffect($"Apple tile item gives +{appleBonus} coin", ModifierType.Plus, appleBonus, i => i.Id == "apple") as RelicWithCounter;

            appleTreeSapling = appleTreeSapling.WithAfterSpinRelicEffect($"After each spin increase counter by 1", (_) => appleTreeSapling.IncrementCounter(1)) as RelicWithCounter;
            appleTreeSapling = appleTreeSapling.WithTransformationRelicEffect(
                $"When counter value reach {counterValueToTransform} transforms into apple tree",
                (_) => appleTreeSapling.Counter >= counterValueToTransform, AppleTree()) as RelicWithCounter;

            return appleTreeSapling;
        }
        private Relic RandomToken()
        {
            return new Relic("Random Token", Rarity.Epic)
                .WithOnStageStartedRelicEffect("At the beginning of each stage, receive 1 random token",
                    (game) => game.Inventory.Tokens.AddToken(TokensCollection.RandomTokenType()));
        }
        private Relic DiamondToken()
        {
            const int minDiamonds = 5;
            const int maxDiamonds = 7;
            const int extraCoinsForDiamonds = 5;
            return new Relic("Diamond Token", Rarity.Rare)
                .WithAfterTokenUsedRelicEffect("When using any token, receive from 5 to 7 diamonds",
                    (game, token) => game.Inventory.AddDiamonds(Randomizer.Int(minDiamonds, maxDiamonds)))
                .WithGameStatRelicEffect($"Receive {extraCoinsForDiamonds}% more diamonds for extra coins after each stage", GameStat.AfterStageCoinsToDiamondsCoefficient, ModifierType.Plus, extraCoinsForDiamonds * 0.01);
        }
        private Relic UFO()
        {
            const double alienBonus = 0.5;
            return new Relic("UFO", Rarity.Epic)
                .WithNonConstantCalculationRelicEffect($"Aliens give additional {alienBonus} coins for each alien in the inventory",
                    (game) =>
                    {
                        int aliensCount = game.Inventory.TileItems.Count(ti => ti.IsAlien());
                        return new(ModifierType.Plus, aliensCount * alienBonus);
                    },
                    ti => ti.IsAlien());
        }
        private Relic Globe()
        {
            const int coinsPerAnimal = 10;
            return new Relic("Globe", Rarity.Epic)
                .WithOnStageStartedRelicEffect($"On the start of each stage receive {coinsPerAnimal} coins for each animal and bird in the inventory",
                    (game) =>
                    {
                        int coins = game.Inventory.TileItems.Count(ti => ti.HasOneOfTags("animal", "bird")) * coinsPerAnimal;
                        game.AddCoins(coins);
                    });
        }

        private Relic Alarm()
        {
            const double humansIncomeBonus = 2.5;
            return new Relic("Alarm", Rarity.Rare, isUnique: true)
                .WithNonConstantCalculationRelicEffect($"All humans in the first spins of each stage give ×{humansIncomeBonus} coins",
                    (game) =>
                    {
                        if (game.SpinsLeft == game.StatsTracker.StageSpinsCount - 1)
                            return new(ModifierType.Multiply, humansIncomeBonus);
                        return new(ModifierType.Plus, 0);
                    },
                    ti => ti.HasTag("human"));
        }

        private Relic FourLeafClover()
        {
            const int luckBonus = 5;
            return new Relic("Four Leaf Clover", Rarity.Rare)
                .WithGameStatRelicEffect($"Gives +{luckBonus} to luck", GameStat.Luck, ModifierType.Plus, luckBonus);
        }

        private Relic Milk()
        {
            const double humanCoinMultiplier = 1.15;
            return new Relic("Milk", Rarity.Rare)
                .WithCoinsCalculationRelicEffect($"All humans give ×{humanCoinMultiplier} coins", ModifierType.Multiply, humanCoinMultiplier, (ti) => ti.HasTag("human"));
        }

        private Relic OrangeJuice()
        {
            const int humanBonus = 2;
            return new Relic("Orange Juice", Rarity.Common)
                .WithCoinsCalculationRelicEffect($"All humans give +{humanBonus} coins", ModifierType.Plus, humanBonus, (ti) => ti.HasTag("human"));
        }

        private Relic Telescope()
        {
            const double planetMultiplier = 1.25;
            return new Relic("Telescope", Rarity.Rare)
                .WithCoinsCalculationRelicEffect($"All planets give {planetMultiplier} × coins", ModifierType.Multiply, planetMultiplier, (ti) => ti.HasTag("planet"));
        }
        private Relic BowlOfCandies()
        {
            const int minCandies = 2;
            const int maxCandies = 4;
            return new Relic("Bowl Of Candies", Rarity.Epic)
                .WithOnStageStartedRelicEffect(
                $"With start of each stage receive from {minCandies} to {maxCandies} candy tile items",
                (game) =>
                {
                    TileItem candy = game.TileItemWithId("candy");
                    for (int i = 0; i < Randomizer.Int(minCandies, maxCandies); i++)
                    {
                        game.AddTileItemToInventory(candy);
                    }
                });
        }
        private Relic DiamondPass()
        {
            const int diamondsCount = 15;
            return new Relic("Diamond Pass", Rarity.Legendary)
                .WithOnNewStageChoosingSkippedRelicEffect(
                    $"Skipping tile item or relic on the before stage items choosing phase gives {diamondsCount} diamonds",
                    (game) => game.Inventory.AddDiamonds(diamondsCount));
        }
        private Relic WaffleIron()
        {
            const int waffleChance = 30; //30%
            return new Relic("Waffle Iron", Rarity.Epic)
                .WithAfterSpinRelicEffect(
                $"After each spin have a {waffleChance}% chance to add 1 waffle to inventory",
                (game) =>
                {
                    if (Randomizer.Percent(waffleChance))
                        game.AddTileItemToInventory(game.TileItemWithId("waffle"));
                }
                );
        }
        private Relic GalacticAssemblyMedal()
        {
            const int aliensFromPlanetsBonus = 1;
            const int planetsFromAliensBonus = 1;
            return new Relic("Galactic Assembly Medal", Rarity.Legendary)
                .WithNonConstantCalculationRelicEffect(
                    $"Aliens give +{aliensFromPlanetsBonus} coin for every planet variant in the inventory",
                    (game) => new(ModifierType.Plus, game.Inventory.TileItems.Count(ti => ti.IsPlanet()) * aliensFromPlanetsBonus),
                    ti => ti.IsAlien())
                .WithNonConstantCalculationRelicEffect(
                    $"Planets give +{planetsFromAliensBonus} coin for every alien variant in the inventory",
                    (game) => new(ModifierType.Plus, game.Inventory.TileItems.Count(ti => ti.IsAlien()) * planetsFromAliensBonus),
                    ti => ti.IsPlanet());
        }
        private Relic Hourglass()
        {
            const int coinsBonus = 2;
            return new Relic("Hourglass", Rarity.Epic, isUnique: true)
                .WithNonConstantCalculationRelicEffect(
                    $"In the last 3 spins of every stage all tile items give +{coinsBonus} coins",
                    (game) =>
                    {
                        if (game.SpinsLeft < 3)
                            return new(ModifierType.Plus, coinsBonus);
                        else
                            return new(ModifierType.Plus, 0);
                    },
                    _ => true
                );
        }
        private Relic ZooTickets()
        {
            const int coinsBonus = 1;
            return new Relic("Zoo Tickets", Rarity.Legendary, isUnique: true)
                .WithNonConstantCalculationRelicEffect(
                    $"All people (except zookeeper) give +{coinsBonus} coins for every bird and animal variants in the inventory",
                    (game) =>
                    {
                        int count = game.Inventory.TileItems.Where(ti => ti.HasOneOfTags("animal", "bird")).DistinctBy(ti => ti.Id).Count();
                        return new(ModifierType.Plus, count * coinsBonus);
                    },
                    (ti) => ti.HasTag("human") && !ti.IdIs("zookeeper")
                );
        }
        private Relic PiggyBank()
        {
            const int coinsForRemainingSpins = 3;
            return new Relic("Piggy Bank", Rarity.Legendary, isUnique: true)
                .WithNonConstantCalculationRelicEffect(
                    $"All humans give +{coinsForRemainingSpins} coins for every remaining spin",
                    (game) => new(ModifierType.Plus, (game.SpinsLeft + 1) * coinsForRemainingSpins),
                    ti => ti.HasTag("human")
            );
        }
        private Relic BagOfSourWorms()
        {
            const int startingCounter = 7;
            RelicWithCounter sourWorms = new("Bag Of Sour Worms", Rarity.Legendary, startingCounterValue: startingCounter);
            sourWorms = sourWorms
                .WithAfterSpinRelicEffect("Every spin if counter is more than 0 decreases counter by 1 and adds 1 sour worm",
                (game) =>
                {
                    if (sourWorms.Counter < 1) return;
                    int rand = Randomizer.Int(1, 3);
                    switch (rand)
                    {
                        case 1: game.AddTileItemToInventory(game.TileItemWithId("green_and_yellow_sour_worm")); break;
                        case 2: game.AddTileItemToInventory(game.TileItemWithId("pink_and_blue_sour_worm")); break;
                        default: game.AddTileItemToInventory(game.TileItemWithId("orange_sour_worm")); break;
                    }
                    sourWorms.IncrementCounter(-1);
                }
                ) as RelicWithCounter;
            sourWorms = sourWorms.WithTransformationRelicEffect("After counter reaches 0 disappears", (_) => sourWorms.Counter == 0, null) as RelicWithCounter;
            return sourWorms;
        }
        private Relic OlympicFlag()
        {
            const int
                firstWith5Spins = 15,
                secondWith5Spins = 25,
                firstWith3Spins = 5,
                secondWith3Spins = 10,
                medalCoinsBonus = 1;
            return new Relic("Olympic Flag", Rarity.Mythic, isUnique: true)
                .WithCoinsCalculationRelicEffect($"All medals give +{medalCoinsBonus} coin", ModifierType.Plus, 1, ti => ti.HasTag("medal"))
                .WithAfterStageRewardRelicEffect(
                    $"If you complete stage with 3 or more spins, receive one of medals (first place with {firstWith3Spins}%, second place with {secondWith3Spins}%, third place with {100 - firstWith3Spins - secondWith3Spins}% )",
                    (stageNumber, game) =>
                    {
                        if (game.SpinsLeft >= 3)
                        {
                            int rand = Randomizer.Int(1, 100);
                            if (rand <= firstWith3Spins)
                                return [game.TileItemWithId("first_place_medal")];
                            else if (rand <= firstWith3Spins + secondWith3Spins)
                                return [game.TileItemWithId("second_place_medal")];
                            else
                                return [game.TileItemWithId("third_place_medal")];
                        }
                        return [];
                    })
                .WithAfterStageRewardRelicEffect(
                    $"If you complete stage with 5 or more spins receive, one of medals (first place with {firstWith5Spins}%, second place with {secondWith5Spins}%, third place with {100 - firstWith5Spins - secondWith5Spins}% )",
                    (stageNumber, game) =>
                    {
                        if (game.SpinsLeft >= 5)
                        {
                            int rand = Randomizer.Int(1, 100);
                            if (rand <= firstWith5Spins)
                                return [game.TileItemWithId("first_place_medal")];
                            else if (rand <= firstWith5Spins + secondWith5Spins)
                                return [game.TileItemWithId("second_place_medal")];
                            else
                                return [game.TileItemWithId("third_place_medal")];
                        }
                        return [];
                    });
        }
        private Relic IntergalacticBubbleGum()
        {
            return new Relic("Intergalactic Bubblegum", Rarity.Epic)
                .WithNonConstantCalculationRelicEffect(
                    $"All aliens give +1 coin for every type of sweet tile item in your inventory",
                    (game) =>
                    {
                        int count = game.Inventory.TileItems.Where(ti => ti.HasTag("sweet")).DistinctBy(ti => ti.Id).Count();
                        return new(ModifierType.Plus, count);
                    },
                    (ti) => ti.HasTag("alien"));
        }
        private Relic Storybook()
        {
            return new Relic("Storybook", Rarity.Epic)
                .WithGameStatRelicEffect($"Choose from 1 more relic in before stage choosing phase", GameStat.NewStageRelicsForChoiceCount, ModifierType.Plus, 1)
                .WithGameStatRelicEffect($"There will be 1 more relic to choose from in the shop", GameStat.RelicsInShopCount, ModifierType.Plus, 1);
        }
        private Relic ShippingContainers()
        {
            return new Relic("Shipping Containers", Rarity.Epic)
                .WithGameStatRelicEffect($"There will be 3 more tile items to choose from in the shop", GameStat.TileItemsInShopCount, ModifierType.Plus, 3);
        }
        private Relic Safe()
        {
            const int maxValue = 20;
            RelicWithCounter safe = new("Safe", Rarity.Mythic, isUnique: true);
            safe = safe.WithAfterStageRewardRelicEffect(
                $"After each stage increase counter by count of remaining spins (up to {maxValue})",
                (stageNumber, game) =>
                {
                    if (safe.Counter < maxValue)
                    {
                        safe.IncrementCounter(Math.Min(game.SpinsLeft, maxValue - safe.Counter));
                        game.StatsTracker.SetChanged();
                    }
                    return [];
                }) as RelicWithCounter;

            safe = safe.WithNonConstantGameStatRelicEffect(
                "For every two counter values, reduces the number of coins needed to complete the stage by 1%",
                GameStat.CoinsNeededToCompleteStage,
                (game) => new Tuple<ModifierType, double>(ModifierType.Multiply, 1 - ((double)safe.Counter / 200))) as RelicWithCounter;
            return safe;
        }
        private Relic Compass()
        {
            const int maxValue = 15;
            RelicWithCounter compass = new("Compass", Rarity.Legendary, isUnique: true);
            compass = compass.WithAfterStageRewardRelicEffect(
                $"If you complete stage with 0 spins remain, increase counter by 1 (up to {maxValue})",
                (stageNumber, game) =>
                {
                    if (game.SpinsLeft == 0 && compass.Counter < maxValue)
                    {
                        compass.IncrementCounter(1);
                        game.StatsTracker.SetChanged();
                    }
                    return [];
                }) as RelicWithCounter;
            compass = compass.WithNonConstantGameStatRelicEffect(
                "Reduces the prices in the store based on the counter value",
                GameStat.ShopPriceCoefficient,
                (game) => new(ModifierType.Multiply, 1 - ((double)compass.Counter / 100))) as RelicWithCounter;
            return compass;
        }
    }
}
