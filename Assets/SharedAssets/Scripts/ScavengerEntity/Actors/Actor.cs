using System.Collections.Generic;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public class Actor : Entity
    {
        [HideInInspector]
        public Entity Target { get; private set; }
        [HideInInspector]
        public bool HasTarget { get => Target != null; }

        [Range(.1f, 3f)]
        public float FocusRange = 2;

        [Tooltip("Tags for objects that can be targeted")]
        public List<string> DetectableTags = new List<string> { "entity" };

        private HashSet<string> Targetable;

        [Tooltip("Attacks per second")]
        public float AttackRate = 1;
        [Tooltip("Amount of damage each attack inflicts (HP drop)")]
        public float AttackDamage = 1;

        private void Start()
        {
            Targetable = new HashSet<string>(DetectableTags);
        }

        /// <summary>
        /// Sends out a Raycast to check for any targetable item directly in front of the actor.
        /// Modify which items are targetable by adjusting the list of DetectableTags
        /// </summary>
        /// <returns><see langword="true"/> if a target was found (and added)</returns>
        public bool CheckForTarget()
        {
            //position + transform.forward * 0.5f (if agent's body gets in the way)
            var ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, FocusRange))
            {
                var target = hit.collider.gameObject;
                if (Targetable.Contains(target.tag))
                {
                    Target = target.GetComponent<Entity>();
                    return true;
                }
            }
            Target = null;
            return false;
        }

        /// <summary>
        /// Attack the current target
        /// </summary>
        /// <returns>Destroyed <see cref="Entity"/> if attack reduced health of target below 0</returns>
        public Entity Attack()
        {
            if (!HasTarget)
                return null;

            Target.Health -= AttackDamage;
            if (Target.IsAlive)
                return null;

            var target = Target;
            Target = null;
            return target;
        }
    }
}
