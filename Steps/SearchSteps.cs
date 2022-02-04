using System;
using TechTalk.SpecFlow;

namespace EtsyBDD.Steps
{
    [Binding]
    public class SearchSteps
    {
        [Given(@"search query is ""(.*)""")]
        public void GivenSearchQueryIs(string p0)
        {
            
        }
        
        [When(@"search is run")]
        public void WhenSearchIsRun()
        {
        }
        
        [Then(@"every item in search results has ""(.*)"" in title")]
        public void ThenEveryItemInSearchResultsHasInTitle(string p0)
        {
        }
    }
}
