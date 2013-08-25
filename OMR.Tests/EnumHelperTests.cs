using System;
using OMR.Core.Helpers;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OMR.Tests
{
    [TestClass]
    public class EnumHelperTests
    {
        internal enum EnumNonValuedTest
        {
            [System.ComponentModel.Description("0")]
            Zero,

            [System.ComponentModel.Description("1")]
            One
        }

        internal enum EnumValuedTest
        {
            [System.ComponentModel.Description("RED 10")]
            Red = 10,

            [System.ComponentModel.Description("GREEN 30")]
            Green = 30,

            [System.ComponentModel.Description("BLUE 70")]
            Blue = 70,

            [System.ComponentModel.Description("PURPLE 120")]
            Purple = 120
        }

        internal class CustomMapPoco
        {
            public int Value { get; set; }

            public string Description { get; set; }

            public CustomMapPoco(int value, string description)
            {
                Value = value;
                Description = description;
            }

        }

        [TestMethod]
        public void EnumHelper_GetValue_ValuedEnum()
        {
            var expected = 70;
            var actual = EnumHelper.GetValue<EnumValuedTest>("BLUE 70");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnumHelper_GetValue_NonValuedEnum()
        {
            var expected = 1;
            var actual = EnumHelper.GetValue<EnumNonValuedTest>("1");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnumHelper_GetDescription_ValuedEnum()
        {
            var expected = "RED 10";

            var actual = EnumValuedTest.Red.GetDescription();

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void EnumHelper_GetDescription_NonValuedEnum()
        {
            var expected = "1";

            var actual = EnumNonValuedTest.One.GetDescription();

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void EnumHelper_GetAsDictionaryInt32_String_ValuedEnum()
        {
            var dictionary = EnumHelper.GetAsDictionary<EnumValuedTest>();
            var expected = 10;
            var actual = dictionary["RED 10"];

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnumHelper_GetAsDictionaryInt32_String_NonValuedEnum()
        {
            var dictionary = EnumHelper.GetAsDictionary<EnumNonValuedTest>();
            var expected = 1;
            var actual = dictionary["1"];

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnumHelper_GetAsCustomType_ValuedEnum()
        {
            var ie = EnumHelper.GetAsCustomType<EnumValuedTest, CustomMapPoco>((k, v) => { return new CustomMapPoco(v, k); });
            var expected = 10;
            
            var enumerator = ie.GetEnumerator();
            enumerator.MoveNext();

            Assert.AreEqual(expected, enumerator.Current.Value);
        }

    }
}
