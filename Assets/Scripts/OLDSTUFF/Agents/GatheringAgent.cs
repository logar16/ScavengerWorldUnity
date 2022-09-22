using Assets.SharedAssets.Scripts.ScavengerEntity;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.Agents
{
    public class GatheringAgent : UnitAgent
    {
        [Tooltip("Reward for picking up food.")]
        public float FoodGatheredReward = 1f;

        [Tooltip("Reward for storing food in the depot.")]
        public float FoodStoredReward = 1f;


        protected override void TransferOrDrop(int action)
        {
            switch (action)
            {
                case 1:
                    if (Unit.Transfer<Food>() is StorageDepot)
                        AddReward("food_stored", FoodStoredReward);
                    break;
                case 2:
                    Unit.Transfer<Item>();
                    break;
            }
        }

        protected override Item Gather()
        {
            var item = base.Gather();
            if (item is Food)
            {
                AddReward("food_gathered", FoodGatheredReward);
            }
            return item;
        }
    }
}
