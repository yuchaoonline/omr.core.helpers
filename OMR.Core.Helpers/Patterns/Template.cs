namespace OMR.Core.Helpers.Patterns
{
    using System;
    using System.Collections.Generic;

    public class Template<T, K>
        where T : IComparable
        where K : IComparable
    {
        private Func<T, K> _mappingFunction;

        public Template(Func<T, K> mappingFunction)
        {
            _mappingFunction = mappingFunction;
        }

        public K Map(T rawObject)
        {
            if (rawObject == null)
                throw new ArgumentException("rawObject");

            return _mappingFunction(rawObject);
        }

        public IEnumerable<K> Map(IEnumerable<T> rawObjects)
        {
            if (rawObjects == null)
                throw new ArgumentException("rawObjects");

            var mappedObjects = new List<K>();

            foreach (var item in rawObjects)
            {
                var currentMappedObject = Map(item);

                mappedObjects.Add(currentMappedObject);
            }

            return mappedObjects;
        }

        public bool AreEqual(T rawObject, K mappedObject)
        {
            if (rawObject == null)
                throw new ArgumentException("rawObject");
            if (mappedObject == null)
                throw new ArgumentException("mappedObject");

            K expectedObject = Map(rawObject);

            return expectedObject.CompareTo(mappedObject) == 0;
        }

    }
}
