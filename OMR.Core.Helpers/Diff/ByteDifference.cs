using System;
using System.Collections.Generic;
using System.Linq;

namespace OMR.Core.Helpers.Diff
{
    public class ByteDifference
    {
        public static List<ComparisonResult<ByteData>> GetDiff(byte[] source, byte[] destination)
        {
            var gc = new GenericComparer<ByteData>();
            gc.CompareFunc = (a, b) => { return a.Order.CompareTo(b.Order); };
            gc.EqualsFunc = (a, b) => { return a.Value.Equals(b.Value); };

            var wSource = ByteData.GetByteData(source);
            var wDestination = ByteData.GetByteData(destination);

            return gc.Compare(wSource.ToList(), wDestination.ToList());
        }

        //TODO:
        public static byte[] ApplyChanges(byte[] bytes, List<ComparisonResult<ByteData>> result)
        {
            throw new NotImplementedException();
        }

       
    }
}
