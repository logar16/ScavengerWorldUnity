﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public class Actor : Entity
    {
        [HideInInspector]
        public Entity Target { get; protected set; }
        [HideInInspector]
        public bool HasTarget { get => Target != null; }

        [Range(.1f, 3f)]
        public float FocusRange = 1.5f;

        [Tooltip("Tags for objects that can be targeted. " +
            "If zero entries, the actor can target any ScavengerEntity")]
        public List<string> DetectableTags;

        private HashSet<string> Targetable;
        private bool CheckTag;

        [Range(0.1f, 5f)]
        [Tooltip("Attacks per second")]
        public float AttackRate = 1;
        //TODO: Add visual attack indicator
        [Range(0.1f, 5f)]
        [Tooltip("Amount of damage each attack inflicts (HP drop)")]
        public float AttackDamage = 1;

        public delegate void AttackEvent(Entity target);
        public event AttackEvent OnDestroyedTarget;

        private void Start()
        {
            Targetable = new HashSet<string>(DetectableTags);
            CheckTag = Targetable.Count > 0;
        }

        override public void Reset()
        {
            Target = null;
            base.Reset();
        }

        /// <summary>
        /// Sends out a Raycast to check for any targetable item directly in front of the actor.
        /// Modify which items are targetable by adjusting the list of DetectableTags
        /// </summary>
        /// <returns><see langword="true"/> if a target was found (and added)</returns>
        public bool CheckForTarget()
        {
            //Debug.DrawRay(transform.position, transform.forward * FocusRange, Color.green);
            //position + transform.forward * 0.5f (if agent's body gets in the way)
            var ray = new Ray(transform.position, transform.forward);
            if (Physics.SphereCast(ray, 0.75f, out RaycastHit hit, FocusRange))
            {
                var target = hit.collider.gameObject;
                if (!CheckTag || Targetable.Contains(target.tag))
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
        /// <returns><see cref="Entity"/> that was attacked if attack was successful (<see langword="null"/> otherwise). 
        /// The <see cref="OnDestroyedTarget"/> event is dispatched if the attack destroyed 
        /// the target <see cref="Entity"/>.</returns>
        public Entity Attack()
        {
            //TODO: Limit number of attacks by AttackRate
            if (!HasTarget)
                return null;

            Target.TakeDamage(AttackDamage);

            var target = Target;
            if (!Target.IsAlive)
            {
                //It must have died from the attack
                OnDestroyedTarget?.Invoke(Target);
                Target = null;
            }

            return target;
        }
    }
}
