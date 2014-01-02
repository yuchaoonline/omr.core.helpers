using Microsoft.VisualStudio.TestTools.UnitTesting;
using OMR.Core.Helpers;
using OMR.Core.Helpers.Diff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OMR.Tests
{
    [TestClass]
    public class GenericComparerTests
    {
        [TestMethod]
        public void DifferentHelloWorldComareTest1()
        {
            var text1 = "Hello world";
            var text2 = "Hello World!";

            var by1 = Encoding.UTF8.GetBytes(text1);
            var by2 = Encoding.UTF8.GetBytes(text2);

            var ba1 = ByteData.GetByteData(by1);
            var ba2 = ByteData.GetByteData(by2);

            var gc = new GenericComparer<ByteData>();
            gc.CompareFunc = (a, b) => { return a.Order.CompareTo(b.Order); };
            gc.EqualsFunc = (a, b) => { return a.Value.Equals(b.Value); };

            var result = gc.Compare(ba2.ToList(), ba1.ToList());

            var newBytes = ByteDifference.ApplyChanges(by1, result);

            var newText = Encoding.UTF8.GetString(newBytes);

            Assert.AreEqual(text2, newText);
        }

        [TestMethod]
        public void DifferentHelloWorldComareTest2()
        {
            var random = new Random();

            var maxBytesCount = 15;

            var bList1 = new byte[maxBytesCount];
            var bList2 = new byte[maxBytesCount];

            var differences = 0;

            for (int i = 0; i < maxBytesCount; i++)
            {
                var randomByte = (byte)random.Next(1,255);

                if (randomByte % 7 == 0)
                {
                    bList2[i] = (randomByte);
                    ++differences;
                    continue;
                }
                else if (randomByte % 11 == 0)
                {
                    bList1[i] = (randomByte);
                    ++differences;
                    continue;
                }
                else
                {
                    bList1[i] = (randomByte);
                    bList2[i] = (randomByte);
                    continue;
                }
            }

            var gc = new GenericComparer<ByteData>();
            gc.CompareFunc = (a, b) => { return a.Order.CompareTo(b.Order); };
            gc.EqualsFunc = (a, b) => { return a.Value.Equals(b.Value); };

            var result = gc.Compare(ByteData.GetByteData(bList2).ToList(), ByteData.GetByteData(bList1).ToList());


            Assert.AreEqual(differences, result.Count);

            var newBytes = ByteDifference.ApplyChanges(bList1, result);

            var s = string.Empty;
            for (int i = 0; i < maxBytesCount; i++)
            {
                s += bList2[i] + "\t" + newBytes[i] + "\n";
            }

            CollectionAssert.AreEqual(bList2, newBytes);
        }

        [TestMethod]
        public void DiffAndPathFile()
        {
            var diff = FileDiffrence.GetDiff(@"c:\tests\f2.txt", @"c:\tests\f1.txt");
            var newBytes = FileDiffrence.ApplyChanges(File.ReadAllBytes(@"c:\tests\f1.txt"), diff);

            File.WriteAllBytes(@"c:\tests\f3.txt", newBytes);
        }
    }

}
