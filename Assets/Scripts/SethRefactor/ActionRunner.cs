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
        none,
        gather,
        dropoff,
        attackenemy,
        attackstorage,
        move,
    }

    public struct ActionRequest
    {
        public ActionType Type;
        public Vector3 NewPosition;

        public ActionRequest(ActionType actionType, Vector3 newPosition = default)
        {
            Type = actionType;
            NewPosition = newPosition;
        }
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

        public Action CurrentAction { get; private set; }

        private void Awake()
        {
            unit = GetComponent<Unit>();
            mover = GetComponent<Mover>();
            actorAgent = GetComponent<AI.ActorAgent>();
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
            if (CurrentAction is null || CurrentAction.IsEmpty) return;

            if (CurrentAction.RequiresInRange(unit))
            {
                if (!mover.HasReachedTarget(CurrentAction.Target))
                {
                    mover.MoveToTarget(CurrentAction.Target);
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

        public void SetCurrentAction(ActionType actionType, Vector3 moveHereIfNoAction = default)
        {
            switch (actionType)
            {
                case ActionType.gather:
                    InitGatherAction();    
                    break;
                case ActionType.dropoff:
                    InitDropoffAction();
                    break;
                case ActionType.attackenemy:
                    InitAttackEnemyAction();
                    break;
                case ActionType.attackstorage:
                    InitAttackEnemyStorageAction();
                    break;
                case ActionType.move:
                    InitMoveAction(moveHereIfNoAction);
                    break;
                case ActionType.none:
                    CurrentAction = null;
                    break;
                default:
                    break;
            }
        }

        private void InitGatherAction()
        {
            Gatherable gatherable;
            mover.FoodIsNearby(out gatherable);
            if (gatherable != null)
            {
                CurrentAction = GetActionOfType(ActionType.gather);
                CurrentAction.Target = gatherable.Interactable;
                CurrentAction.IsRunning = false;
            }
            else
            {
                CurrentAction = null;
            }
        }

        private void InitDropoffAction()
        {
            CurrentAction = GetActionOfType(ActionType.dropoff);
            CurrentAction.Target = unit.StorageDepot;
            CurrentAction.IsRunning = false;
            Debug.Log($"Dropoff target: {CurrentAction.Target.gameObject.name}");
        }

        private void InitAttackEnemyAction()
        {
            Unit enemyUnit;
            mover.EnemyUnitIsNearby(out enemyUnit);
            if (enemyUnit != null)
            {
                CurrentAction = GetActionOfType(ActionType.attackenemy);
                CurrentAction.Target = enemyUnit.Interactable;
                CurrentAction.IsRunning = false;
            }
            else
            {
                CurrentAction = null;
            }
        }

        private void InitAttackEnemyStorageAction()
        {
            Unit enemyStorage;
            mover.EnemyStorageIsNearby(out enemyStorage);
            if (enemyStorage != null)
            {
                CurrentAction = GetActionOfType(ActionType.attackstorage);
                CurrentAction.Target = enemyStorage.Interactable;
                CurrentAction.IsRunning = false;
            }
            else
            {
                CurrentAction = null;
            }
        }

        public void InitMoveAction(Vector3 pos)
        {
            mover.MoveHereIfNoActionMarker.transform.position = pos;
            mover.MoveHereIfNoActionMarker.gameObject.SetActive(true);

            CurrentAction = GetActionOfType(ActionType.move);
            CurrentAction.Target = mover.MoveHereIfNoActionMarker;
            CurrentAction.IsRunning = false;
        }

        private void OnReceivedActions(ActionRequest request)
        {
            SetCurrentAction(request.Type, request.NewPosition);
        }

        private Action GetActionOfType(ActionType actionType)
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

        public void CancelCurrentAction()
        {
            CurrentAction.StopAction(unit);
            CurrentAction = null;
        }        
    }
}