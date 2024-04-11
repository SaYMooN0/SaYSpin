namespace SaYSpin.src.gameplay_parts
{
    public record Difficulty(
        string Name,
        string[] Negatives,
        string[] Positives,
        double NeededCoinsMultiplier,
        double ShopPricesMultiplier,
        int StartingDiamondsCount,
        int StartingTokensCount,
        int StartingTileItemsCount,
        int StartingRelicsCount,
        double RubiesAfterRunMultiplier,
        double ExpAfterRunMultiplier)
    {

        public string GetFullImagePath() => $"/resources/images/difficulties/{Name.ToLower()}.png";
        public static Difficulty NormalDifficulty { get; } = new Difficulty(
        "Normal",
        [],
        [],
        NeededCoinsMultiplier: 1,
        ShopPricesMultiplier: 1,
        StartingDiamondsCount: 10,
        StartingTokensCount: 3,
        StartingTileItemsCount: 3,
        StartingRelicsCount: 1,
        RubiesAfterRunMultiplier: 1,
        ExpAfterRunMultiplier: 1);
        public static Difficulty New(string name, double neededCoinsMultiplier, double shopPricesMultiplier, int startingDiamondsCount, int startingTokensCount, int startingTileItemsCount, int startingRelicsCount, double rubiesAfterRunMultiplier, double expAfterRunMultiplier)
        {
            var negatives = new List<string>();
            var positives = new List<string>();

            var diffCoinsMultiplier = Math.Round(neededCoinsMultiplier - NormalDifficulty.NeededCoinsMultiplier, 1);
            if (diffCoinsMultiplier > 0)
                negatives.Add($"Coins requirement increased by {diffCoinsMultiplier * 100}%.");
            else if (diffCoinsMultiplier < 0)
                positives.Add($"Coins requirement decreased by {-diffCoinsMultiplier * 100}%.");

            var diffShopPricesMultiplier = Math.Round(shopPricesMultiplier - NormalDifficulty.ShopPricesMultiplier, 1);
            if (diffShopPricesMultiplier > 0)
                negatives.Add($"Shop prices increased by {diffShopPricesMultiplier * 100}%.");
            else if (diffShopPricesMultiplier < 0)
                positives.Add($"Shop prices decreased by {-diffShopPricesMultiplier * 100}%.");

            var diffStartingDiamondsCount = startingDiamondsCount - NormalDifficulty.StartingDiamondsCount;
            if (diffStartingDiamondsCount > 0)
                positives.Add($"Start with {diffStartingDiamondsCount} more diamonds.");
            else if (diffStartingDiamondsCount < 0)
                negatives.Add($"Start with {-diffStartingDiamondsCount} fewer diamonds.");

            var diffStartingTokensCount = startingTokensCount - NormalDifficulty.StartingTokensCount;
            if (diffStartingTokensCount > 0)
                positives.Add($"Start with {diffStartingTokensCount} more tokens.");
            else if (diffStartingTokensCount < 0)
                negatives.Add($"Start with {-diffStartingTokensCount} fewer tokens.");

            var diffRubiesAfterRunMultiplier = Math.Round(rubiesAfterRunMultiplier - NormalDifficulty.RubiesAfterRunMultiplier, 1);
            if (diffRubiesAfterRunMultiplier > 0)
                positives.Add($"Receive {diffRubiesAfterRunMultiplier * 100}% more rubies after runs.");
            else if (diffRubiesAfterRunMultiplier < 0)
                negatives.Add($"Receive {-diffRubiesAfterRunMultiplier * 100}% fewer rubies after runs.");

            var diffExpAfterRunMultiplier = Math.Round(expAfterRunMultiplier - NormalDifficulty.ExpAfterRunMultiplier, 1);
            if (diffExpAfterRunMultiplier > 0)
                positives.Add($"Receive {diffExpAfterRunMultiplier * 100}% more experience after runs.");
            else if (diffExpAfterRunMultiplier < 0)
                negatives.Add($"Receive {-diffExpAfterRunMultiplier * 100}% less experience after runs.");

            var diffStartingTileItemsCount = startingTileItemsCount - NormalDifficulty.StartingTileItemsCount;
            if (diffStartingTileItemsCount > 0)
                positives.Add($"Start with {diffStartingTileItemsCount} more tile items.");
            else if (diffStartingTileItemsCount < 0)
                negatives.Add($"Start with {-diffStartingTileItemsCount} fewer tile items.");

            var diffStartingRelicsCount = startingRelicsCount - NormalDifficulty.StartingRelicsCount;
            if (diffStartingRelicsCount > 0)
                positives.Add($"Start with {diffStartingRelicsCount} more relics.");
            else if (diffStartingRelicsCount < 0)
                negatives.Add($"Start with {-diffStartingRelicsCount} fewer relics.");


            return new Difficulty(
                name,
                negatives.ToArray(),
                positives.ToArray(),
                neededCoinsMultiplier,
                shopPricesMultiplier,
                startingDiamondsCount,
                startingTokensCount,
                startingTileItemsCount,
                startingRelicsCount,
                rubiesAfterRunMultiplier,
                expAfterRunMultiplier);
        }

    }
}

