# Rules of the Game

## Endgame Conditions
Something along the lines of:

1. Winter comes (time-limit)
2. All food is gathered
3. All enemies destroyed

## Food Gathering
Units gather food and deposit in the team depot.  Units have a maximum capacity.  They may also have a gathering rate (perhaps implemented as a cool down between gatherings).

The purpose of gathering food is to add it to the team food storage.  To do so, the agent must move to the depot (bring it into focus) and deposit the food into the depot (using the "drop" action).

## Attacking Other Units
Units can attack each other (including teammates, so be careful).  If a unit's HP drops too low, it will die, dropping any food/items it carried.

If all enemies die, you win (probably--unless you still need to survive the winter).


## Items
Later versions may allow for picking up weapons or other items and using them to improve attack or rate of gathering food.

For now this introduces too much complexity so it will be reserved for a later time.