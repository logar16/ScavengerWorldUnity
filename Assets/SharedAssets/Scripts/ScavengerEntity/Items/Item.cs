
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public abstract class Item : Entity
    {
        public Unit Creator { get; private set; }
        public void CreatedBy(Unit creator) => Creator = creator;

        public IHolder Owner { get; private set; }

        public bool Taken { get => Owner != null; }

        override public void Reset()
        {
            Owner = null;
            base.Reset();
        }

        /// <summary>
        /// Indicate that the item is being picked up if it is not already owned
        /// </summary>
        /// <returns><see langword="true"/> if the new owner has picked up the item 
        /// (<see langword="false"/> if already taken)</returns>
        public bool PickUpWith(IHolder newOwner)
        {
            if (Taken)
                return false;

            if (newOwner == null)
                return false;

            Owner = newOwner;
            
            //Zero out movement if any
            var rigid = gameObject.GetComponent<Rigidbody>();
            if (rigid)
                rigid.velocity = Vector3.zero;
            return true;
        }

        /// <summary>
        /// The current owner has dropped the item, it should appear in 
        /// </summary>
        public void Drop()
        {
            Owner = null;
        }
    }
}
