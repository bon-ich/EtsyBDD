using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace EtsyBDD.PageObjects
{
    class ItemPage
    {
        private readonly IWebDriver _driver;
        private WebDriverWait _wait;
        private const int _waitTime = 5;

        private const string _title = "//h1";

        public ItemPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(_waitTime));
        }

        public string GetItemTitle()
        {
            var title = _driver.FindElement(By.XPath(_title));
            return title.Text;
        }

        public void CloseTab()
        {
            // switch back to the 1st tab
            _driver.Close();
            _driver.SwitchTo().Window(_driver.WindowHandles[0]);
        }
    }
}