Feature: FZ04_Interaction

Background: 
	Given I am logged into the application
	And I am on the interaction screen

Scenario: Available player profiles are displayed
	When the interaction screen is opened
	Then player cards should be displayed
