using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Drop Off")]
    public class DropOff : ActionLogic
    {
        public override bool RequiresInRange(Unit agent, IInteractable target)
        {
            return true;
        }

        public override void StartAction(Unit agent, IInteractable target)
        {
            // Implement drop off logic here 
        }

        public override void StopAction(Unit agent, IInteractable target)
        {
            
        }

        public override void UpdateAction(Unit agent, IInteractable target)
        {
            
        }
    }
}