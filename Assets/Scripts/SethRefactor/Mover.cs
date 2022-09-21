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
        [Range(5f, 100f)]
        [SerializeField] private float focusRange = 100f;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private Interactable moveHereIfNoActionMarker;
        [SerializeField] private NavMeshAgent navigator;
        
        private Unit unit;

        public Interactable MoveHereIfNoActionMarker => moveHereIfNoActionMarker;
        public Interactable Target { get; set; }

        private void Awake()
        {
            navigator = GetComponent<NavMeshAgent>();
            unit = GetComponent<Unit>();
        }

        public void Move(Vector3 pos)
        {
            navigator.SetDestination(pos);
        }

        public bool HasReachedTarget(Interactable target) => Vector3.Distance(transform.position, target.transform.position) <= navigator.stoppingDistance;

        public void MoveToTarget(Interactable target)
        {
            navigator.SetDestination(target.transform.position);
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
                if (enemyStorage != null
                    && enemyStorage.TeamId != this.unit.TeamId
                    && enemyStorage.IsStorageDepot)
                {
                    return true;
                }
            }
            enemyStorage=null;
            return false;
        }
    }
}