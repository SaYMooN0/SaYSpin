namespace SaYSpin.src.gameplay_parts.inventory_related
{
    public static class InventoryStatsCalculationExtension
    {
        public static int CoinsNeededToCompleteTheStageCoefficient(this Inventory inventory) => 1;
        public static int StartingCoins(this Inventory inventory) => 0;
        public static double CoinsToDiamondsCoefficient(this Inventory inventory) => 1;
        

    }
}
