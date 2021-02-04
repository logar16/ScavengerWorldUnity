using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity.Actors.Units
{
    public class Unit : Actor, IHolder
    {
        [Tooltip("How many pieces of food or other items the unit can carry at one time")]
        public int GatherLimit = 10;

        [HideInInspector]
        public List<Item> Items = new List<Item>();

        [Tooltip("The number of food pieces that can be gathered per second")]
        public float GatherRate = 1;

        public void Gather()
        {
            if (HasTarget && Target is Item item)
            {
                Take(item);
            }
        }

        public bool Take(Item item)
        {
            if (Items.Count >= GatherLimit)
                return false;

            if (item.PickUpWith(this))
            {
                Items.Add(item);
                return true;
            }
            return false;
        }

        public void Drop(Item item)
        {
            //TODO: Check if there is anything special that should happen when the item leaves
            //TODO: Does the agent know which item to drop, or it just drops them in a particular order?
            //  Options could be stack or queue, with a stack making sense if moving an item (take and drop)
            Items.Remove(item);
        }

        public Item DropNext()
        {
            var index = Items.Count - 1;
            if (index >= 0)
            {
                var item = Items[index];
                Items.RemoveAt(index);
                return item;
            }

            return null;
        }

        public void Transfer(Item item)
        {
            if (HasTarget)
                Transfer(item, Target as IHolder);
        }

        public void Transfer(Item item, IHolder other)
        {
            item.Transfer(other);
        }

    }
}
