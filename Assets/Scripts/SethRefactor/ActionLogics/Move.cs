using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Move")]
    public class Move : ActionLogic
    {
        public override bool RequiresInRange(Unit unit, Interactable target)
        {
            return false;
        }

        public override void StartAction(Unit unit, Interactable target)
        {
            Debug.Log("Started Move action!");
            unit.Mover.MoveToTarget(target);
            
        }

        public override void StopAction(Unit unit, Interactable target)
        {
            // Turn of marker            
            unit.Mover.StopMoving();
            unit.ActionRunner.ClearCurrentAction();
            Destroy(target.gameObject);
        }

        public override void UpdateAction(Unit unit, Interactable target)
        {
            if (Vector3.Distance(unit.transform.position, target.transform.position) <= unit.Mover.StopDistance)
            {
                StopAction(unit, target);
            }
        }
    }
}