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
        /// <param name="item">The item that this holder is to take</param>
        /// <returns><see langword="true"/> if the holder can take the item</returns>
        bool Take(Item item);
        
        /// <summary>
        /// Drop the item (no longer held)
        /// </summary>
        /// <param name="item"></param>
        void Drop(Item item);

        /// <summary>
        /// Drop the next available item (if possible)
        /// </summary>
        Item DropNext();

        /// <summary>
        /// Transfer ownership of an owned item to a new owner
        /// </summary>
        /// <param name="item"></param>
        /// <param name="other">The new owner</param>
        void Transfer(Item item, IHolder other);    //TODO: add a default implementation by using >= C#8

        //It is assumed that the IHolder inherits from MonoBehavior
        GameObject gameObject { get; }
        Transform transform { get; }
    }
}
