# Ideas

## Observations

### Focus
Many actions require brining something into focus.  This could work two ways:
1. Only the thing that is most central is in focus.  This is similar to using a reticule to select items in a FPV sort of game.
   * For instance, there is food and an enemy both right in front of an agent, the agent will bring the food into focus by turning slightly toward the food or the enemy by turning slightly toward it.  It can then decide the appropriate action to take given the focused object.
   * Implementation would probably be sending out a raycast straight forward and seeing what object lies dead ahead 
2. Items that are within a certain distance ahead are all considered in focus and the agent must pick which type it wants to act on.
   * For instance, there is food and an enemy both right in front of an agent, it can detect both, but it decides it wants to attack the enemy instead of pick up food.  It must select the *attack* action and the *enemy* as target (using an enumerated tag or the like)
   * Implementation is probably using colliders to trigger on entry to add an object to the list of focusable objects (within range) and on exit it is removed from the list of options.


### Teams
How will agents determine team ID of other units, food depots, and markers?
It will quickly become unmanageable to make a specific tag for each object and its team (like in SoccerTwos example env.)

Possible Solutions:
* Dynamic tags added that can be queried to determine team ID.  Doesn't work well for the Raycast which would need the tags beforehand
  * Perhaps there is a way to take the raycast data and modify it such as by moving all identified *agent* values to *enemy* values if the raycast fell on an enemy.  This seems quite hacky though.
* Have a collision sphere/cylinder which triggers a method which can then decide what sort of observation to make based on team ID



## Actions
As of right now, all units can do all actions but to varying degrees.  Perhaps some actions can be enabled or disabled for certain types of units?

### Pick Up
In order to pick up something, the agent must move close enough to an object to have it in focus. It can then execute the "pick up" command to pick up the object of focus.  If the object of focus is not something which can be picked up (e.g. other units) then nothing happens.

