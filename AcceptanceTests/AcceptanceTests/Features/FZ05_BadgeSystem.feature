@FZ05
Feature: FZ05_BadgeSystem

As a user
I want to earn and see badges
So that I can track my progress in the app

Background:
	Given I am logged into the application for badges
	And I am on the Home screen

Scenario: Earned badges are displayed on Home screen
	When the Home screen is opened
	Then earned badges should be displayed in the badges panel

Scenario: Swipe can unlock a new badge
	Given I remember the current number of displayed badges
	When I perform one swipe action
	Then a badge unlock message may appear
	And the number of displayed badges should stay the same or increase

Scenario: User can see previously earned badges on Home screen
	Given badges are already displayed
	When I reopen the Home screen
	Then earned badges should still be displayed in the badges panel