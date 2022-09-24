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

## Training Setup

Recommend to follow the [official installation guide](https://github.com/Unity-Technologies/ml-agents/blob/release_19_docs/docs/Installation.md) to get setup, but here's the TLDR:

1. Open `ScavengerWorldUnity` project in Unity and in the editor select `Assets -> Scenes -> Training -> GathererTraining`
1. Open up a terminal of your choice (and preferably a python virtual environment)
1. `pip3 install torch~=1.7.1 -f https://download.pytorch.org/whl/torch_stable.html`
1. `python -m pip install mlagents==0.28.0`
1. cd to where you had `ScavengerWorldUnity` stored on disk
1. To start training, run `mlagents-learn.exe configs\TrainingZones\FoodCollectionPoca.yml --run-id=FoodGatherExp1`
1. Go back to the Unity Editor and hit the `play` button in the middle of the top screen.
1. To monitor training metrics, on another terminal window and run `tensorboard --logdir results`
1. Open the address `localhost:6006` in your browser to see the tensorboard dashboard.

## Scaled Training Locally

To enhance training speed, we can fit more simulations running concurrently in our local machine with rendering disabled.

1. Build the simulation as a independent game `.exe`
   1. Open the training scene in the editor
   1. Click on `File -> Build Setting -> Add Open Scenes -> Select Windows, Mac, Linux as the Platform -> Build`
   1. Select the location to stored the game .exe
1. Bring up a terminal and enable the virtual environment where you have `mlagent-learn` installed
   1. Run `mlagents-learn.exe configs\TrainingZones\FoodCollectionPoca.yml --run-id=FoodGatherExpPoca --no-graphics --env=<GAME_EXE_PATH> --num-envs=<n>`
      - `--no-graphics`: turns off graphic rendering
      - `--env`: is the location where you saved the game .exe in the previous step
      - `--num-envs`: is the number of concurrent game simulation to run simultaneously


## TODOs

* Improve dynamic object creation in editor
* Enforce more rules and add options to tune (or separate classes)
* Build more training zones
* Build the full game
* Allow team-level control as well as per-unit control