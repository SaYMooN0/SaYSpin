using SaYSpin.src.enums;
using SaYSpin.src.extension_classes;
using SaYSpin.src.extension_classes.factories;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.static_classes;
using SaYSpin.src.secondary_classes;


namespace SaYSpin.src.inventory_items_storages
{
    internal class TileItemsStorage : IInventoryItemStorage<TileItem>
    {
        Dictionary<string, Func<TileItem>> _storedItems { get; set; }
        HashSet<string> _avaliableTileItems;
        public TileItemsStorage()
        {
            _storedItems = new Dictionary<string, Func<TileItem>>
            {
                ["Apple"] = () => TileItem.Ordinary("Apple", Rarity.Common, 3, ["fruit"]),
                ["Banana"] = () => TileItem.Ordinary("Banana", Rarity.Common, 3, ["fruit"]),
                ["Dragon Fruit"] = () => TileItem.Ordinary("Dragon Fruit", Rarity.Rare, 7, ["fruit"]),
                ["Golden Apple"] = () => TileItem.Ordinary("Golden Apple", Rarity.Epic, 17, ["fruit", "gold"]),
                ["Orange"] = () => TileItem.Ordinary("Orange", Rarity.Common, 3, ["fruit"]),
                ["Pineapple"] = () => TileItem.Ordinary("Pineapple", Rarity.Rare, 5, ["fruit"]),
                ["Candy"] = () => TileItem.Ordinary("Candy", Rarity.Common, 3, ["sweet"]),
                ["Chocolate Bar"] = () => TileItem.Ordinary("Chocolate Bar", Rarity.Rare, 5, ["sweet"]),
                ["Lollipop"] = () => TileItem.Ordinary("Lollipop", Rarity.Common, 3, ["sweet"]),
                ["Gold Bar"] = () => TileItem.Ordinary("Gold Bar", Rarity.Rare, 20, ["gold"]),
                ["Parrot"] = Parrot,
                ["Chest"] = Chest,
                ["Golden Chest"] = GoldenChest,
                ["Pigeon"] = Pigeon,
                ["Owl"] = Owl,
                ["Magic Ball"] = MagicBall,
                ["Wizard"] = () => TileItem.Ordinary("Wizard", Rarity.Rare, 15, ["human"]),
                ["Carrot"] = () => TileItem.Ordinary("Carrot", Rarity.Common, 5, []),
                ["Golden Carrot"] = () => TileItem.Ordinary("Golden Carrot", Rarity.Epic, 15, []),
                ["Sweet Tooth"] = SweetTooth,
                ["Pirate"] = Pirate,
                ["Capybara"] = Capybara,
                ["Rabbit"] = Rabbit,
                ["Green Alien"] = GreenAlien,
                ["Purple Alien"] = PurpleAlien,
                ["Orange Alien"] = OrangeAlien,
                ["Cyan Alien"] = CyanAlien,
                ["Mars"] = Mars,
                ["Saturn"] = Saturn,
                ["Venus"] = Venus,
                ["Neptune"] = Neptune,
                ["Artificial Satellite"] = ArtificialSatellite,
                ["Waffle"] = Waffle,
                ["Waffle With Apples"] = WaffleWithApples,
                ["Zookeeper"] = Zookeeper,
                ["Tiger"] = Tiger,
            };
            //_avaliableTileItems = InitAvailable();
            _avaliableTileItems = _storedItems.Keys.ToHashSet();
        }
        public Dictionary<string, Func<TileItem>> GetAvailableItems() =>
        _storedItems
            .Where(nameItemPair => _avaliableTileItems.Contains(nameItemPair.Key))
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
        private TileItem Parrot()
        {
            const int pirateIncomeMultyplier = 2;
            return TileItem.Ordinary("Parrot", Rarity.Rare, 3, ["bird"]).WithTileItemsEnhancingTileItemEffect(
                    $"Adjacent pirates give {pirateIncomeMultyplier}× coins",
                    SlotMachineArea.Adjacent,
                    ModifierType.Multiply,
                    pirateIncomeMultyplier,
                    (ti) => ti.IdIs("pirate"));
        }
        private TileItem SweetTooth()
        {
            TileItemWithCounter t = TileItemWithCounter
                .New("Sweet Tooth", "Gives coins equal to the value of the counter", Rarity.Epic, 0, ["human"])
                .SetBaseIncomeCalculationFunc(ti => ti.Counter);
            t = t.WithAbsorbingTileItemEffect(
                "Eats adjacent sweets and increases the counter depending on the rarity of the eaten sweet",
                t => t.HasTag("sweet"),
                (game, absorbedTI) => t.IncrementCounter((int)absorbedTI.Rarity + 1));
            return t;
        }

