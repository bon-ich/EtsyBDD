using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace EtsyBDD.PageObjects
{
    class SearchResultsPage
    {
        private readonly IWebDriver _driver;

        private const string _searchResultItem = "//div[contains(@class, 'search-listings-group')]//ul[contains(@class, 'tab-reorder-container')]//li";
        private const string _searchResultItemsTitle = "//div[contains(@class, 'search-listings-group')]//ul[contains(@class, 'tab-reorder-container')]//li//h3";
        private const string _noResultsText = "//p[@class='wt-text-heading-02 wt-pt-xs-8']";
        private const string _searchResultItemLink = "//div[contains(@class, 'search-listings-group')]//ul[contains(@class, 'tab-reorder-container')]//li//a[contains(@class, 'listing-link')]";

        public SearchResultsPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public IList<IWebElement> GetSearchResultItems()
        {
            IList<IWebElement> foundItems = new List<IWebElement>();
            foundItems = _driver.FindElements(By.XPath(_searchResultItem));
            return foundItems;
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
    }
}
