# Agent Design

#### Goal: Make smart agents that can work effectively as a team
#### Alternative Goal: Make an efficient agent that can control a team of agents

## Hierarchical Agents
I think that hierarchical agents has a lot of promise.  Instead of trying to throw a massive deep neural network at the problem and hope the gradients flow, etc. we instead break down the actions into smaller chunks and have hierarchical groups of networks working together to simplify taking actions.
Instead of retraining each agent how to move around, we have a motion-control model which is utilized by the parent model with simple interactions.  The parent is in charge of high-level decisions instead of trying to decide which specific combination of actions it must take, it decides what state it wants to be in next and it asks its subordinate networks to make that a reality.
If I recall correctly, at each layer of the networks, the master network will give rewards to subordinate networks based on how well they accomplished the request, not whether or not the master itself was rewarded for that decision.
If the agent is able to learn how to predict the next state, it can hopefully learn to choose which state looks best and then reverse-engineer the sort of action that would lead to that state.

## Transferrable Learning
In order to save myself a lot of time, I want to create agents that don't have to be completely retrained because of some minor change in the environment.  Whether that is because I changed a variable or because they are moving from a training scenario to an evaluation scenario.

## Lifelong Learning
I want the agents to be able to learn throughout their lives, not just on the training environments.  The test sets should be pretty different from the training set in appearance, but not in basic game mechanics.  The agents should be able to handle that and learn how to maximize reward in the current environment.

## Genetic Evolution
This game naturally lends itself to a survival of the fittest situation which allows for natural selection and genetic evolution.  I'd like to do something like (probably not as well as) [Deepmind's FTW Capture-the-flag paper](deepmind.com/blog/article/capture-the-flag-science).

