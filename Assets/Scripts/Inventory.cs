using UnityEngine;

namespace ScavengerWorld
{
    [System.Serializable]
    public class Inventory
    {        
        [SerializeField] private int maxCapacity;
        [SerializeField] private bool isStorageDepot;
        private int itemCount;

        public int ItemCount => itemCount;
        public int MaxCapacity => maxCapacity;
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

        public int Add(int amount)
        {
            int finalCount = itemCount + amount;
            if (finalCount > maxCapacity)
            {
                itemCount = maxCapacity;
                return finalCount - maxCapacity;
            }

            itemCount += amount;
            return 0;
        }

        public int Remove(int amount)
        {
            int amountToGive = 0;
            if (amount < itemCount)
            {
                amountToGive = amount;
                itemCount -= amount;
                return amountToGive;
            }

            amountToGive = itemCount;
            itemCount = 0;
            return amountToGive;
        }

        public int RemoveAll()
        {
            int itemAmountToGive = itemCount;
            itemCount = 0;
            return itemAmountToGive;
        }

        public bool WillBeOverfilled(int amount)
        {
            return itemCount + amount > maxCapacity;
        }
    }
}