namespace SaYSpin.src
{
    public static class Randomizer
    {
        private static Random _rnd = new();
        public static int Int(int maxValue) =>
            _rnd.Next(0, maxValue);
        public static bool Percent(int percent) =>
            _rnd.Next(0, 100) < percent;

    }
}
