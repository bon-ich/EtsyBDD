using NUnit.Framework;
using System;
using TechTalk.SpecFlow;

namespace EtsyBDD.Steps
{
    [Binding]
    public class SearchSteps
    {
        public TestContext TestContext { get; set; }
        string url;
        string username;
        string password;

        [Given(@"search query is ""(.*)""")]
        public void GivenSearchQueryIs(string searchQuery)
        {
            url = TestContext.Parameters["url"] ?? "null";
            Console.WriteLine("search query is " + searchQuery + "and url is " + url);
        }
        
        [When(@"search is run")]
        public void WhenSearchIsRun()
        {
            username = TestContext.Parameters["username"] ?? "null";
            Console.WriteLine("search is run and username is " + username);
        }
        
        [Then(@"every item in search results has ""(.*)"" in title")]
        public void ThenEveryItemInSearchResultsHasInTitle(string p0)
        {
            password = TestContext.Parameters["password"] ?? "null";
            Console.WriteLine("checking search results and password is " + password);
        }
    }
}
