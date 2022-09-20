using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [CreateAssetMenu(menuName = "Scavenger World/Action Logics/Gather")]
    public class Gather : ActionLogic
    {
        public override bool RequiresInRange(Unit agent, Interactable target)
        {
            return true;
        }

        public override void StartAction(Unit agent, Interactable target)
        {
            Debug.Log("Started Gather action!");

        }

        public override void StopAction(Unit agent, Interactable target)
        {
            
        }

        public override void UpdateAction(Unit agent, Interactable target)
        {
            // Gather logic here if it happens over time
        }
    }
}