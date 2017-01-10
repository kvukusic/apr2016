using System;

namespace APR.DZ4
{
    public static class RandomNumberGeneratorProvider
    {
        private static readonly Random _randomNumberGenerator = new Random();

        public static Random Instance { get { return _randomNumberGenerator; } }
    }
}