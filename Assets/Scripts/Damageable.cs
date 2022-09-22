using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [RequireComponent(typeof(Interactable))]
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private Attribute health;

        private Interactable interactable;

        public Interactable Interactable => interactable;
        public float CurrentHealth => health.CurrentValue;
        public float HealthPercentage => health.Percentage;
        public bool IsAlive { get; private set; }

        private void Awake()
        {
            health.SetCurrentValue(health.MaxValue);
            IsAlive = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsAlive)
            {
                gameObject.SetActive(false);
            }
            else if (health.CurrentValue == 0f)
            {
                IsAlive = false;
            }
        }

        public void TakeDamage(float amount)
        {
            health.Reduce(amount);
        }
    }
}