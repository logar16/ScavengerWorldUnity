using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScavengerWorld.AI;
using Unity.MLAgents.Actuators;

namespace ScavengerWorld
{
    /// <summary>
    /// Responsible for holding gameplay data about unit and 
    /// handling unit-specific changes.
    /// (e.g. taking damage, inventory changes, stats)
    /// </summary>
    public class Unit : MonoBehaviour, IInteractable
    {
        [SerializeField] private int teamId;
        [SerializeField] private Attribute health;
        [SerializeField] private Inventory inventory;
        [SerializeField] private List<Action> actionsAvailable;

        private EntitySummary Summary;
        private MeshRenderer meshRenderer;

        public Transform Transform => transform;

        public int TeamId => teamId;
        public float HealthRemaining => health.Remaining;
        public float HowFullIsInventory => inventory.HowFull();
        public bool IsStorageDepot => inventory.IsStorageDepot;

        void Awake()
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();

            health.SetCurrentValue(health.MaxValue);
        }

        // Start is called before the first frame update
        void Start()
        {            
            InitActions();
            PreCheck();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InitActions()
        {
            foreach (Action a in actionsAvailable)
            {
                a.Target = this;
            }
        }

        public void AddItem(int amount)
        {
            inventory.Add(amount);
        }

        public void RemoveItem(int amount)
        {
            inventory.Remove(amount);
        }

        public void RemoveAllItems()
        {
            inventory.RemoveAll();
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
                Health = health.CurrentValue,
                Position = transform.localPosition
            };
            return Summary;
        }

        // For testing only
        private void PreCheck()
        {
            Debug.Log($"Unit: {gameObject.name} " +
                $"| Health: {health.CurrentValue}/{health.MaxValue} " +
                $"| Inventory: {inventory.HowFull()} " +
                $"| Actions available: {actionsAvailable.Count}");
        }
    }
}