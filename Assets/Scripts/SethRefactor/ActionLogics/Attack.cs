using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Attack")]
    public class Attack : ActionLogic
    {
        public override bool RequiresInRange(Unit agent, IInteractable target)
        {
            return true;
        }

        public override void StartAction(Unit agent, IInteractable target)
        {
            // Implement damage logic here
        }

        public override void StopAction(Unit agent, IInteractable target)
        {
            
        }

        public override void UpdateAction(Unit agent, IInteractable target)
        {
            
        }
    }
}