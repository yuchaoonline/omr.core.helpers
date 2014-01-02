using System.Collections.Generic;
using System.IO;

namespace OMR.Core.Helpers.Diff
{
    public class FileDiffrence
    {
        public static List<ComparisonResult<ByteData>> GetDiff(string sourceFilePath, string destinationFilePath)
        {
            var sourceBytes = File.ReadAllBytes(sourceFilePath);
            var destinationBytes = File.ReadAllBytes(destinationFilePath);
            
            return ByteDifference.GetDiff(sourceBytes, destinationBytes);
        }

        public static byte[] ApplyChanges(byte[] bytes, List<ComparisonResult<ByteData>> result)
        {
            return ByteDifference.ApplyChanges(bytes, result);
        }
    }
}
