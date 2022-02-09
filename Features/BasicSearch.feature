Feature: Basic Search Functionality

@Search
Scenario: Search with multiword query
	Given unauthorized user is on the home page
	When user searches for "leather bag"
	Then every item in search results has "leather bag" in title

Scenario: Search with bad query doesn't give results
	Given unauthorized user is on the home page
	When user searches for "dkjfghdjfghdfjg"
	Then no search results available for "dkjfghdjfghdfjg"

Scenario: Search result items can be opened
	Given unauthorized user is on the home page
	When user searches for "leather bag"
	Then every item in search results can be opened
