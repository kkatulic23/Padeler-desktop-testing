@FZ03
Feature: FZ03_GeolocationSearch

As a user
I want to filter nearby players by radius
So that I can find players near my current location

Background:
	Given I launch the application for geolocation
	And I am on the home form

Scenario: Successful filtering by current location
	When I open the filter form
	And I apply geolocation filters with radius 50 km
	Then nearby players should be displayed on the home screen

Scenario: Players are displayed within a random selected radius
	When I open the filter form
	And I apply geolocation filters with a random radius
	Then displayed players should be within the selected random radius

Scenario: Minimum radius filtering works correctly
	When I open the filter form
	And I apply geolocation filters with radius 1 km
	Then displayed players should be within 1 km
