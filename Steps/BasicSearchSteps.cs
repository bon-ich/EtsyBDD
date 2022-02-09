using EtsyBDD.Drivers;
using EtsyBDD.PageObjects;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace EtsyBDD.Steps
{
    [Binding]
    public class BasicSearchSteps
    {
        public TestContext? TestContext { get; set; }

        private readonly BrowserDriver _browserDriver;
        private string _baseUrl;
        private HomePage? _homePage;
        private SearchResultsPage? _searchResultsPage;

        public BasicSearchSteps(BrowserDriver browserDriver)
        {
            _browserDriver = browserDriver;
            _baseUrl = TestContext.Parameters["url"] ?? "";
        }

        [Given(@"unauthorized user is on the home page")]
        public void GivenUnauthorizedUserOpensHomePage()
        {
            _homePage = new HomePage(_browserDriver.Current, _baseUrl);
            _homePage = _homePage.GoToPage();
        }

        [When(@"user searches for ""(.*)""")]
        public void WhenUserSearchesFor(string searchQuery)
        {
            _homePage!.EnterSearchQuery(searchQuery);
            _searchResultsPage = _homePage!.ClickSearchButton();
        }

        [Then(@"every item in search results has ""(.*)"" in title")]
        public void ThenEveryItemInSearchResultsHasInTitle(string searchQuery)
        {
            bool titlesHaveQuery = _searchResultsPage!.AllItemTitlesContainSearchQuery(searchQuery);
            Assert.IsTrue(titlesHaveQuery);
        }

        [Then(@"no search results available for ""(.*)""")]
        public void ThenNoSearchResultsAvailable(string searchQuery)
        {
            bool pass = _searchResultsPage!.NoResultsTextContainsSearchQuery(searchQuery);
            Assert.IsTrue(pass);
        }

        [Then(@"every item in search results can be opened")]
        public void ThenEveryItemInResultCanBeOpened()
        {
            bool pass = _searchResultsPage!.AllSearchItemsHaveLink();
            Assert.IsTrue(pass);
        }
    }
}
