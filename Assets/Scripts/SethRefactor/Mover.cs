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
        [SerializeField] private float FocusRange = 1.5f;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private NavMeshAgent navigator;
        [SerializeField] private AI.ActorAgent actorAgent;
        [SerializeField] private Unit unit;

        public IInteractable Target { get; set; }

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

        public bool HasReachedTarget() => Vector3.Distance(transform.position, Target.Transform.position) <= navigator.stoppingDistance;

        public void MoveToTarget()
        {
            navigator.SetDestination(Target.Transform.position);
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

        /// <summary>
        /// Sends out a Raycast to check for any interactable directly in front of the actor.
        /// Modify which items are targetable by adjusting the list of DetectableTags
        /// </summary>
        /// <returns><see langword="true"/> if a target was found (and added)</returns>
        public bool CheckForTarget()
        {
            //Debug.DrawRay(transform.position, transform.forward * FocusRange, Color.green);
            //position + transform.forward * 0.5f (if agent's body gets in the way)
            var ray = new Ray(transform.position, transform.forward);
            if (Physics.SphereCast(ray, 0.75f, out RaycastHit hit, FocusRange, interactableLayer))
            {
                var target = hit.collider.gameObject;
                if (target.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    Target = interactable;
                    return true;
                }
            }
            Target = null;
            return false;
        }

        public bool FoodIsNearby()
        {
            var ray = new Ray(transform.position, transform.forward);
            if (Physics.SphereCast(ray, 0.75f, out RaycastHit hit, FocusRange, interactableLayer))
            {
                Food target = hit.collider.gameObject.GetComponent<Food>();
                if (target != null)
                {
                    return true;
                }
            }
            return false;
        }

        public bool EnemyUnitIsNearby()
        {
            var ray = new Ray(transform.position, transform.forward);
            if (Physics.SphereCast(ray, 0.75f, out RaycastHit hit, FocusRange, interactableLayer))
            {
                Unit target = hit.collider.gameObject.GetComponent<Unit>();
                if (target != null 
                    && target.TeamId != unit.TeamId 
                    && !target.IsStorageDepot)
                {
                    return true;
                }
            }
            return false;
        }

        public bool EnemyStorageIsNearby()
        {
            var ray = new Ray(transform.position, transform.forward);
            if (Physics.SphereCast(ray, 0.75f, out RaycastHit hit, FocusRange, interactableLayer))
            {
                Unit target = hit.collider.gameObject.GetComponent<Unit>();
                if (target != null 
                    && target.TeamId != unit.TeamId
                    && target.IsStorageDepot)
                {
                    return true;
                }
            }
            return false;
        }
    }
}