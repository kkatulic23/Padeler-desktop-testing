Feature: FZ09_MatchList

As a logged in user
I want to manage my matched users
So that I can view, rename and remove them from my match list

Background: 
	Given I am logged into the application
	And I am on the Match screen

Scenario: Matched users are displayed in the list
	When the Match screen is opened
	Then matched users should be displayed

Scenario: Nickname can be changed for a matched user
	Given at least one matched user is displayed
	When I enter a nickname for the matched user
	And I click the save button
	Then the nickname should be saved

Scenario: Matched user can be deleted from the list
	Given at least one matched user is displayed
	When I select a matched user
	And I click the delete button
	Then the matched user should no longer be displayed
