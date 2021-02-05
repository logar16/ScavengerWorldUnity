using System.Collections.Generic;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public abstract class Entity : MonoBehaviour
    {
        public float StartingHealth = 10;
        [HideInInspector]
        public float Health { get; private set; }

        [HideInInspector]
        public bool IsAlive { get => Health > 0; }

        virtual public void Reset()
        {
            Health = StartingHealth;
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (!IsAlive)
                Die();
        }

        /// <summary>
        /// Function that executes whent the entity has been killed/destroyed
        /// </summary>
        private void Die()
        {
            //TODO: Does this entity need to be destroyed or fire off any events?
            gameObject.SetActive(false);
        }

        protected T Pop<T>(List<T> list, int index = -1)
        {
            if (list.Count == 0)
                return default(T);

            if (index < 0)
                index += list.Count;

            index %= list.Count;
            var element = list[index];
            list.RemoveAt(index);
            return element;
        }
    }

}