        private TileItem Pirate()
        {
            TileItemWithCounter t = TileItemWithCounter
                .New("Pirate", "Gives (7 + value of counter) coins ", Rarity.Legendary, 7, ["human"])
                .SetBaseIncomeCalculationFunc(t => t.Counter + 7);
            t = t.WithAbsorbingTileItemEffect(
                "Absorbs adjacent chests and golden tile items giving 3 times of their basic income",
                tiToAbsorb => tiToAbsorb.IsConsumableByPirate(),
                (game, absorbedTI) =>
                    {
                        t.IncrementCounter(1);
                        game.AddCoins(absorbedTI.InitialCoinValue * 3);
                    });
            return t;
        }

        private TileItem Chest()
        {
            const int minCoins = 20;
            const int maxCoins = 50;
            const int minDiamonds = 3;
            const int maxDiamonds = 7;
            return TileItem.Ordinary("Chest", Rarity.Rare, 1, ["chest"])
                .WithOnDestroyTileItemEffect(
                $"After opening gives from {minCoins} to {maxCoins} coins and from {minDiamonds} to {maxDiamonds} diamonds",
                (game) =>
                {
                    game.AddCoins(Randomizer.Int(minCoins, maxCoins));
                    game.Inventory.AddDiamonds(Randomizer.Int(minDiamonds, maxDiamonds));
                });
        }
        private TileItem GoldenChest()
        {
            const int minCoins = 50;
            const int maxCoins = 150;
            const int minDiamonds = 10;
            const int maxDiamonds = 20;
            return TileItem.Ordinary("Golden Chest", Rarity.Epic, 3, ["chest", "gold"])
                .WithOnDestroyTileItemEffect(
                $"After opening gives from {minCoins} to {minCoins} coins and from {minDiamonds} to {minDiamonds} diamonds",
                (game) =>
                {
                    game.AddCoins(Randomizer.Int(minCoins, maxCoins));
                    game.Inventory.AddDiamonds(Randomizer.Int(minDiamonds, maxDiamonds));
                });
        }
        private TileItem Pigeon()
        {
            const int adjacentBirdBonus = 4;
            return TileItem.Ordinary("Pigeon", Rarity.Common, 2, ["bird"])
                .WithTileItemsEnhancingTileItemEffect($"Adjacent birds give +{adjacentBirdBonus} coins", SlotMachineArea.Adjacent, ModifierType.Plus, adjacentBirdBonus, (ti) => ti.HasTag("bird"));
        }
        private TileItem Owl()
        {
            const double adjacentWizardMultiplier = 1.6;
            return TileItem.Ordinary("Owl", Rarity.Rare, 5, ["bird"])
                .WithTileItemsEnhancingTileItemEffect($"Adjacent wizards give extra {adjacentWizardMultiplier}× coins", SlotMachineArea.Adjacent, ModifierType.Multiply, adjacentWizardMultiplier, (ti) => ti.IdIs("wizard"));
        }
        private TileItem MagicBall()
        {
            const double adjacentAllMultiplier = 1.5;
            const double adjacentWizardMultiplier = 2;
            return TileItem.Ordinary("Magic Ball", Rarity.Epic, 5, ["magical"])
                .WithTileItemsEnhancingTileItemEffect($"All adjacent items give {adjacentAllMultiplier}× coins", SlotMachineArea.Adjacent, ModifierType.Multiply, adjacentAllMultiplier, (ti) => true)
                .WithTileItemsEnhancingTileItemEffect($"Adjacent wizards give extra {adjacentWizardMultiplier} coins", SlotMachineArea.Adjacent, ModifierType.Multiply, adjacentWizardMultiplier, (ti) => ti.IdIs("wizard"));
        }
        private TileItem Capybara()
        {
            const int bonusMultiplier = 2;
            return TileItem.Ordinary("Capybara", Rarity.Mythic, 10, ["animal"])
                .WithTileItemsEnhancingTileItemEffect($"Adjacent humans, animals and birds give {bonusMultiplier}× coins",
                    SlotMachineArea.Adjacent, ModifierType.Multiply, bonusMultiplier, (ti) => ti.HasOneOfTags("bird", "animal", "human"));
        }
        private TileItem Rabbit()
        {
            const int carrotBonus = 15;
            const int goldenCarrotBonus = 150;
            return TileItem.Ordinary("Rabbit", Rarity.Rare, 3, ["animal"])
                .WithAbsorbingTileItemEffect($"Eats adjacent carrots and adds {carrotBonus} coins for each", (ti) => ti.IdIs("carrot"),
                    (game, absorbedTI) => game.AddCoins(carrotBonus))
                .WithAbsorbingTileItemEffect($"Eats adjacent golden carrots and adds {goldenCarrotBonus} coins for each", (ti) => ti.IdIs("golden_carrot"),
                    (game, absorbedTI) => game.AddCoins(goldenCarrotBonus));
        }
        private TileItem GreenAlien()
        {
            const int alienBonus = 1;
            return TileItem.Ordinary("Green Alien", Rarity.Rare, 3, ["alien"])
                .WithTileItemsEnhancingTileItemEffect($"All aliens give +{alienBonus} coin", SlotMachineArea.AllTiles, ModifierType.Plus, alienBonus, (ti) => ti.IsAlien());
        }
        private TileItem PurpleAlien()
        {
            const double areaMultiplier = 1.4;
            return TileItem.Ordinary("Purple Alien", Rarity.Epic, 5, ["alien"])
                .WithTileItemsEnhancingTileItemEffect($"All aliens in a 5 by 5 square from give {areaMultiplier}× coins", SlotMachineArea.Square5, ModifierType.Multiply, areaMultiplier, (ti) => ti.IsAlien());
        }
        private TileItem OrangeAlien()
        {
            const int lineBonus = 3;
            return TileItem.Ordinary("Orange Alien", Rarity.Rare, 1, ["alien"])
                .WithTileItemsEnhancingTileItemEffect($"All aliens in the same horizontal line give +{lineBonus} coins", SlotMachineArea.HorizontalLine, ModifierType.Plus, lineBonus, (ti) => ti.IsAlien());
        }
        private TileItem CyanAlien()
        {
            const int verticalLineBonus = 3;
            const double cornerMultiplier = 2;
            return TileItem.Ordinary("Cyan Alien", Rarity.Legendary, 2, ["alien"])
                .WithTileItemsEnhancingTileItemEffect($"All aliens in the same vertical line give +{verticalLineBonus} coins", SlotMachineArea.VerticalLine, ModifierType.Plus, verticalLineBonus, (ti) => ti.IsAlien())
                .WithTileItemsEnhancingTileItemEffect($"All aliens in the corner tiles give {cornerMultiplier}× coins", SlotMachineArea.CornerTiles, ModifierType.Multiply, cornerMultiplier, (ti) => ti.IsAlien());
        }
        private TileItem Mars()
        {
            const double adjacentGreenAlienBonus = 1.3;
            return TileItem.Ordinary("Mars", Rarity.Epic, 5, ["planet"])
                .WithTileItemsEnhancingTileItemEffect($"Adjacent green aliens give {adjacentGreenAlienBonus} more coins", SlotMachineArea.Adjacent, ModifierType.Multiply, adjacentGreenAlienBonus, (ti) => ti.IdIs("green_alien"));
        }
        private TileItem Saturn()
        {
            const double lineMultiplier = 1.7;
            return TileItem.Ordinary("Saturn", Rarity.Epic, 5, ["planet"])
                .WithTileItemsEnhancingTileItemEffect($"Purple aliens in the same vertical line give {lineMultiplier}× coins", SlotMachineArea.VerticalLine, ModifierType.Multiply, lineMultiplier, (ti) => ti.IdIs("purple_alien"))
                .WithTileItemsEnhancingTileItemEffect($"Purple aliens in the same horizontal line give {lineMultiplier}× coins", SlotMachineArea.HorizontalLine, ModifierType.Multiply, lineMultiplier, (ti) => ti.IdIs("purple_alien"));
        }
        private TileItem Venus()
        {
            const int areaBonus = 5;
            return TileItem.Ordinary("Venus", Rarity.Epic, 10, ["planet"])
                .WithTileItemsEnhancingTileItemEffect($"Orange aliens in a 5 by 5 area give {areaBonus} more coins", SlotMachineArea.Square5, ModifierType.Plus, areaBonus, (ti) => ti.IdIs("orange_alien"));
        }
        private TileItem Neptune()
        {
            const double horizontalMultiplier = 3;
            const int cornerBonus = 5;
            return TileItem.Ordinary("Neptune", Rarity.Epic, 2, ["planet"])
                .WithTileItemsEnhancingTileItemEffect($"Cyan aliens in the same horizontal line give {horizontalMultiplier}× coins", SlotMachineArea.HorizontalLine, ModifierType.Multiply, horizontalMultiplier, (ti) => ti.IsAlien())
                .WithTileItemsEnhancingTileItemEffect($"Cyan aliens in the corner tiles give {cornerBonus} more coins", SlotMachineArea.CornerTiles, ModifierType.Plus, cornerBonus, (ti) => ti.IsAlien());
        }
        private TileItem ArtificialSatellite()
        {
            const double adjacentPlanetMultiplier = 2.5;
            return TileItem.Ordinary("Artificial Satellite", Rarity.Legendary, 10, [])
                .WithTileItemsEnhancingTileItemEffect($"Adjacent planets give {adjacentPlanetMultiplier}× coins", SlotMachineArea.Adjacent, ModifierType.Multiply, adjacentPlanetMultiplier, (ti) => ti.IsPlanet());
        }
        private TileItem Waffle()
        {
            var tileItem = TileItem.UnavailableInBeforeStageChoosingPhase("Waffle", Rarity.Rare, 5, ["sweet"]);
            tileItem = tileItem
                .WithAreaScanningTileItemEffect("If there are two adjacent apples, absorbs them", SlotMachineArea.Adjacent, (ti) => ti.IdIs("apple"), (game, tiWithCoords) =>
                    {
                        var tileItems = tiWithCoords.Where(ti => !ti.TileItem.Markers.Contains(Markers.WillBeAbsorbed)).Take(2).ToArray();
                        if (tileItems.Count() >= 2)
                        {
                            var ti1 = tileItems[0];
                            var ti2 = tileItems[1];
                            ti1.TileItem.AddMarker(Markers.WillBeAbsorbed);
                            ti2.TileItem.AddMarker(Markers.WillBeAbsorbed);
                            game.DestroyTileItem(ti1.TileItem, ti1.row, ti1.column);
                            game.DestroyTileItem(ti2.TileItem, ti2.row, ti2.column);
                            tileItem.AddMarker(Markers.ReadyToPerformAction);
                        }
                    })
                .WithTransformationEffect(
                "When absorbing apples transforms into waffle with apples tile item",
                (game) => tileItem.Markers.Contains(Markers.ReadyToPerformAction),
                WaffleWithApples());
            return tileItem;

        }
        private TileItem WaffleWithApples() =>
            TileItem.UnavailableInBeforeStageChoosingPhase("Waffle With Apples", Rarity.Legendary, 15, ["sweet"]);

