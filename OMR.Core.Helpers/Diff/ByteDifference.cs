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

        public static byte[] ApplyChanges(byte[] bytes, List<ComparisonResult<ByteData>> result)
        {
            var listOfBytes = new List<byte>(bytes);

            var orderedResults = result.OrderByDescending(c => c.Value.Order);

            foreach (var item in orderedResults)
            {
                if (item.Target == ComparisonResultType.CONFLICT)
                {
                    listOfBytes[item.Value.Order] = item.Value.Value;
                }
                else if (item.Target == ComparisonResultType.WCREATE)
                {
                    listOfBytes.Insert(item.Value.Order, item.Value.Value);
                }
                else if (item.Target == ComparisonResultType.WDELETE)
                {
                    listOfBytes.RemoveAt(item.Value.Order);
                }
            }

            return bytes.ToArray();
        }

       
    }
}
