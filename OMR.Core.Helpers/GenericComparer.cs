namespace OMR.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GenericComparer<T>
    {
        public Func<T, object> SortFunc { get; set; }

        public Func<T, T, int> CompareFunc { get; set; }

        public Func<T, T, bool> EqualsFunc { get; set; }

        public List<ComparisonResult<T>> Compare(List<T> source, List<T> destination, bool inculudeIndenticals = false)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (destination == null)
                throw new ArgumentNullException("destination");

            //if (SortFunc == null)
            //    throw new InvalidProgramException("SortFunc has not implemented yet");

            if (CompareFunc == null)
                throw new InvalidProgramException("CompareFunc has not implemented yet");

            if (EqualsFunc == null)
                throw new InvalidProgramException("DetectChangesFunc has not implemented yet");

            //TODO argument exception

            if (SortFunc != null)
            {
                source = source.OrderBy(SortFunc).ToList();
                destination = destination.OrderBy(SortFunc).ToList();
            }

            var result = new List<ComparisonResult<T>>();

            int sourceIndex = 0;
            int destinationIndex = 0;

            while (true)
            {
                if (sourceIndex == source.Count && destinationIndex == destination.Count)
                {
                    break;
                }

                if (sourceIndex == source.Count)
                {
                    result.Add(new ComparisonResult<T>(destination[destinationIndex], ComparisonResultType.WDELETE));

                    destinationIndex += 1;
                    continue;
                }

                if (destinationIndex == destination.Count)
                {
                    result.Add(new ComparisonResult<T>(source[sourceIndex], ComparisonResultType.WCREATE));

                    sourceIndex += 1;
                    continue;
                }

                int compareResult = CompareFunc(source[sourceIndex], destination[destinationIndex]);

                if (0 == compareResult)
                {
                    if (EqualsFunc(destination[destinationIndex], source[sourceIndex]))
                    {
                        //IDENTICAL
                        if(inculudeIndenticals)
                            result.Add(new ComparisonResult<T>(destination[destinationIndex], ComparisonResultType.IDENTICAL));
                    }
                    else
                    {
                        result.Add(new ComparisonResult<T>(source[sourceIndex], ComparisonResultType.CONFLICT));
                    }

                    sourceIndex += 1;
                    destinationIndex += 1;
                }
                else if (0 < compareResult)
                {
                    result.Add(new ComparisonResult<T>(destination[destinationIndex], ComparisonResultType.WDELETE));

                    destinationIndex += 1;
                }
                else if (0 > compareResult)
                {
                    result.Add(new ComparisonResult<T>(source[sourceIndex], ComparisonResultType.WDELETE));

                    sourceIndex += 1;
                }

            }

            return result;
        }
    }

    public class ComparisonResult<T>
    {
        public T Value { get; set; }

        public ComparisonResultType Target { get; set; }

        public ComparisonResult(T value, ComparisonResultType fileTarget)
        {
            Value = value;
            Target = fileTarget;
        }
    }

    public enum ComparisonResultType
    {
        UNKOWN,
        CONFLICT,
        WCREATE,
        WDELETE,
        IDENTICAL
    }

}
