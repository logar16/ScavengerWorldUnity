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
            Debug.Log("Started Dropoff action!");
            target.Unit.AddItem(unit.RemoveAllItems());
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