
namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public abstract class Item : Entity
    {
        public IHolder Owner { get; private set; }

        public bool Taken { get => Owner != null; }

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
                return true;

            Owner = newOwner;
            transform.SetParent(Owner.transform);
            return true;
        }

        /// <summary>
        /// The current owner has dropped the item, it should appear in 
        /// </summary>
        public void Drop()
        {
            if (Taken)
            {
                Owner.Drop(this);
                transform.position = Owner.transform.position;
            }

            Owner = null;
        }

        /// <summary>
        /// Transfer from one owner to another (such as food being transferred to a storage depot)
        /// </summary>
        /// <param name="newOwner">If null, the transfer will turn into a drop</param>
        public void Transfer(IHolder newOwner)
        {
            Drop();
            newOwner?.Take(this);
        }
    }
}
