Feature: FZ04_Interaction

Background: 
	Given I am logged into the application
	And I am on the interaction screen

Scenario: Available player profiles are displayed
	When the interaction screen is opened
	Then player cards should be displayed

Scenario: Skip action show the next player
	Given at least two cards are available
	When I click the skip button
	Then the nex player card should be displayed

Scenario: Like action show the next player
	Given at least two cards are available
	When I click the like button
	Then the next player card should be displayed

Scenario: Mutual like creates a match
	Given another player has already clicked the like button
	When I click the like button
	Then a match notificaiton should be displayed
