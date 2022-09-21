using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI;
using Unity.MLAgents.Actuators;
using System;

namespace ScavengerWorld
{
    /// <summary>
    /// Responsible for holding gameplay data about unit and 
    /// handling unit-specific changes.
    /// (e.g. taking damage, inventory changes, stats)
    /// </summary>
    [RequireComponent(typeof(Damageable))]
    [RequireComponent(typeof(Interactable))]
    public class Unit : MonoBehaviour
    {
        [SerializeField] private int teamId;
        [SerializeField] private Interactable storageDepot;
        [SerializeField] private Inventory inventory;
        [SerializeField] private Stats stats;

        private EntitySummary Summary;
        private MeshRenderer meshRenderer;
        private Interactable interactable;
        private Damageable damageable;
        private Mover mover;
        private ActionRunner actionRunner;

        public Stats Stats => stats;
        public Interactable StorageDepot => storageDepot;
        public Interactable Interactable => interactable;
        public Damageable Damageable => damageable;
        public Mover Mover => mover;
        public ActionRunner ActionRunner => actionRunner;

        public int TeamId => teamId;
        public float HowFullIsInventory => inventory.HowFull();
        public bool IsStorageDepot => inventory.IsStorageDepot;

        public float StepReward { get; private set; }

        void Awake()
        {
            damageable = GetComponent<Damageable>();
            interactable = GetComponent<Interactable>();
            mover = GetComponent<Mover>();
            actionRunner = GetComponent<ActionRunner>();
            meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        // Start is called before the first frame update
        void Start()
        {            
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Attack(Damageable enemy)
        {
            // play little animation
            enemy.TakeDamage(stats.attackDamage);
        }
        
        public void AddItem(Gatherable gatherable)
        {
            if (inventory.HowFull() == 1f) return;

            if (inventory.WillBeOverfilled(stats.gatherRate)) return;

            if (gatherable.AmountAvailable > stats.gatherRate)
            {
                inventory.Add(gatherable.Take(stats.gatherRate));
                return;
            }

            inventory.Add(gatherable.TakeAll());
        }

        public void AddItem(int amount)
        {
            if (inventory.HowFull() == 1f) return;

            if (inventory.WillBeOverfilled(amount)) return;

            inventory.Add(amount);
        }

        public void RemoveItem(int amount)
        {
            inventory.Remove(amount);
        }

        public int RemoveAllItems()
        {
            return inventory.RemoveAll();
        }

        /// <summary>
        /// Summarize the top visual features in a simple to interpret way.
        /// Convert to a float vector using <see cref="EntitySummary.ToArray"/>
        /// </summary>
        /// <returns></returns>
        public virtual EntitySummary Summarize()
        {
            if (Summary)
                return Summary;

            var size = meshRenderer.bounds.size;
            // This function (renderer.material) automatically instantiates the materials and makes them unique to this renderer. 
            //      It is your responsibility to destroy the materials when the game object is being destroyed.
            var color = meshRenderer.material.color;

            Summary = new EntitySummary
            {
                Size = size,
                Color = color,
                Health = damageable.CurrentHealth,
                Position = transform.localPosition
            };
            return Summary;
        }

        internal void SetReward(float reward)
        {
            StepReward = reward;
        }
    }
}