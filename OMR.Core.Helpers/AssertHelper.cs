namespace OMR.Core.Helpers
{
    using System;

    public static class AssertHelper
    {
        public static void AreNotNull(params object[] objects)
        {
            if (objects == null)
                throw new ArgumentNullException("objects");

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] == null)
                {
                    throw new ArgumentNullException("object " + i);
                }
            }
        }

        public static void AreNotNullOrEmpty(params string[] strings)
        {
            if (strings == null)
                throw new ArgumentNullException("strings");

            for (int i = 0; i < strings.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(strings[i]))
                {
                    throw new ArgumentException("string " + i);
                }
            }
        }

        public static void AreEqual(object object1, object object2)
        {
            if (!object1.Equals(object2))
            {
                throw new InvalidProgramException("obj1 and obj2 are not equal");
            }
        }

        public static void IsTrue(bool condition)
        {
            if (!condition)
            {
                throw new InvalidProgramException("obj1 and obj2 are not equal");
            }
        }
    }
}
