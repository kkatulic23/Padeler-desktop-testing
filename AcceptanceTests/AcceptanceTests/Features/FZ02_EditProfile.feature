@FZ02
Feature: FZ02_EditProfile

As a user
I want to edit my profile
So that I can keep my profile data updated

Background:
	Given I am logged into the application for profile editing
	And I open the profile screen

Scenario: Existing profile data is displayed
	Then existing profile data should be displayed

Scenario: User can successfully save valid profile changes
	When I change the name to "KristianTest"
	And I click the save button on profile
	Then profile changes should be saved successfully

Scenario: System does not allow saving with empty required field
	When I clear the name field
	And I click the save button on profile
	Then I should see a validation error containing "Name is required."

Scenario: System does not allow invalid email format
	When I enter invalid email "kriviemail"
	And I click the save button on profile
	Then I should see a validation error containing "A valid email is required."

Scenario: Changes are not saved if user leaves without clicking Save
	Given current profile name is remembered
	When I change the name without saving
	And I leave the profile screen
	And I return to the profile screen
	Then unsaved profile changes should not be persisted