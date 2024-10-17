using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Register
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test case Start");

            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.facebook.com/r.php");

            // Xử lý textbox
            IWebElement firstNameTextBox = driver.FindElement(By.Name("firstname"));
            firstNameTextBox.SendKeys("Kiet");
            Thread.Sleep(3000);

            // Xử lý Dropdown
            IWebElement monthDropdown = driver.FindElement(By.Name("birthday_month"));
            SelectElement monthSelect = new SelectElement(monthDropdown);
            monthSelect.SelectByText("Sep");
            Thread.Sleep(3000);

            // Xử lý Radio Button
            IWebElement maleRadioButton = driver.FindElement(By.XPath("//input[@value='2']")); // value='2' cho giới tính nam
            maleRadioButton.Click();
            Thread.Sleep(3000);

            // Kiểm tra trạng thái của Radio Button đã được chọn
            bool isGenderSelected = maleRadioButton.Selected;
            if (isGenderSelected)
            {
                Console.WriteLine("Radio Button Male đã được chọn");
            }
            else
            {
                Console.WriteLine("Radio Button Male chưa được chọn");
            }

            // Xử lý button
            IWebElement signUpButton = driver.FindElement(By.Name("websubmit"));
            signUpButton.Click();
            Thread.Sleep(3000);

            // Đóng trình duyệt
            driver.Quit();

            Console.WriteLine("Test case Ended");

        }
    }
}
