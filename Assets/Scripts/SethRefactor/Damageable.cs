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
        public float MaxHealth => health.MaxValue;
        public float HealthRemaining => health.Remaining;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (health.Remaining == 0f)
            {
                Destroy(gameObject);
            }
        }

        public void TakeDamage(float amount)
        {
            health.Reduce(amount);
        }
    }
}