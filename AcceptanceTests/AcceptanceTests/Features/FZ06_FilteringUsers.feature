@FZ06
Feature: FZ06_FilteringUsers

As a user
I want to filter potential partners
So that I can find users that match my preferences

Background:
	Given I launch the application for filtering users
	And I am on the home form for filtering users

Scenario: Users are filtered by one selected criterion
	When I open the filter form for user filtering
	And I apply filters with gender Male
	Then filtered users should be displayed

Scenario: Users are filtered by multiple selected criteria
	When I open the filter form for user filtering
	And I apply filters with gender Male and level Intermediate
	Then filtered users should be displayed

Scenario: Users are filtered by all selected criteria
	When I open the filter form for user filtering
	And I apply filters with gender Male, level Intermediate, position Right and frequency Weekly
	Then filtered users should be displayed

Scenario: All users are shown when all filters are set to Any
	When I open the filter form for user filtering
	And I apply filters with all values set to Any
	Then filtered users should be displayed

Scenario: Results change after changing filter values
	When I open the filter form for user filtering
	And I apply filters with gender Male
	And I remember the first displayed player
	And I open the filter form for user filtering
	And I apply filters with gender Female
	Then the displayed player should be different