For now, this action should just move an item to be [attached to the unit](https://docs.unity3d.com/ScriptReference/Transform.SetParent.html) who picked it up.  It also needs to disable any other unit from trying to take possession of it.  Perhaps the easiest way to do this is to disable the GameObject (so it is no longer in the environment and we avoid duplicate pick ups) and we add a pack or something to the unit which we can dynamically expand as it has more food or what not and diminishes as that food is dropped.

### Drop
In order to deliver food to food storage depot or to leave markers, the unit will need to use a "drop" action.  The action will take a value indicating what type of item to drop (either food or marker or current item if those are a thing later).
For now, if the unit is within a close distance to the food depot, then a drop will add food to the depot.  Later, this ability may be moved to "give" action if there is reason to add it.

### Give?
In order to drop off food to the team depot, the agent will need to "give" the food to the depot.  In other cases, units may wish to give food to their allies or to give negative things (poisoned food or a bomb) to enemies.

### Attack
All objects have some HP which can be reduced by attacking the object.  To attack, the agent must approach an item until it comes into focus, and then it can execute the "attack" command.  At this point it will attack the focused object (whether friend, foe, food, or otherwise).  The attack will drop the HP of the object respective to the strength of the attacker.  If the HP drops below 0, then the attacked object is destroyed (and drops all things it was carrying if it did carry anything).


### Create
Units can create markers, and later maybe other items.  See [Marker](#markers) section for more details.


## Units

Each unit will have a cost and each team gets a budget to attempt to keep things balanced.

### Scout
The Searcher has expanded range for identifying food items and other units (including friend or foe status). Fastest unit.

### Gatherer
The Gatherer is able to carry additional food items, and can gather food faster.

### Warrior
The Warrior is designed for attacking other units.  It has greater health and attack.

### Thief/Spy
The Thief is especially good at pilfering from enemy storehouses, and is harder for enemies to identify as non-team.

### Leader (Optional)
The Leader inspires nearby units to be more effective, especially in their other abilities.  The Leader also has the ability to convert units of other teams to its team.

## Monsters
I think there should be a neutral party that is just there to make things harder, cue Monsters.  They are rewarded every time they eat a unit.  They will have high health so they can battle several units 1:1 and win.  It will take coordination to eliminate them.  There could even be a game mode where the only task is to kill the most monsters or to coordinate with other teams to take out the monsters.

Maybe units should give off indicators of how much food they have and this can be used to attract attention from monsters.

## Markers
These temporary objects can be placed by and identified by units.  Their sphere of influence will decay over time until they are eventually rendered ineffective and removed.  Agents will require a cool down between placing these so they can't spam the action.  Markers will first need to be created and then they can be dropped.

Markers may be considered "items" but they are more like a chemical or visual signal (e.g. cairn) that the unit leaves behind--not so much an actual object. Even so, they can be destroyed or moved, so they could be treated as an object.

If they can be moved, then a unit can handle multiple markers?  Either it can hold one and must drop it before picking up another (makes drop action easier), or else it will add them to a stack or queue and they will be dropped in the order they were added (stack makes sense if you want to pick one up and then drop it again even if you have others you've created).

Should there be other types of communication such as sound or the unit changing its appearance?


## Items
Later versions may allow for picking up weapons or other items and using them to improve attack or rate of gathering food.

This can easily take off and become really complex (think weapons, and a forge to make weapons faster, and tools to make forges faster, and tools to mine fuel for the forge etc. etc.).  Let's not remake Minecraft for the purposes of this project.


To begin, there should only be two real items: weapon and tool (for gathering).

Other objects that could be considered items would be 
* Markers - which could be moved around to frustrate enemies or inform teammates


## Game Setup
Pick number of "years" to play the game, as in how many cycles of gathering and evaluating outcomes.
If you want to be realistic, modify team counts in relation to performance and let them try again.

(Optional) Each team can pick their stats of what types of units it wants this time around, such as 30% gathers and 70% warriors 0% all others.
This is especially important if gameplay lasts for several "years" and agents can adapt to other players' strategies.

### Inter-team Cooperation?
Is there a way to allow for inter-team cooperation, not just intra-team? It would be interesting to add politics to the game and forcing teams to gang up against rival teams for a season.

## Units vs Teams
Can units defect from their team if they feel they are going to lose?  This is only possible if units are controlled completely separate from the overall team.
Also, an easy way to win is to have every unit of the team defect at once and they are suddenly all on the same team


## Test Environments

### Food Collection
* Solo - Just have one agent learning that it can pick up food and deliver it to the storage depot
* Single Team - Several agents on one team learning to help each other gather food quicker
  * Should there be an additional reward for cooperation?
  * Should there be a penalty for killing your coworkers? (other than the obvious guilt that would rack the agent)
* Multi Team - Single agent for each team and several teams
  * Could also do several agents on each team
  * Will there be a zero-sum reward of most-gathered gets an end-prize?


## Unity and Code

### Custom Sensor
I think the `ISensor` makes sense for a compromise between Ray sensors and full-on Visual observations.  I envision using ray casts to identify objects in the visual field and then create a summary of each object and send that as observations.  For example, there is a piece of food and an ally agent and an enemy agent in view.  The observations produced would look like:
```json
//[normed-distance, width, height, depth, colorR, colorG, colorB, health, custom1, custom2]
[
  [0.30, .5, .5, .5, 0, 16, 0, 2, 1, 0],  //food (small and green, with other data indicating freshness or something)
  [0.80, 1, 1, 1, 0, 8, 12, 8, 3, 12], //ally (same color as agent and 8 health, 3 indicates the agent's class, and 12 food)
  [0.35, 1, 1, 1, 16, 16, 0, 10, 1, 0]  //enemy (all yellow so an enemy, with 10 health and 1 food)
]
```
To make this data useful, the agent will need to be given some static observations as well, such as its color, so it can compare with other agents.  Other information as needed could be shared, but it is best to keep static info limited.

### Actions
Could consider adding implementations of `IActuator` (see [example](https://github.com/Unity-Technologies/ml-agents/blob/release_7_docs/Project/Assets/ML-Agents/Examples/Basic/Scripts/BasicActuatorComponent.cs)) to make possible actions a little more clean.  I'm not sure if it supports dynamically resizing the possible actions the model can take? Would also need to consider if you have to hardcode that the gather action is index [2] and attack is index [3], then what happens if we remove the gather option for some agents?  Can we just add checkboxes next to each action and the actuator dynamically resizes possible actions and just loops over the action types and if enabled uses the next action input else checks next action type?