        private TileItem Zookeeper()
        {
            TileItemWithCounter t = TileItemWithCounter
                .New("Zookeeper", "Gives coins equal to the value of the counter", Rarity.Legendary, 0, ["human"])
                .SetBaseIncomeCalculationFunc(ti => ti.Counter);
            t = t.WithAreaScanningTileItemEffect(
                "Each spin increases the counter by the number of adjacent animals and birds",
                SlotMachineArea.Adjacent,
                (ti) => ti.HasTag("animal") || ti.HasTag("bird"),
                (game, tiWithCoords) => t.IncrementCounter(tiWithCoords.Count())
            );
            return t;
        }

        private TileItem Tiger()
        {
            const int adjacentAnimalBonus = 2;

            var newTI = TileItem.Ordinary("Tiger", Rarity.Epic, 8, ["animal"]);
            newTI = newTI
                .WithAreaScanningTileItemEffect(
                    $"If there any adjacent zookeeper increases income of adjacent animal tiles by {adjacentAnimalBonus} coins",
                    SlotMachineArea.Adjacent,
                    (ti) => ti.IdIs("zookeeper"),
                    (game, tiWithCoords) =>
                    {
                        if (tiWithCoords.Count() > 0)
                            newTI.AddMarker(Markers.ReadyToPerformAction);
                    }
                )
                .WithTileItemsEnhancingTileItemEffect(
                    string.Empty, SlotMachineArea.Adjacent, ModifierType.Plus, adjacentAnimalBonus,
                    (ti) => ti.HasTag("animal") && newTI.Markers.Contains(Markers.ReadyToPerformAction)
                );

            return newTI;
        }

    }
}
