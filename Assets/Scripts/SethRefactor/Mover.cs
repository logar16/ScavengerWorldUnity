using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;
using UnityEngine.AI;

namespace ScavengerWorld
{
    /// <summary>
    /// Responsible for handling all movement related logic
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class Mover : MonoBehaviour
    {        
        [Range(.1f, 3f)]
        [SerializeField] private float focusRange = 1.5f;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private NavMeshAgent navigator;
        [SerializeField] private AI.ActorAgent actorAgent;
        [SerializeField] private Unit unit;

        public Interactable Target { get; set; }

        private void Awake()
        {
            navigator = GetComponent<NavMeshAgent>();
            actorAgent = GetComponent<AI.ActorAgent>();
            unit = GetComponent<Unit>();
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

        }

        public void OnReceivedActions(ActionSegment<int> discrete)
        {

        }

        public void Move(Vector3 pos)
        {
            navigator.SetDestination(pos);
        }

        public bool HasReachedTarget() => Vector3.Distance(transform.position, Target.transform.position) <= navigator.stoppingDistance;

        public void MoveToTarget()
        {
            navigator.SetDestination(Target.transform.position);
        }

        public void StopMoving()
        {
            navigator.velocity = Vector3.zero;
            navigator.ResetPath();
        }

        public void HandleRotation()
        {
            // To be implemented if needed
        }

        public bool FoodIsNearby(out Gatherable gatherable)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, focusRange, interactableLayer);

            if (colliders.Length == 0) 
            {
                gatherable = null;
                return false;
            };

            foreach (Collider c in colliders)
            {
                gatherable = c.GetComponent<Gatherable>();
                if (gatherable != null) return true;
            }
            gatherable=null;
            return false;
        }

        public bool EnemyUnitIsNearby(out Unit enemyUnit)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, focusRange, interactableLayer);

            if (colliders.Length == 0)
            {
                enemyUnit = null;
                return false;
            }                

            foreach (Collider c in colliders)
            {
                enemyUnit = c.GetComponent<Unit>();
                if (enemyUnit != null
                    && enemyUnit.TeamId != this.unit.TeamId
                    && !enemyUnit.IsStorageDepot)
                {
                    return true;
                }
            }
            enemyUnit=null;
            return false;
        }

        public bool EnemyStorageIsNearby(out Unit enemyStorage)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, focusRange, interactableLayer);

            if (colliders.Length == 0)
            {
                enemyStorage = null;
                return false;
            }

            foreach (Collider c in colliders)
            {
                enemyStorage = c.GetComponent<Unit>();
                if (unit != null
                    && unit.TeamId != this.unit.TeamId
                    && unit.IsStorageDepot)
                {
                    return true;
                }
            }
            enemyStorage=null;
            return false;
        }
    }
}