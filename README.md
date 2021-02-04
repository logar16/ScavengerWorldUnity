# ScavengerWorldUnity
Unity project for my Scavenger World RL environment.  It uses ML-Agents to support communication between the environment and RL agents.

See more details in the Docs directory

## Game Mechanics
* There can be 2+ teams of 1+ organisms (referred to as "units").  
* Food will be randomly scattered throughout the world.
  * It could also be arranged in clumps  
* Each team has a home base that they store food within.  
* Time is given for the organisms to search for and gather food back to the base.  
* A signal will be given when winter is approaching and then soon thereafter, winter will arrive.  
* When winter comes, each team will have its stockpile evaluated and calculations will be made to decide how many of its members will survive.  
* Excesses of resources result in additional offspring for next year.  And then the cycle repeats.
