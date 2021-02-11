using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public interface IHolder
    {
        /// <summary>
        /// This method should check that it is possible to take the item, 
        /// and then alert the item that is has been taken and has a new owner.
        /// If the action is a success, the holder may need to physically move the item around etc.
        /// </summary>
        /// <param name="item">The item that this holder is to take. May be null</param>
        /// <returns><see langword="true"/> if the holder took the item</returns>
        bool Take(Item item);
        
        /// <summary>
        /// Drop the next available item of the given type (if possible)
        /// </summary>
        /// <returns>Either the dropped item or null if none was available</returns>
        T Drop<T>() where T : Item;

        /// <summary>
        /// Transfer ownership of an owned item to a new owner.
        /// This will (using generics) transfer the next item of the given type if possible.
        /// </summary>
        /// <param name="other">The new owner</param>
        bool Transfer<T>(IHolder other) where T : Item;

        //It is assumed that the IHolder inherits from MonoBehavior
        GameObject gameObject { get; }
        Transform transform { get; }
    }
}
