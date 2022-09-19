using Grpc.Core.Logging;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace ScavengerWorld
{
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
            actorAgent.OnActionsReceived += OnActionsReceived;
        }

        private void OnDestroy()
        {
            actorAgent.OnActionsReceived -= OnActionsReceived;
        }

        // Update is called once per frame
        void Update()
        {
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

        private void OnActionsReceived(ActionSegment<int> discrete)
        {
            // Run action determined by RL model
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