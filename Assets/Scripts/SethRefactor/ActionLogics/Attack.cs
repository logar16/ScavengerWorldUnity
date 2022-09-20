using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Attack")]
    public class Attack : ActionLogic
    {
        public override bool RequiresInRange(Unit agent, Interactable target)
        {
            return true;
        }

        public override void StartAction(Unit agent, Interactable target)
        {
            // Implement damage logic here
        }

        public override void StopAction(Unit agent, Interactable target)
        {
            
        }

        public override void UpdateAction(Unit agent, Interactable target)
        {
            
        }
    }
}