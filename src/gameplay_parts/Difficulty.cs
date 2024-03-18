namespace SaYSpin.src.gameplay_parts
{
    public class Difficulty
    {
        public Difficulty(string name,
            string[] negatives,
            string[] positives,
            string imagePath,
            double neededCoinsMultiplier,
            double shopPricesMultiplier,
            int startingDiamondsCount,
            int startingTokensCount,
            double rubiesAfterRunMultiplier,
            double expAfterRunMultiplier)
        {
            Name = name;
            Negatives = negatives;
            Positives = positives;
            ImagePath = imagePath;
            NeededCoinsMultiplier = neededCoinsMultiplier;
            ShopPricesMultiplier = shopPricesMultiplier;
            StartingDiamondsCount = startingDiamondsCount;
            StartingTokensCount = startingTokensCount;
            RubiesAfterRunMultiplier = rubiesAfterRunMultiplier;
            ExpAfterRunMultiplier = expAfterRunMultiplier;
        }

        public string Name { get; init; }
        public string[] Negatives { get; init; }
        public string[] Positives { get; init; }
        public string ImagePath { get; init; }

        public double NeededCoinsMultiplier {  get; init; } 
        public double ShopPricesMultiplier { get; init; }
        public int StartingDiamondsCount { get; init; }
        public int StartingTokensCount { get; init; }

        public double RubiesAfterRunMultiplier { get; init; }
        public double ExpAfterRunMultiplier { get; init; }
    }
}
