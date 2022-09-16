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
        public float DamageTakenPenalty;

        //TODO: Consider having one variable each that is positive if enemy or negative if not, 
        //  instead of reward and penalty variables
        [Header("Enemy Destruction Rewards")]

        [Tooltip("Reward for giving damage to anything of the enemy")]
        public float DamageGivenReward;
        [Tooltip("Reward for destroying an enemy unit.")]
        public float UnitReward;
        [Tooltip("Reward for destroying an enemy marker.")]
        public float ItemReward;
        [Tooltip("Reward for destroying an enemy storage depot.")]
        public float StorageReward;

        [Header("Self Destruction Penalties (should be negative)")]
        [Tooltip("Penalty for giving damage to anything allied")]
        public float DamageGivenPenalty;
        [Tooltip("Reward for destroying an allied unit.")]
        public float UnitPenalty;
        [Tooltip("Reward for destroying an ally marker.")]
        public float ItemPenalty;
        [Tooltip("Reward for destroying the team's storage depot.")]
        public float StoragePenalty;

        public override void Initialize()
        {
            base.Initialize();
            Unit.OnDeath += OnUnitDeath;
            Unit.OnDamageTaken += OnUnitDamaged;
            Unit.OnDestroyedTarget += OnDestructiveAttack;
        }

        private void OnUnitDamaged(float damage)
        {
            AddReward("damage_penalty", DamageTakenPenalty * damage);
        }

        private void OnDestructiveAttack(Entity target)
        {
            var ally = SameTeam(target);

            switch (target)
            {
                case Unit _:
                    if (ally)
                        AddReward("friendly_fire_unit", UnitPenalty);
                    else
                        AddReward("destroy_unit_reward", UnitReward);
                    break;

                case Item _:
                    if (ally)
                        AddReward("friendly_fire_item", ItemPenalty);
                    else
                        AddReward("destroy_enemy_item", ItemReward);
                    break;

                case StorageDepot _:
                    if (ally)
                        AddReward("friendly_fire_storage", StoragePenalty);
                    else
                        AddReward("destroy_enemy_storage", StorageReward);
                    break;
            }
        }

        private void OnUnitDeath(Entity sender)
        {
            AddReward("death_penalty", DeathPenalty);
        }

        protected override void Attack()
        {
            var target = Unit.Attack();
            if (!target) 
                return;

            if (SameTeam(target))
                AddReward("damaged_ally_penalty", DamageGivenPenalty);
            else
                AddReward("damaged_enemy_reward", DamageGivenReward);
        }
    }
}
