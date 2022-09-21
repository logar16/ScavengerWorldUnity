using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Gather")]
    public class Gather : ActionLogic
    {
        public override bool RequiresInRange(Unit unit, Interactable target)
        {
            return true;
        }

        public override void StartAction(Unit unit, Interactable target)
        {
            unit.AddItem(target.Gatherable);
            StopAction(unit, target);
        }

        public override void StopAction(Unit unit, Interactable target)
        {
            unit.ActionRunner.ClearCurrentAction();
        }

        public override void UpdateAction(Unit unit, Interactable target)
        {
            // Gather logic here if it happens over time
        }
    }
}