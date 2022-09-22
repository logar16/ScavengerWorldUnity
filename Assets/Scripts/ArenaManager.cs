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
        [Range(50, 10000)]
        [SerializeField]  private int maxStep = 1000;
        [SerializeField] private Interactable moveHereIfNoActionMarker;
        [SerializeField] private List<TeamGroup> teams;

        private FoodSpawner foodSpawner;
        private int currentStep;
        private Queue<Interactable> moveMarkerPool = new();

        private void Awake()
        {
            foodSpawner = GetComponent<FoodSpawner>();          
            foodSpawner.CreateFood();
            ResetArena();
        }

        private void Start()
        {
            InitMoveMarkerPool(10);
        }

        private void OnEnable()
        {
            Academy.Instance.OnEnvironmentReset += ResetArena;
            Academy.Instance.AgentPreStep += AgentStep;
        }

        private void OnDisable()
        {
            Academy.Instance.OnEnvironmentReset -= ResetArena;
            Academy.Instance.AgentPreStep -= AgentStep;
        }

        private void OnApplicationQuit()
        {
            foodSpawner.DestroyGameObjects();
            moveMarkerPool.Clear();
        }

        void AgentStep(int step)
        {
            currentStep++;

            var shouldEnd = currentStep >= maxStep;
            var onlyOneLeft = teams.Where(t => t.IsAlive()).Count() == 1;
            shouldEnd = shouldEnd || onlyOneLeft;
            shouldEnd = shouldEnd || foodSpawner.AllFoodGathered();

            if (shouldEnd)
            {
                EndEpisode();
            }
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
            for (int i = 0; i < teams.Count; i++)
            { 
                teams[i].ResetTeam(i);
            }
            foodSpawner.ResetFood();
        }

        private void InitMoveMarkerPool(int poolSize)
        {
            for (int i = 0; i < poolSize; i++)
            {
                Interactable marker = Instantiate(moveHereIfNoActionMarker, Vector3.zero, Quaternion.identity);
                marker.gameObject.SetActive(false);
                moveMarkerPool.Enqueue(marker);
            }
        }

        public Interactable GetMoveMarker(Vector3 pos)
        {
            if (moveMarkerPool.Count > 0)
            {
                Interactable marker = moveMarkerPool.Dequeue();
                marker.transform.position = pos;
                marker.gameObject.SetActive(true);
                return marker;
            }
            else
            {
                return Instantiate(moveHereIfNoActionMarker, pos, Quaternion.identity);
            }
        }

        public void ReturnMoveMarker(Interactable moveMarker)
        {
            moveMarker.gameObject.SetActive(false);
            moveMarkerPool.Enqueue(moveMarker);
        }
    }
}