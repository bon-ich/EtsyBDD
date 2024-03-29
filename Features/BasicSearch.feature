﻿Feature: Basic Search Functionality

@ignore
@Search
Scenario: Search with multiword query
	Given unauthorized user is on the home page
	When user searches for "leather bag"
	Then every item in search results has "leather bag" in title

@ignore
@Search
Scenario: Search with bad query doesn't give results
	Given unauthorized user is on the home page
	When user searches for "dkjfghdjfghdfjg"
	Then no search results available for "dkjfghdjfghdfjg"

@ignore
@Search
Scenario: Search result items can be opened
	Given unauthorized user is on the home page
	When user searches for "leather bag"
	Then every item in search results can be opened

@ignore
@Search @Filters
Scenario: Search results items respect predefined filter
	Given unauthorized user is on the home page
	When user searches for "leather bag"
	And user applies "USD 25 to USD 50" filter  
	Then search results respect the applied filter

@ignore
@Search @Filters
Scenario: Search results items respect custom price filter
	Given unauthorized user is on the home page
	When user searches for "leather bag"
	And user applies custom filter to price between 125.00 and 250.50
	Then search results respect the applied filter 

@ignore
@Search @Sorting
Scenario: Search result items respect sorting by highest price
	Given unauthorized user is on the home page
	When user searches for "cross stitch"
	And user sorts search results by "Highest Price"
	Then search results respect applied sorting

@Search
Scenario: Proper search result item is open when ckicked
	Given unauthorized user is on the home page
	When user searches for "cross stitch"
	Then all item link leads to proper item page