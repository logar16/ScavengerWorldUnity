using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [System.Serializable]
    public class Attribute
    {
        [SerializeField] private float maxValue;
        private float currentValue;

        public float CurrentValue => currentValue;
        public float MaxValue => maxValue;
        public float Remaining => currentValue / maxValue;

        public Attribute(float currentValue, float maxValue)
        {
            this.currentValue = currentValue;
            this.maxValue = maxValue;
        }

        public void SetCurrentValue(float amount)
        {
            currentValue = Mathf.Clamp(amount, 0, maxValue);
        }

        public void Reduce(float amount)
        {
            currentValue = Mathf.Clamp(currentValue - amount, 0, maxValue);
        }

        public void Add(float amount)
        {
            currentValue = Mathf.Clamp(currentValue - amount, 0, maxValue);
        }
    }
}