using NUnit.Framework;
using System.Threading;


namespace TestReports
{
    public class TestGoogle : AutomationBase
    {
        [Test]
        public void TestDemo()
        {
            driver.Url = "https://www.google.com/";
            _log += "Pass demo Google";
            Thread.Sleep(4000);
            Assert.Pass(_log);
        }
    }
}
