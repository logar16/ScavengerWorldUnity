using Grpc.Core.Logging;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace ScavengerWorld
{
    public enum ActionType
    {
        gather,
        dropoff,
        attackenemy,
        attackstorage,
        move,
        none
    }

    /// <summary>
    /// Responsible for executing discrete interaction actions
    /// (e.g. gather, attack, drop off)
    /// </summary>
    [RequireComponent(typeof(Unit))]
    [RequireComponent(typeof(Mover))]
    public class ActionRunner : MonoBehaviour
    {
        [SerializeField] private Unit unit;
        [SerializeField] private Mover mover;
        [SerializeField] private AI.ActorAgent actorAgent;
        [SerializeField] private List<Action> actionsAvailable;

        public Interactable Target { get; private set; }
        public ActionType ActionType { get; private set; }
        public Action CurrentAction { get; private set; }

        private void Awake()
        {
            unit = GetComponent<Unit>();
            mover = GetComponent<Mover>();
            actorAgent = GetComponent<AI.ActorAgent>();

            ActionType = ActionType.none;
        }

        // Start is called before the first frame update
        void Start()
        {
            actorAgent.OnReceivedActions += OnReceivedActions;
        }

        private void OnDestroy()
        {
            actorAgent.OnReceivedActions -= OnReceivedActions;
        }

        // Update is called once per frame
        void Update()
        {
            /// 1. RL model returns action type that should be performed
            /// 2. Agent searches for nearby interactable that can be acted with action type
            /// 3. Set CurrentAction
            /// 

            if (ActionType == ActionType.none) return;

            if (ActionType == ActionType.move)
            {
                // move logic
                return;
            }

            DecideAction();
            //SetCurrentAction();

            if (CurrentAction is null || CurrentAction.IsEmpty) return;

            if (CurrentAction.RequiresInRange(unit))
            {
                if (!mover.HasReachedTarget())
                {
                    mover.MoveToTarget();
                    return;
                }
                mover.StopMoving();
            }

            if (!CurrentAction.IsRunning)
            {
                CurrentAction.StartAction(unit);
                return;
            }

            CurrentAction.UpdateAction(unit);
        }

        public void DecideAction()
        {
            switch (ActionType)
            {
                case ActionType.gather:
                    Gatherable gatherable;
                    mover.FoodIsNearby(out gatherable);
                    if (gatherable != null)
                    {
                        Target = gatherable.Interactable;
                        CurrentAction = GetActionOfType(ActionType.gather);
                    }                        
                    else
                    {
                        Target = null;
                        CurrentAction = null;
                    }                         
                    break;
                case ActionType.dropoff:
                    Target = unit.StorageDepot.Interactable;
                    CurrentAction = GetActionOfType(ActionType.dropoff);
                    CurrentAction.Target = Target;
                    break;
                case ActionType.attackenemy:
                    Unit enemyUnit;
                    mover.EnemyUnitIsNearby(out enemyUnit);
                    if (enemyUnit != null)
                    {
                        Target = enemyUnit.Interactable;
                        CurrentAction = GetActionOfType(ActionType.attackenemy);
                        CurrentAction.Target = Target;
                    }
                    else
                    {
                        Target = null;
                        CurrentAction = null;
                    }
                    break;
                case ActionType.attackstorage:
                    Unit enemyStorage;
                    mover.EnemyStorageIsNearby(out enemyStorage);
                    if (enemyStorage != null)
                    {
                        Target = enemyStorage.Interactable;
                        CurrentAction = GetActionOfType(ActionType.attackstorage);
                    }
                    else
                    {
                        Target = null;
                        CurrentAction= null;
                    }
                    break;
                default:
                    break;
            }
        }

        private void OnReceivedActions(ActionSegment<int> discrete)
        {
            // Run action determined by RL model
        }

        public Action GetActionOfType(ActionType actionType)
        {
            foreach (Action a in actionsAvailable)
            {
                if (a.ActionType == actionType)
                {
                    return a;
                }
            }
            return null;
        }

        public void Clear()
        {
            Target = null;
            CurrentAction = null;
        }

        public void SetCurrentAction(Action action)
        {
            CurrentAction = action;
        }

        public void CancelCurrentAction()
        {
            CurrentAction.StopAction(unit);
            CurrentAction = null;
        }        
    }
}