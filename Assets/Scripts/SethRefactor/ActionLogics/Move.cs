using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Move")]
    public class Move : ActionLogic
    {
        public override bool RequiresInRange(Unit agent, Interactable target)
        {
            return false;
        }

        public override void StartAction(Unit agent, Interactable target)
        {
            Debug.Log("Started Move action!");
        }

        public override void StopAction(Unit agent, Interactable target)
        {
            // Turn of marker
        }

        public override void UpdateAction(Unit agent, Interactable target)
        {
            
        }
    }
}