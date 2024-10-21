using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
namespace TestReports
{
    public class AutomationBase
    {
        public IWebDriver driver { get; set; }
        public static DateTime testingDay {  get; set; }
        
        public static DateTime timeStart { get; set; }
        public static DateTime timeEnd { get; set; }

        public static string TestCaseID;
        public static string _log;


        [SetUp]
        public void SetUp()
        {
            timeStart = DateTime.Now;
            testingDay = DateTime.Now;
            driver = new ChromeDriver();
        }

        [TearDown]
        public void TearDown()
        {
            timeEnd = DateTime.Now;
            ReportExcel();
            driver.Quit();
        }

        #region Tạo báo cáo

        private string GetReportFolder()
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory + "Report";
            string currentWeek = "Week" + CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Today, CultureInfo.CurrentUICulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentUICulture.DateTimeFormat.FirstDayOfWeek).ToString();
            string currentFolderReport = Path.Combine(rootPath, currentWeek);
            int i = 0;

            while (true)
            {
                if (!Directory.Exists(currentFolderReport))
                {
                    Directory.CreateDirectory(currentFolderReport);
                    return currentFolderReport;
                }
                else
                {
                    ++i;
                    currentFolderReport = Path.Combine(rootPath, currentWeek + "_" + i);
                }
            }
        }

        private void AllBorders(Microsoft.Office.Interop.Excel.Borders _border)
        {
            _border[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            _border[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            _border[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            _border[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            _border.Color = System.Drawing.Color.Black;
        }
        private void ReportExcel()
        {
            var excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = true;

            string filePath = GetReportFolder() + "\\Report.xlsx";
            var Page = Regex.Split(this.ToString(),@"\.").ToList();

            if (!File.Exists(filePath))
            {
                excel.Application.Workbooks.Add(Type.Missing);

                #region Summary Report
                excel.Range["B3:D5"].Merge(Type.Missing);
                excel.Range["B3:D5"].Interior.Color = System.Drawing.ColorTranslator.FromHtml("#54FF9F");
                excel.Range["B3:D5"].Font.Bold = true;
                excel.Range["B3"].Value = "Summary";
                excel.Range["B3"].Font.Size = 20;
                excel.Range["B3"].VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                excel.Range["B3"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                excel.Range["B6"].Value = "Passed";
                excel.Range["C6"].Value = "Failed";
                excel.Range["D6"].Value = "Error";
                excel.Range["B7"].Formula = "=COUNTIF(F:F,\"Passed\")";
                excel.Range["C7"].Value = "=COUNTIF(F:F,\"Failed\")";
                excel.Range["D7"].Value = "=COUNTIF(F:F,\"Error\")";
                AllBorders(excel.Range["B3:D7"].Borders);

                #endregion

                #region Testing Information
                var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)excel.Worksheets[1];

                worksheet.Range["B12:D12"].Merge(Type.Missing);
                worksheet.Range["B12:D12"].Interior.Color = System.Drawing.ColorTranslator.FromHtml("#54FF9F");
                worksheet.Range["B12:D12"].Font.Bold = true;
                worksheet.Range["B12"].Value = "Testing Information";
                worksheet.Range["B12"].Font.Size = 20;
                worksheet.Range["B12"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                worksheet.Range["B12"].VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                worksheet.Range["B13"].Value = "Total Time Run";
                worksheet.Range["B14"].Value = "Start time";
                worksheet.Range["B15"].Value = "Stop time";

                TimeSpan totalTime = timeEnd - timeStart;  
               
                worksheet.Range["C13"].Value = totalTime.ToString(@"hh\:mm\:ss");
                worksheet.Range["C14"].Value = timeStart.ToString("dd/MM/yyyy HH:mm:ss");
                worksheet.Range["C15"].Value = timeEnd.ToString("dd/MM/yyyy HH:mm:ss");

                AllBorders(worksheet.Range["B12:C15"].Borders);

                worksheet.Range["B13:C15"].Font.Size = 14;
                worksheet.Range["B13:C15"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                worksheet.Range["B13:C15"].VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                #endregion

                #region Chart
                var ws = (Microsoft.Office.Interop.Excel.Worksheet)excel.Worksheets.get_Item(1);
                var chart = (Microsoft.Office.Interop.Excel.ChartObjects)ws.ChartObjects(Type.Missing);
                var myChart = (Microsoft.Office.Interop.Excel.ChartObject)chart.Add(630, 10, 250, 250);
                var chartPage = (Microsoft.Office.Interop.Excel.Chart)myChart.Chart;

                var seriesCollection = chartPage.SeriesCollection();
                var series = seriesCollection.NewSeries();
                series.XValues = ws.Range["B6","D6"];
                series.Values = ws.Range["B7","D7"];

                chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlPie;
                chartPage.ApplyLayout(6);
                chartPage.ChartTitle.Text = "Summary";
                chartPage.ChartTitle.Font.Size = 10;
                ((Microsoft.Office.Interop.Excel.LegendEntry)chartPage.Legend.LegendEntries(1)).LegendKey.Interior.Color = (int)Microsoft.Office.Interop.Excel.XlRgbColor.rgbBlue;
                ((Microsoft.Office.Interop.Excel.LegendEntry)chartPage.Legend.LegendEntries(2)).LegendKey.Interior.Color = (int)Microsoft.Office.Interop.Excel.XlRgbColor.rgbRed;
                ((Microsoft.Office.Interop.Excel.LegendEntry)chartPage.Legend.LegendEntries(3)).LegendKey.Interior.Color = (int)Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightSalmon;
                
                
                #endregion

                #region Header
                excel.Range["B21"].Value = "Test Case ID";
                excel.Range["C21"].Value = "Page";
                excel.Range["D21"].Value = "Test Case Name";
                excel.Range["E21"].Value = "URL";
                excel.Range["F21"].Value = "Result";
                excel.Range["G21"].Value = "Log";
                excel.Range["H21"].Value = "Thoi gian Test";
                excel.Range["B21:H21"].Interior.Color = System.Drawing.ColorTranslator.FromHtml("#54FF9F");
                excel.Range["B21:H21"].Font.Size = 14;
                worksheet.Range["B21:H21"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                worksheet.Range["B21:H21"].VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                AllBorders(excel.Range["B21:H21"].Borders);
                #endregion

                #region Data
                excel.Range["B22"].Value = TestCaseID;

                // Tên màn hình : 
                // Truyền vào hoặc lấy dữ liệu từ file data test
                excel.Range["C22"].Value = Page[1];
                excel.Range["D22"].Value = TestContext.CurrentContext.Test.MethodName;
                excel.Range["E22"].Value = this.driver.Url.ToString();
                excel.Range["F22"].Value = TestContext.CurrentContext.Result.Outcome.Status.ToString();

                // Log
                // Tự sinh Log
                excel.Range["G22"].Value = _log.ToString();
                excel.Range["H22"].NumberFormat = "hh:mm:ss";
                excel.Range["H22"].Value = totalTime.ToString(@"hh\:mm\:ss"); ;
                AllBorders(excel.Range["B22:H22"].Borders);
                #endregion

                excel.Range["C16"].Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                excel.ActiveWorkbook.SaveAs(filePath);
            }
            else
            {
                excel.ActiveWorkbook.Saved = true;
                excel.ActiveWorkbook.Close();
                excel.Quit();
            }
        }

        #endregion

    }
}
