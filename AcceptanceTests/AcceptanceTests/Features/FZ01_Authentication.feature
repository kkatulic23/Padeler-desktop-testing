@FZ01
Feature: FZ01_Authentication

As a user
I want to register, log in and log out
So that I can use the application

# REGISTRATION

Background:
	Given I launch the application

Scenario: Successful user registration
	Given I open the registration form
	When I enter valid registration data
	And I click the register button
	Then the registration should be successful

Scenario: Registration with existing username
	Given I open the registration form
	When I enter an existing username
	And I click the register button
	Then an error message should be displayed

Scenario: Registration with existing email
	Given I open the registration form
	When I enter an existing email
	And I click the register button
	Then an error message should be displayed

Scenario: Registration with empty fields
	Given I open the registration form
	When I leave required fields empty
	And I click the register button
	Then a validation error message should be displayed

Scenario: Registration with invalid email format
	Given I open the registration form
	When I enter an invalid email format
	And I click the register button
	Then a validation error message should be displayed

# LOGIN

Scenario: Successful login
	Given I am on the login form
	When I enter valid credentials
	And I click the login button
	Then I should be logged into the application

Scenario: Login with wrong password
	Given I am on the login form
	When I enter a valid username and wrong password
	And I click the login button
	Then an error message should be displayed

Scenario: Login with wrong username
	Given I am on the login form
	When I enter a wrong username and valid password
	And I click the login button
	Then an error message should be displayed

Scenario: Login with empty fields
	Given I am on the login form
	When I leave username and password empty
	And I click the login button
	Then a validation error message should be displayed

# SESSION & LOGOUT

Scenario: User stays logged in after closing the application
	Given I am logged in
	When I close the application
	Then I should still be logged in

Scenario: User can log out
	Given I am logged in
	When I click the logout button
	Then I should be redirected to the login form

Scenario: User is not logged in after logout
	Given I log out from the application
	When I close the application
	Then I should see the login form

