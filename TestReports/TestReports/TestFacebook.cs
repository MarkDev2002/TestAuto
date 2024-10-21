using NUnit.Framework;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace TestReports
{
    public class TestFacebook : AutomationBase
    {
        [Test]
        public void TestFacebookHomePage()
        {
            driver.Url = "https://www.facebook.com/";
            _log = "Navigated to Facebook homepage";

            // Assert that we have landed on the homepage by checking the title
            string pageTitle = driver.Title;
            ClassicAssert.IsTrue(pageTitle.Contains("Facebook"), "Page title does not contain 'Facebook'.");
            _log += "\nPage title check passed";

            // Log the result
            Assert.Pass(_log);
        }

        [Test]
        public void TestFacebookRegistration()
        {
            driver.Url = "https://www.facebook.com/r.php/";

            // Find and interact with registration elements
            IWebElement firstNameInput = driver.FindElement(By.Name("firstname"));
            IWebElement lastNameInput = driver.FindElement(By.Name("lastname"));
            IWebElement emailInput = driver.FindElement(By.Name("reg_email__"));
            IWebElement passwordInput = driver.FindElement(By.Name("reg_passwd__"));

            // Fill in registration form
            firstNameInput.SendKeys("John");
            lastNameInput.SendKeys("Doe");
            emailInput.SendKeys("johndoe@example.com");
            passwordInput.SendKeys("SuperSecret123!");

            _log += "\nFilled in registration form";

            // Click the "Sign Up" button
            IWebElement signUpButton = driver.FindElement(By.Name("websubmit"));
            signUpButton.Click();

            _log += "\nClicked on Sign Up";

            // Wait for the page to load (wait for some element to appear)
            Thread.Sleep(5000);

            // Assert based on expected outcome (e.g., error message if email is already in use)
            // For demonstration, we'll check if an error message related to the email appears
            try
            {
                IWebElement errorElement = driver.FindElement(By.CssSelector("#reg_error_inner"));
                string errorText = errorElement.Text;
                ClassicAssert.IsTrue(errorText.Contains("email"), "Expected error message related to email did not appear.");
                _log += "\nError message appeared: " + errorText;
            }
            catch (NoSuchElementException)
            {
                _log += "\nNo error message found, registration might have gone through.";
            }

            Assert.Pass(_log);
        }

        [Test]
        public void TestInvalidRegistration()
        {
            driver.Url = "https://www.facebook.com/r.php/";

            // Find the registration form elements
            IWebElement firstNameInput = driver.FindElement(By.Name("firstname"));
            IWebElement lastNameInput = driver.FindElement(By.Name("lastname"));
            IWebElement emailInput = driver.FindElement(By.Name("reg_email__"));
            IWebElement passwordInput = driver.FindElement(By.Name("reg_passwd__"));

            // Fill the form with invalid data
            firstNameInput.SendKeys("");
            lastNameInput.SendKeys("");
            emailInput.SendKeys("invalid-email");
            passwordInput.SendKeys("123");

            _log += "\nFilled in invalid registration data";

            // Click the "Sign Up" button
            IWebElement signUpButton = driver.FindElement(By.Name("websubmit"));
            signUpButton.Click();

            _log += "\nClicked on Sign Up with invalid data";

            // Wait for the error message
            Thread.Sleep(5000);

            // Assert based on the error message
            try
            {
                IWebElement errorElement = driver.FindElement(By.CssSelector("#reg_error_inner"));
                string errorText = errorElement.Text;
                ClassicAssert.IsTrue(errorText.Contains("valid email"), "Expected error message for invalid email did not appear.");
                _log += "\nError message appeared: " + errorText;
            }
            catch (NoSuchElementException)
            {
                _log += "\nNo error message found, test failed.";
                Assert.Fail(_log);
            }

            Assert.Pass(_log);
        }
    }
}
