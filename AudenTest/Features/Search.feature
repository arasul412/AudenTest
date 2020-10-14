Feature: Check loan affordability

As a user, I want to be able to check the loan affordability

Scenario: Select loan amount other than the default value

Given a web browser is at the Auden loan page
When I select the loan_amount and repayment day as weekend
	| loan_amount | weekend		|
	| £250        | 18 Oct 2020	|
Then the loan amount and the first repayment day are shown as Friday

