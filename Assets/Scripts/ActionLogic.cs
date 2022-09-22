using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    public abstract class ActionLogic : ScriptableObject
    {
        public ActionType actionType;

        [TextArea(1,5)]
        public string description;

        public abstract bool RequiresInRange(Unit agent, Interactable target);

        /// <summary>
        /// Logic to run before time-dependent logic runs. Good for checking things
        /// or executing instantaneous logic.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="target"></param>
        public abstract void StartAction(Unit agent, Interactable target);

        /// <summary>
        /// Time-dependent action logic goes here. Ex: gathering over time
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="target"></param>
        public abstract void UpdateAction(Unit agent, Interactable target);

        /// <summary>
        /// Logic to run after time-dependent logic has run. Good for any cleanup
        /// or post-action checks.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="target"></param>
        public abstract void StopAction(Unit agent, Interactable target);
    }
}