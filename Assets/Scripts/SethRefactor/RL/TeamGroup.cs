using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

namespace ScavengerWorld.AI
{
    public class TeamGroup : MonoBehaviour
    {
        [SerializeField] private Unit storage;
        [SerializeField] private List<ActorAgent> actorAgents;

        public int FoodStored => storage.InventoryItemCount;

        private SimpleMultiAgentGroup group = new();

        void Start()
        {
            foreach (var agent in actorAgents)
            {
                group.RegisterAgent(agent);
                agent.Unit.StorageDepot = storage;
            }
        }

        public void EndEpisode(float reward)
        {
            group.SetGroupReward(reward);
            group.EndGroupEpisode();
        }

        public void ResetTeam()
        {
            // TODO: Randomly place units around the center transform
            foreach (var agent in actorAgents)
            {
                agent.transform.position = storage.transform.position;
            }
        }

        public bool IsAlive()
        {
            //TODO: check all agents are not dead
            return true;
        }
    }

}