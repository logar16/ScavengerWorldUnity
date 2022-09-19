using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Gather")]
    public class Gather : ActionLogic
    {
        public override bool RequiresInRange(Unit agent, IInteractable target)
        {
            return true;
        }

        public override void StartAction(Unit agent, IInteractable target)
        {
            // Gather logic here if just an instant pickup
        }

        public override void StopAction(Unit agent, IInteractable target)
        {
            
        }

        public override void UpdateAction(Unit agent, IInteractable target)
        {
            // Gather logic here if it happens over time
        }
    }
}