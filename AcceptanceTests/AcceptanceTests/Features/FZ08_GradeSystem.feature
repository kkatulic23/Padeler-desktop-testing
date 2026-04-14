@FZ08
Feature: FZ08_GradeSystem

As a user
I want to rate my match
So that I can leave feedback for other players

Background:
	Given I am logged into the application for rating
	And I open the match list

Scenario: User can open the rating form
	When I open the rating form for the first available match
	Then the rating form should be displayed

Scenario: User can save a rating with a comment
	When I open the rating form for the first available match
	And I set the rating to 5
	And I enter the comment "Odlican suigrac"
	And I submit the rating
	Then the rating should be saved successfully

Scenario: User cannot rate the same player twice
	Given I have already rated the first available match user
	When I try to rate the same user again
	Then an error message about duplicate rating should appear