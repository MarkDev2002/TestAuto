using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        IWebDriver driver = new ChromeDriver();
        // Chuyển đến website cellphones mục tablet
        Console.WriteLine("Test case start");
        driver.Navigate().GoToUrl("https://cellphones.com.vn/tablet.html/");

        List<string[]> items = new List<string[]>();

        // Tìm kiếm class bao trọn sản phẩm
        IReadOnlyCollection<IWebElement> productElements = driver.FindElements(By.ClassName("product-info-container"));

        foreach (IWebElement productElement in productElements)
        {
            // Lấy ra từng element của tên và giá sản phẩm
            string name = productElement.FindElement(By.ClassName("product__name")).Text;
            string price = productElement.FindElement(By.ClassName("product__price--show")).Text;
        }

        // Đường dẫn File muốn lưu dưới dạng Excel
        string csvFilePath = @"C:\Users\myblue\Desktop\TestSele\Crawl\items.csv";

        // Lưu dữ liệu vào file excel theo UTF8
        using (StreamWriter writer = new StreamWriter(csvFilePath, false, Encoding.UTF8))
        {
            writer.WriteLine("Name,Price");
            foreach (string[] item in items)
            {
                writer.WriteLine(string.Join(",", item));
            }
        }
        Console.WriteLine("Data have been save");
        driver.Quit();
    }
}

