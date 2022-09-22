using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using UnityEngine;

namespace ScavengerWorld.AI
{
    public class TeamGroup : MonoBehaviour
    {
        [SerializeField] private Unit storage;
        [SerializeField] private List<ActorAgent> actorAgents;

        public int FoodStored => storage.InventoryItemCount;
        public int TeamId { get; set; }

        private SimpleMultiAgentGroup group = new();

        void Awake()
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

        public void ResetTeam(int teamId)
        {
            // TODO: Randomly place units around the center transform
            foreach (var agent in actorAgents)
            {
                agent.transform.position = storage.transform.position;
                agent.Unit.TeamId = teamId;
                agent.gameObject.SetActive(true);
            }
            storage.TeamId = teamId;
            storage.RemoveAllItems();
        }

        public bool IsAlive()
        {
            return actorAgents.Any(a => a.gameObject.activeInHierarchy);
        }
    }

}