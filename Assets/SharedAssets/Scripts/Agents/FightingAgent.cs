using Assets.SharedAssets.Scripts.ScavengerEntity;
using System;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.Agents
{
    public class FightingAgent : UnitAgent
    {
        [Tooltip("Penalty for dying")]
        public float DeathPenalty;
        [Tooltip("Penalty for taking damage")]
        public float DamagePenalty;

        [Header("Enemy Destruction Rewards")]
        [Tooltip("Reward for destroying an enemy unit.")]
        public float UnitReward;
        [Tooltip("Reward for destroying an enemy marker.")]
        public float ItemReward;
        [Tooltip("Reward for destroying an enemy storage depot.")]
        public float StorageReward;

        [Header("Self Destruction Penalties (should be negative)")]
        [Tooltip("Reward for destroying an allied unit.")]
        public float UnitPenalty;
        [Tooltip("Reward for destroying an ally marker.")]
        public float ItemPenalty;
        [Tooltip("Reward for destroying the team's storage depot.")]
        public float StoragePenalty;

        private bool Died;
        private float Health;

        public override void Initialize()
        {
            base.Initialize();
            Unit.OnDeath += OnUnitDeath;
        }

        private void OnUnitDeath(Entity sender)
        {
            Died = true;
            AddReward("death_penalty", DeathPenalty);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            base.OnActionReceived(actions);

            if (!Died && Unit.Health < Health)
            {
                Health = Unit.Health;
                AddReward("damage_penalty", DamagePenalty);
            }
        }

        public override void OnEpisodeBegin()
        {
            base.OnEpisodeBegin();
            Died = false;
            Health = Unit.MaxHealth;
        }

        protected override void Attack()
        {
            var destroyed = Unit.Attack();
            switch (destroyed)
            {
                case Unit unit:
                    if (SameTeam(unit))
                        AddReward("friendly_fire_unit", UnitPenalty);
                    else
                        AddReward("destroy_unit_reward", UnitReward);
                    break;
                case Item item:
                    if (SameTeam(item.Creator))
                        AddReward("friendly_fire_item", ItemPenalty);
                    else
                        AddReward("destroy_enemy_item", ItemReward);
                    break;
                case StorageDepot storage:
                    if (SameTeam(storage.Team.Id))
                        AddReward("friendly_fire_storage", StoragePenalty);
                    else
                        AddReward("destroy_enemy_storage", StorageReward);
                    break;
            }
        }
    }
}
