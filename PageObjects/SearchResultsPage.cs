using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace EtsyBDD.PageObjects
{
    class SearchResultsPage
    {
        private readonly IWebDriver _driver;

        private const int _waitTime = 5;

        private const string _searchResultItem = "//div[contains(@class, 'search-listings-group')]//ul[contains(@class, 'tab-reorder-container')]//li";
        private const string _searchResultItemAdItem = "//span[contains(.,'Ad by')]/ancestor::li";
        private const string _searchResultItemsTitle = "//div[contains(@class, 'search-listings-group')]//ul[contains(@class, 'tab-reorder-container')]//li//h3";
        private const string _searchResultItemsPrice = ".//p[@class='wt-text-title-01']//span[@class='currency-value']";
        private const string _noResultsText = "//p[@class='wt-text-heading-02 wt-pt-xs-8']";
        private const string _searchResultItemLink = "//div[contains(@class, 'search-listings-group')]//ul[contains(@class, 'tab-reorder-container')]//li//a[contains(@class, 'listing-link')]";
        private const string _allFiltersButtons = "//button[@id='search-filter-button']";
        private const string _filtersMenu = "//div[@class='main-filters']";
        private const string _filterCheckbox = "//div[@class='main-filters']//label[contains(.,'{0}')]";
        private const string _applyFiltersButton = "//button[@aria-label='Apply']";
        private const string _minPriceFilterField = "//input[@id='search-filter-min-price-input']";
        private const string _maxPriceFilterField = "//input[@id='search-filter-max-price-input']";
        private const string _sortByDropdown = "//button[contains(@title, 'Sort by:')]";
        private const string _sortByOption = "//a[@class='wt-menu__item ' and contains(., '{0}')]";

        private WebDriverWait _wait;

        public SearchResultsPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(_waitTime));
        }

        public IList<IWebElement> GetSearchResultItems()
        {
            IList<IWebElement> foundItems = new List<IWebElement>();
            foundItems = _driver.FindElements(By.XPath(_searchResultItem));
            return foundItems;
        }

        public IList<IWebElement> GetAdItems()
        {
            IList<IWebElement> adItems = new List<IWebElement>();
            adItems = _driver.FindElements(By.XPath(_searchResultItemAdItem));
            Console.WriteLine("ad items count: " + adItems.Count);
            return adItems;
        }

        public void ApplySorting(string sortBy)
        {
            // expand sort dropdown
            IWebElement sortByDropdown = _driver.FindElement(By.XPath(_sortByDropdown));
            WaitUntilElementIsClickable(sortByDropdown);
            sortByDropdown.Click();
            // select sort option
            IWebElement sortByOption = _driver.FindElement(By.XPath(String.Format(_sortByOption, sortBy)));
            WaitUntilElementIsClickable(sortByOption);
            sortByOption.Click();

            // wait until items update
            WaitUntilItemsUpdate();
        }

        public bool IsPriceSortingCorrect(bool highest)
        {
            bool sortingCorrect = true;
            // get items 
            var items = new List<IWebElement>(GetSearchResultItems());
            Console.WriteLine("not filtered items count: " + items.Count);

            var adItems = GetAdItems();

            // filter items
            foreach (IWebElement i in adItems)
            {
                if (items.Contains(i))
                {
                    items.Remove(i);
                }
            }
            Console.WriteLine("filtered items count: " + items.Count);

            // check sorting from high to low
            if (highest)
            {
                double previousPrice = double.MaxValue;
                foreach (IWebElement item in items)
                {
                    var priceText = item.FindElement(By.XPath(_searchResultItemsPrice)).Text;
                    double price = double.Parse(priceText);
                    Console.WriteLine($"checking if {price} < {previousPrice}");
                    if (price > previousPrice)
                    {
                        sortingCorrect = false;
                        break;
                    }
                    if (!sortingCorrect)
                    {
                        break;
                    }
                    previousPrice = price;
                }
            }
            // check sorting from low
            else
            {
                double previousPrice = double.MinValue;
                foreach (IWebElement item in items)
                {
                    var priceText = item.FindElement(By.XPath(_searchResultItemsPrice)).Text;
                    double price = double.Parse(priceText);
                    Console.WriteLine($"checking if {price} > {previousPrice}");
                    if (price < previousPrice)
                    {
                        sortingCorrect = false;
                        break;
                    }
                    if (!sortingCorrect)
                    {
                        break;
                    }
                    previousPrice = price;
                }
            }
            return sortingCorrect;
        }

        public void ApplyCustomPriceFilter(decimal minPrice, decimal maxPrice)
        {
            OpenFilters();
            _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(_filtersMenu)));

            IWebElement minPriceField = _driver.FindElement(By.XPath(_minPriceFilterField));
            minPriceField.SendKeys(minPrice.ToString());
            IWebElement maxPriceField = _driver.FindElement(By.XPath(_maxPriceFilterField));
            maxPriceField.SendKeys(maxPrice.ToString());

            ClickApplyFiltersButton();
            WaitUntilItemsUpdate();
        }

        public List<string> GetItemsTitles()
        {
            IList<IWebElement> foundItemsTitle = _driver.FindElements(By.XPath(_searchResultItemsTitle));
            List<string> titles = new List<string>();
            foreach (IWebElement t in foundItemsTitle)
            {
                titles.Add(t.Text);
            }
            return titles;
        }

        public List<decimal> GetItemsPrices()
        {
            var priceTests = _driver.FindElements(By.XPath(_searchResultItem + _searchResultItemsPrice));
            List<decimal> prices = new List<decimal>();

            foreach (IWebElement p in priceTests)
            {
                decimal price;
                if (decimal.TryParse(p.Text, out price))
                {
                    prices.Add(price);
                }
            }
            return prices;
        }

        public bool AllItemTitlesContainSearchQuery(string searchQuery)
        {
            bool allElementsHaveQueryInTitle = true;

            searchQuery = searchQuery.ToLower();
            var searchTokens = searchQuery.Split(' ');

            var results = GetItemsTitles();

            foreach (string title in results)
            {
                string lowerTitle = title.ToLower();

                foreach (string token in searchTokens)
                {
                    Console.WriteLine($"looking for '{token}' in '{lowerTitle}'");
                    if (!lowerTitle.Contains(token))
                    {
                        allElementsHaveQueryInTitle = false;
                        break;
                    }
                }
                if (allElementsHaveQueryInTitle == false)
                {
                    break;
                }
            }
            return allElementsHaveQueryInTitle;
        }

        public bool NoResultsTextContainsSearchQuery(string searchQuery)
        {
            IWebElement noResultsText = _driver.FindElement(By.XPath(_noResultsText));
            return noResultsText.Text.Contains(searchQuery);
        }

        public bool AllSearchItemsHaveLink()
        {
            int itemsLinks = 0;
            IList<IWebElement> foundLinks = new List<IWebElement>();
            foundLinks = _driver.FindElements(By.XPath(_searchResultItemLink));
            foreach (var link in foundLinks)
            {
                string href = link.GetAttribute("href");
                if (href != null || href != "")
                {
                    itemsLinks += 1;
                }
            }
            var searchResultItems = GetSearchResultItems();
            return itemsLinks == searchResultItems.Count;
        }

        public void ApplyFilter(string filterName)
        {
            OpenFilters();
            _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(_filtersMenu)));
            SelectFilter(filterName);
            ClickApplyFiltersButton();
            WaitUntilItemsUpdate();
        }

        public bool DoSearchResultsRespectPriceFilter(decimal min, decimal max)
        {
            bool respect = true;
            var prices = GetItemsPrices();
            foreach (decimal price in prices)
            {
                Console.WriteLine($"checking if price {price} in [{min}, {max}]");
                if (price <= min || price >= max)
                {
                    Console.WriteLine(@"{price} is not in boundaries");
                    respect = false;
                    break;
                }
            }
            return respect;
        }

        private void WaitUntilItemsUpdate()
        {
            var item = GetSearchResultItems()[0];
            _wait.Until(ExpectedConditions.StalenessOf(item));
        }

        private void WaitUntilElementIsClickable(IWebElement element)
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(element));
        }
        private void OpenFilters()
        {
            IWebElement filtersButton = _driver.FindElement(By.XPath(_allFiltersButtons));
            WaitUntilElementIsClickable(filtersButton);
            filtersButton.Click();
            Console.WriteLine("Clicked open filters button");
        }

        private void SelectFilter(string filterName)
        {
            string selector = String.Format(_filterCheckbox, filterName);
            IWebElement filter = _driver.FindElement(By.XPath(selector));
            WaitUntilElementIsClickable(filter);
            filter.Click();
            Console.WriteLine("Selected filter " + filterName);
        }

        private void ClickApplyFiltersButton()
        {
            IWebElement applyButton = _driver.FindElement(By.XPath(_applyFiltersButton));
            WaitUntilElementIsClickable(applyButton);
            applyButton.Click();
            Console.WriteLine("Clicked apply filters button");
        }
    }
}
