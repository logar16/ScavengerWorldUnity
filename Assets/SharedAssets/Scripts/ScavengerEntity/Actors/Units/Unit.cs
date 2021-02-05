using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public class Unit : Actor, IHolder
    {
        [Tooltip("How many pieces of food the unit can carry at one time")]
        public int FoodLimit = 10;
        [Tooltip("How many items the unit can carry at one time")]
        public int ItemLimit = 1;

        [HideInInspector]
        public List<Item> Items = new List<Item>();
        [HideInInspector]
        public List<Food> FoodSupply = new List<Food>();

        [Tooltip("The number of food pieces that can be gathered per second")]
        public float GatherRate = 1;

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

        public bool Gather()
        {
            if (HasTarget && Target is Item item && Take(item))
            {
                Target = null;  //Since it was gathered and is now owned
                return true;
            }

            return false;
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
                    //TODO: Add food to the agent's pack
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
            food.transform.position = transform.position - transform.forward;   //TODO: Add some randomness
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

        public bool Transfer<T>() where T : Item
        {
            if (HasTarget)
                return Transfer<T>(Target as IHolder);
            else
                return false;
        }

        public bool Transfer<T>(IHolder other) where T : Item
        {
            var complete = other?.Take(Drop<T>());
            return complete.HasValue ? complete.Value : false;
        }
    }
}
