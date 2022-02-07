using OpenQA.Selenium;

namespace EtsyBDD.PageObjects
{
    class HomePage
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl;

        private const string _searchField = "//input[@name='search_query']";
        private const string _submitSearchButton = "//button[contains(@class, 'global-enhancements-search-input-btn-group__btn')]";

        private IWebElement SearchField => _driver.FindElement(By.XPath(_searchField));
        private IWebElement SearchButton => _driver.FindElement(By.XPath(_submitSearchButton));

        public HomePage(IWebDriver driver, string baseUrl)
        {
            _driver = driver;
            _baseUrl = baseUrl;
        }

        public HomePage GoToPage()
        {
            _driver.Navigate().GoToUrl(_baseUrl);
            return this;
        }

        public HomePage EnterSearchQuery(string query)
        {
            SearchField.Clear();
            SearchField.SendKeys(query);
            return this;
        }

        public SearchResultsPage ClickSearchButton()
        {
            SearchButton.Click();
            return new SearchResultsPage(_driver);
        }
    }
}
