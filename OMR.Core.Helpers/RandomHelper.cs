namespace OMR.Core.Helpers
{
    using System;
    using System.Text;

    public static class RandomHelper
    {
        private static Random randomSeed = new Random();

        public static string RandomString(int size, bool lowerCase)
        {
            AssertHelper.IsTrue(size > 0);

            StringBuilder RandStr = new StringBuilder(size);

            int Start = (lowerCase) ? 97 : 65;

            for (int i = 0; i < size; i++)
            {
                RandStr.Append((char)(26 * randomSeed.NextDouble() + Start));
            }

            return RandStr.ToString();
        }

        public static int RandomNumber(int Minimal, int Maximal)
        {
            AssertHelper.IsTrue(Minimal > 0);
            AssertHelper.AreEqual(Maximal > Minimal, true);

            return randomSeed.Next(Minimal, Maximal);
        }

        public static bool RandomBool()
        {
            return (randomSeed.NextDouble() > 0.5);
        }
    }
}