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

## Task 6

For this I added a new test then updated the TestTodoListBuilder to be able to take a userId to set the responsibleUserId on the list item, this was nullable so existing tests would function without changes.  

From here I then ran the test so it failed and then updated the code so that it also checked for lists that had items with the passed in userId.

## Task 7
My first thought from reading this is that is the rank unique or can two items share the same rank, since no constraint is defined I'll carry on with the assumption two items can have the same rank, this is just for ease and not to overcomplicate things for now.  

Being completely honest this was tricky due to not knowing razor so there was a lot of googling but in the end I had the razor page pass a param to the controller and do the filtering on there. Typically I wouldn't do any logic in a controller and I'd instead call a service with the logic in, better i'd pass the filter to the service and the service would do the correct db call.

Typically I would add tests to cover the logic in the controller but I don't have the time at the moment.

## Task 8
First I looked into the gravatar api docs found here: https://docs.gravatar.com/api/profiles/rest-api/  
They recommended to use an API key which I looked at creating but I had to provide an organisation name and website which I was unsure what to use so I haven't used an API Key.  
This comes with the caveat of only being able to use 100 requests per hour, when doing this in an organisation I would get an api key

My approach for this was to first get the request working in postman, this way I could get it working easily and look at the model. From here I created a model, new configuration/options and a new service for the gravatar logic that was non static.  

Once this was working I plugged it into the front end and it worked. Again typically I would test this but it would be extra time I don't want to spend.  

I put in a check to handle the 429 response code so that if we do hit the rate limit it doesn't throw errors and fails gracefully, the code is async so shouldn't lock the front end up. Although I'm unsure how this works in Razor but that's why you'd use it in MVVM

## Task 9
This was tricky due to a lack of razor knowledge, I tried lifting the logic from the TodoItem Create.cshtml and putting it in the Detail.cshtml, then only displaying it when the button was clicked but it didn't look great and the code was messy.  
After some googling I decided to try a modal which was tricky but seemed to work with random fields, I then tried plugging in the actual variables and it seemed to get passed to the controller. I'm unsure if what I've done is bad practice or not.  

I also spotted that the create never allowed you to set the rank, and that the input allowed for negative numbers so I tidied that up.

## Task 10
This one I find a bit unclear, while I understand it wants you to set the rank it doesn't say where it should be set, so I assume it means on the creation of the item, which I've already done.   

The way I've currently implemented the rank ordering it does filter the page, so I needed to address this but from looking into it would require a fair bit of work so I am not going to complete this.

# Future improvements

Below is a list of things I'd like to sort/improve with the task if I had more time:
-	Create an api key for gravatar service so we're less likely to hit the request limit
-	Add more unit tests for all the service but primarily for the ones I've created, currently test coverage is at a whopping 10%. Coverage can be false confidence but just having the regression suite there would be useful
-   Personally I dislike any logic within the front, unless it's simple logic like hiding something based on a bool, so I'd like to change any ordering/filtering done in the front end back into the back end
- 