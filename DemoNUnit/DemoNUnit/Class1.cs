using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace DemoNUnit
{
    [TestFixture]
    public class Class1
    {
        private IWebDriver driver;
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }
        [Test]
        public void Test()
        {
            driver.Url = "https://doaitran.super.site/";
            Thread.Sleep(5000);
        }

        [TearDown]
        public void Finish()
        {
            driver.Quit();
        }
    }
}
