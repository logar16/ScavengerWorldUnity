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
        [SerializeField] private float maxHealth;        
        [SerializeField] private List<Action> actionsAvailable;

        private float health;
        private EntitySummary Summary;
        private MeshRenderer renderer;

        public Transform Transform => transform;

        public float HealthRemaining => health/maxHealth;

        void Awake()
        {
            renderer = GetComponentInChildren<MeshRenderer>();
        }

        // Start is called before the first frame update
        void Start()
        {            
            InitActions();
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

        /// <summary>
        /// Summarize the top visual features in a simple to interpret way.
        /// Convert to a float vector using <see cref="EntitySummary.ToArray"/>
        /// </summary>
        /// <returns></returns>
        public virtual EntitySummary Summarize()
        {
            if (Summary)
                return Summary;

            var size = renderer.bounds.size;
            // This function (renderer.material) automatically instantiates the materials and makes them unique to this renderer. 
            //      It is your responsibility to destroy the materials when the game object is being destroyed.
            var color = renderer.material.color;

            Summary = new EntitySummary
            {
                Size = size,
                Color = color,
                Health = health,
                Position = transform.localPosition
            };
            return Summary;
        }
    }
}