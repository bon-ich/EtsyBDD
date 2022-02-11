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
        private string _appliedFilter = "";
        private decimal _minPrice = 0;
        private decimal _maxPrice = 0;

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

        [When(@"user applies ""(.*)"" filter")]
        public void WhenUserAppliesPredefinedPriceFilter(string filterName)
        {
            _searchResultsPage!.ApplyFilter(filterName);
            _appliedFilter = filterName;
        }

        [When(@"user applies custom filter to price between (.*) and (.*)")]
        public void WhenUserAppliesCustomPriceFilter(decimal minPrice, decimal maxPrice)
        {
            _searchResultsPage!.ApplyCustomPriceFilter(minPrice, maxPrice);
            _minPrice = minPrice;
            _maxPrice = maxPrice;
            _appliedFilter = "Custom Price Filter";
        }

        [Then(@"search results respect the applied filter")]
        public void ThenSearchResultsRespectAppliedFilter()
        {
            bool pass = false;
            switch (_appliedFilter)
            {
                case "USD 25 to USD 50":
                    pass = _searchResultsPage!.DoSearchResultsRespectPriceFilter(25, 50);
                    break;
                case "Custom Price Filter":
                    pass = _searchResultsPage!.DoSearchResultsRespectPriceFilter(_minPrice, _maxPrice);
                    break;
            }
            Assert.IsTrue(pass);
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
