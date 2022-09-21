using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Drop Off")]
    public class DropOff : ActionLogic
    {
        public override bool RequiresInRange(Unit unit, Interactable target)
        {
            return true;
        }

        public override void StartAction(Unit unit, Interactable target)
        {
            var food = unit.RemoveAllItems();
            target.Unit.AddItem(food);
            unit.SetReward(food * 0.01f);
            StopAction(unit, target);
        }

        public override void StopAction(Unit unit, Interactable target)
        {
            unit.ActionRunner.ClearCurrentAction();
        }

        public override void UpdateAction(Unit unit, Interactable target)
        {
            
        }
    }
}