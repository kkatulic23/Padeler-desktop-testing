@FZ04
Feature: FZ04_Interaction

As a user
I want to be able to see the profile cards
So that I can interact with them

Background: 
	Given I am logged into the application
	And I am on the interaction screen

Scenario: Available player profiles are displayed
	When the interaction screen is opened
	Then player cards should be displayed

Scenario: Skip action show the next player
	Given at least two cards are available
	When I click the skip button
	Then the next player card should be displayed

Scenario: Like action show the next player
	Given at least two cards are available
	When I click the like button
	Then the next player card should be displayed
