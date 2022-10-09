using System;

namespace Pavantares.CCG.Utilities
{
    public static class Randomizer
    {
        public static string GetId()
        {
            return Guid.NewGuid().ToString();
        }

        public static int GetCardsCount()
        {
            return UnityEngine.Random.Range(4, 7);
        }

        public static int GetValue()
        {
            return UnityEngine.Random.Range(0, 8);
        }

        public static int GetTestValue()
        {
            return UnityEngine.Random.Range(-2, 8);
        }
    }
}
