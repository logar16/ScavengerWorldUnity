using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace ScavengerWorld
{
    [System.Serializable]
    public class Inventory
    {        
        [SerializeField] private int maxCapacity;
        [SerializeField] private bool isStorageDepot;
        private int itemCount;

        public bool IsStorageDepot => isStorageDepot;

        public Inventory(int maxCapacity, bool isStorageDepot)
        {
            this.maxCapacity = maxCapacity;
            this.isStorageDepot = isStorageDepot;
            itemCount = 0;
        }

        public void SetItemAmount(int amount)
        {
            itemCount = Mathf.Clamp(amount, 0, maxCapacity);
        }

        public float HowFull() => (float)itemCount / maxCapacity;

        public void Add(int amount)
        {
            itemCount = Mathf.Clamp(itemCount + amount, 0, maxCapacity);
        }

        public void Remove(int amount)
        {
            itemCount = Mathf.Clamp(itemCount - amount, 0, maxCapacity);
        }

        public void RemoveAll()
        {
            itemCount = 0;
        }
    }
}