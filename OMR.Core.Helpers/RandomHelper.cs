namespace OMR.Core.Helpers
{
    using System;
    using System.Text;

    public static class RandomHelper
    {
        private static Random randomSeed = new Random();

        public static string GetString(int size, bool lowerCase)
        {
            AssertHelper.IsTrue(size > 0);

            StringBuilder RandStr = new StringBuilder(size);

            var startValue = (lowerCase) ? 97 : 65;

            for (int i = 0; i < size; i++)
            {
                RandStr.Append((char)(26 * randomSeed.NextDouble() + startValue));
            }

            return RandStr.ToString();
        }

        public static int RandomNumber(int min, int max)
        {
            AssertHelper.IsTrue(min > 0);
            AssertHelper.AreEqual(max > min, true);

            return randomSeed.Next(min, max);
        }

        public static bool RandomBool()
        {
            return (randomSeed.NextDouble() > 0.5);
        }
    }
}