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
            unit.Mover.MoveToTarget(target);                      // Use this for regular game
            //unit.transform.position = target.transform.position;    // Use this for training
        }

        public override void StopAction(Unit unit, Interactable target)
        {
            // Turn of marker            
            unit.Mover.StopMoving();
            unit.ArenaManager.ReturnMoveMarker(target);
            unit.ActionRunner.ClearCurrentAction();
        }

        public override void UpdateAction(Unit unit, Interactable target)
        {
            float distance = Vector3.Distance(unit.transform.position, target.transform.position);
            if (distance <= unit.Mover.StopDistance)
            {
                StopAction(unit, target);
            }
        }
    }
}