Feature: Google Search

@UI
Scenario Outline: Search results must not exceed 10 items per page
    Given The user searches for <Criteria>
    When Displaying search results
    Then the number of results must not exceed 10
    Examples:
      | Criteria	        |
      | The name of the wind|
      | MAS Global          |
