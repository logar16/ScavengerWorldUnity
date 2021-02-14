using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public class Unit : Actor, IHolder
    {
        [Range(0, 30)]
        [Tooltip("Number of food pieces the unit can carry at one time.")]
        public int FoodLimit = 10;

        [Range(0.1f, 5f)]
        [Tooltip("The number of food pieces that can be gathered per second")]
        public float GatherRate = 1;

        [Range(0, 10)]
        [Tooltip("Number of items the unit can carry at one time.")]
        public int ItemLimit = 1;

        [HideInInspector]
        protected readonly List<Item> Items = new List<Item>();
        [HideInInspector]
        protected readonly List<Food> FoodSupply = new List<Food>();

        public int FoodCount { get => FoodSupply.Count; }
        public int ItemCount { get => Items.Count; }

        [Range(1, 100)]
        public int Cost = 1;

        [HideInInspector]
        public BaseTeam Team { get; internal set; }

        override public void Reset()
        {
            Items.Clear();

            foreach (var food in FoodSupply)
            {
                RemoveFood(food);
            }
            FoodSupply.Clear();
            base.Reset();
        }

        public override EntitySummary Summarize()
        {
            var summary = base.Summarize();
            summary.Custom1 = FoodCount;
            return summary;
        }

        /// <summary>
        /// Instructs the unit to gather to current target
        /// </summary>
        /// <returns>The gathered <see cref="Item"/> if one was in focus and available for gathering 
        /// and <see langword="null"/> otherwise</returns>
        public Item Gather()
        {
            //TODO: Limit if it can gather by GatherRate
            if (HasTarget && Target is Item item && Take(item))
            {
                Target = null;  //Since it was gathered and is now owned
                return item;
            }

            return null;
        }

        public bool Take(Item item)
        {
            if (item is null)
                return false;

            if (item is Food food)
            {
                if (Items.Count >= FoodLimit)
                    return false;

                if (food.PickUpWith(this))
                {
                    AddFood(food);
                    return true;
                    //TODO: Add food to the agent's pack visually
                    //transform.SetParent(Owner.transform);
                    //transform.localPosition = -Vector3.forward + Vector3.up;
                }
            }
            else if (item.PickUpWith(this))
            {
                Items.Add(item);
                return true;
                //TODO: Add item to the agent visually
            }
            return false;
        }

        private void AddFood(Food food)
        {
            FoodSupply.Add(food);
            food.gameObject.SetActive(false);
        }

        private void RemoveFood(Food food)
        {
            if (!food)
                return;

            food.gameObject.SetActive(true);
            //TODO: Add some randomness to where it is dropped so there is no conflict on position
            var offset = transform.right * Random.Range(-1f, 1f);
            offset += transform.forward * Random.Range(-1f, 0);
            food.transform.position = transform.position - transform.forward + offset;   
        }


        public T Drop<T>() where T : Item
        {
            if (typeof(T) == typeof(Food))
            {
                var food = Pop(FoodSupply);
                RemoveFood(food);
                return Drop<T>(food);
            }

            var index = Items.FindLastIndex(e => e is T);
            if (index < 0)
                return null;

            return Drop<T>(Pop(Items, index));
        }

        /// <summary>
        /// Calls Item.Drop() and casts the item as a convenience for the public Drop method.
        /// </summary>
        private T Drop<T>(Item item) where T : Item
        {
            if (item)
                item.Drop();
            return item as T;
        }

        /// <summary>
        /// Will attempt to transfer an <see cref="Item"/> to the current Target, 
        /// or drop the item if there is no current Target or the Target cannot accept the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns><see langword="true"/> if the item was transferred to the target and 
        /// <see langword="false"/> if the item was dropped instead</returns>
        public Entity Transfer<T>() where T : Item
        {
            if (HasTarget)
            {
                var target = Target;
                return Transfer<T>(Target as IHolder) ? target : null;
            }
            
            // If there is no target, we just drop
            Drop<T>();
            return null;
        }

        public bool Transfer<T>(IHolder other) where T : Item
        {
            var complete = other?.Take(Drop<T>());
            return complete ?? false;
        }
    }
}
