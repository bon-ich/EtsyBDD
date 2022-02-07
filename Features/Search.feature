Feature: Search

@Search
Scenario: Search with multiword query
	Given home page is open
	And search query is "leather bag"
	When search is run
	Then every item in search results has "leather bag" in title