﻿namespace SaYSpin.src.static_classes
{
    public static class Randomizer
    {
        private static readonly Random _rnd = new();
        public static int Int(int maxValue) =>
            _rnd.Next(0, maxValue);
        public static int Int(int minValue, int maxValue) =>
            _rnd.Next(minValue, maxValue + 1);
        public static bool Percent(int percent) =>
            _rnd.Next(0, 100) < percent;


    }
}
