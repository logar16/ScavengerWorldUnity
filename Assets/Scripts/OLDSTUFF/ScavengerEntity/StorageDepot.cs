using System.Collections.Generic;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public class StorageDepot : Entity, IHolder
    {
        private readonly List<Food> Supply = new List<Food>();

        [Tooltip("Maximum amount of food to hold.")]
        [Range(1, 10_000)]
        public int Limit = 10_000;  // Can be used for win condition
        
        public int Count { get => Supply.Count; }
        public bool Full { get => Count >= Limit; }
        
        public StandardTeam Team { get; internal set; }

        public override EntitySummary Summarize()
        {
            var summary = base.Summarize();
            summary.Custom1 = Full ? 1 : 0;
            return summary;
        }

        override public void Reset()
        {
            foreach (var food in Supply)
            {
                Drop(food);
            }
            Supply.Clear();
            base.Reset();
        }

        public T Drop<T>() where T : Item
        {
            if (typeof(T) == typeof(Food))
            {
                var food = Pop(Supply);
                Drop(food);
                return food as T;
            }
            else
                return null;
        }

        private void Drop(Food food)
        {
            food.gameObject.SetActive(true);
            food.transform.position = transform.position + Vector3.forward;
        }

        public bool Take(Item item)
        {
            if (item is Food food && !Full && food.PickUpWith(this))
            {
                Supply.Add(food);
                //TODO: Make some visual change to indicate food added
                //  Could be a text box or progress bar or objects being added
                food.gameObject.SetActive(false);
                return true;
            }
            return false;
        }

        public bool Transfer<T>(IHolder other) where T : Item
        {
            var complete = other?.Take(Drop<T>());
            return complete ?? false;
        }
    }
}
