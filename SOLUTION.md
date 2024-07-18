# My Solution

## First steps + Task 1
After forking the solution and taking a look I had a play around with the application but then realised I need to update the efcore db so I sorted that 
then created a list with some items.  
After that I spent some time familiarising myself with how the code was structured and what the unit tests covered. 
This involved running dotcover over the solution to see what the coverage was like.

## Task 2

For this I was suprised that the ordering had to be done on the view itself, out of interest I ordered the list in the backend to see if that would work, I also added tests for these. The backend code worked (it's not the prettiest though as I'm relatively new using efcore) however the changes did not replicate in the frontend, I assume this was something to with how Razor works (unfortunately i've never used razor).  

I then implemented the OrderBy on the list in the view.

## Task 3

This was a simple one as the value was hardcoded in the TodoItemEditFields.cs file.

## Task 4 

For this I added the DisplayName attribute to the field, I'm not sure what the best friendly text to use would be as I don't know the end users of this system and how technical they are.  

This would be something I'd look to get from a BA or UX person

## Task 5

Ideally I'd like this done in the backend with an extra endpoint that only returns non completed items or to update the existing endpoint to take in a filter. It was easier enough to update on the front end but I'm unsure how testible this is, having not used razor pages before.  

My concern with this and the changes from task 2 is that without the unit tests they could regress to how it previously was and it wouldn't be picked up very quickly.

# Task 6

For this I added a new test then updated the TestTodoListBuilder to be able to take a userId to set the responsibleUserId on the list item, this was nullable so existing tests would function without changes.  

From here I then ran the test so it failed and then updated the code so that it also checked for lists that had items with the passed in userId.
