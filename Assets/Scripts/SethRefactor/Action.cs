using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    /// <summary>
    /// Wrapper class that allows for creating available actions on Unit in inspector
    /// </summary>
    [System.Serializable]
    public class Action
    {
        [SerializeField] protected ActionLogic actionLogic;

        public ActionType ActionType => actionLogic.actionType;

        public string Name => actionLogic.name;

        public virtual bool IsEmpty => actionLogic is null;        

        public Interactable Target { get; set; }

        public bool IsRunning { get; set; }

        public ActionLogic GetActionLogic() => actionLogic;

        public bool RequiresInRange(Unit unit) => actionLogic.RequiresInRange(unit, Target);

        public void StartAction(Unit unit)
        {
            IsRunning = true;
            actionLogic.StartAction(unit, Target);
        }

        public void UpdateAction(Unit unit)
        {
            actionLogic.UpdateAction(unit, Target);
        }

        public void StopAction(Unit unit)
        {
            IsRunning = false;
            actionLogic.StopAction(unit, Target);
        }
    }
}