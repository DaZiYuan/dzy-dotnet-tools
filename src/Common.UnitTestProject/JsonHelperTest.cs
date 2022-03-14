using Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.UnitTestProject
{
    public class TestObj
    {
        public int PInt { get; set; } = 0;
        public string PString { get; set; } = "1";
        public double PDouble { get; set; } = 2.1;

    }
    [TestClass]
    public class JsonHelperTest
    {
        [TestMethod]
        public void Test()
        {
            TestObj obj = new();
            var json = JsonHelper.JsonSerialize(obj);
            Assert.AreEqual(json, "{\"PInt\":0,\"PString\":\"1\",\"PDouble\":2.1}");

            var obj1 = JsonHelper.JsonDeserialize<TestObj>(json);
            Assert.AreEqual(obj.PDouble, obj1.PDouble);
            Assert.AreEqual(obj.PInt, obj1.PInt);
            Assert.AreEqual(obj.PString, obj1.PString);
        }
    }
}
