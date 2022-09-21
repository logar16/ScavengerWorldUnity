using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScavengerWorld.AI;
using Unity.MLAgents;
using UnityEngine;

namespace ScavengerWorld
{
    // Arena reset = all food collected, one team remaining

    public class ArenaManager : MonoBehaviour
    {
        private FoodSpawner foodSpawner;
        [SerializeField] private List<TeamGroup> teams;

        [Range(50, 2000)]
        [SerializeField]  private int maxStep = 1000;
        private int currentStep;

        private void Awake()
        {
            foodSpawner = GetComponent<FoodSpawner>();
            Academy.Instance.OnEnvironmentReset += ResetArena;
            Academy.Instance.AgentPreStep += AgentStep;
        }        

        // Start is called before the first frame update
        void Start()
        {
            foodSpawner.CreateFood();
            ResetArena();
        }

        void AgentStep(int step)
        {
            currentStep++;

            var shouldEnd = currentStep >= maxStep;
            //TODO: Check if only one still alive
            //TODO: Check if food is all gathered
            if (shouldEnd)
            {
                EndEpisode();
            }
            print("Arena Step");
        }

        private void EndEpisode()
        {
            //Orders low to high
            var winner = teams.OrderBy(t => t.FoodStored).Last();
            foreach (var team in teams)
            {
                var reward = (team == winner) ? 1 : -1;
                team.EndEpisode(reward);
            }
            ResetArena();
        }

        private void ResetArena()
        {
            print("Resetting");
            currentStep = 0;
            foreach (var team in teams)
            {
                team.ResetTeam();
            }
            foodSpawner.ResetFood();
        }
    }
}