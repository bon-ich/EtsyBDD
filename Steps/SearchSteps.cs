using EtsyBDD.Drivers;
using EtsyBDD.PageObjects;
using NUnit.Framework;
using System;
using TechTalk.SpecFlow;

namespace EtsyBDD.Steps
{
    [Binding]
    public class SearchSteps
    {
        public TestContext? TestContext { get; set; }

        private readonly BrowserDriver _browserDriver;
        private string _baseUrl;
        private HomePage? _homePage;
        private SearchResultsPage? _searchResultsPage;

        public SearchSteps(BrowserDriver browserDriver)
        {
            _browserDriver = browserDriver;
            _baseUrl = TestContext.Parameters["url"] ?? "";
        }

        [Given(@"home page is open")]
        public void GivenEtsyOpen()
        {
            _homePage = new HomePage(_browserDriver.Current, _baseUrl);
            _homePage = _homePage.GoToPage();
        }

        [Given(@"search query is ""(.*)""")]
        public void GivenSearchQueryIs(string searchQuery)
        {
            _homePage!.EnterSearchQuery(searchQuery);
        }

        [When(@"search is run")]
        public void WhenSearchIsRun()
        {
            _searchResultsPage = _homePage!.ClickSearchButton();
        }

        [Then(@"every item in search results has ""(.*)"" in title")]
        public void ThenEveryItemInSearchResultsHasInTitle(string searchQuery)
        {
            bool titlesHaveQuery = _searchResultsPage!.AllItemTitlesContainSearchQuery(searchQuery);
            Assert.IsTrue(titlesHaveQuery);
        }
    }
}
