using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Events;

namespace ScavengerWorld.AI
{
    public class ActorAgent : Agent
    {
        [Range(1, 10)]
        public float MovementScale = 3;
        [SerializeField] private Mover mover;
        [SerializeField] private Unit unit;

        public Unit Unit => unit;

        public UnityAction OnNewEpisode;
        public UnityAction<ActionRequest> OnReceivedActions;

        public override void Initialize()
        {
            mover = GetComponent<Mover>();
            unit = GetComponent<Unit>();
            unit.OnRewardEarned += AddReward;
        }

        public override void OnEpisodeBegin()
        {
            OnNewEpisode?.Invoke();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            var health = unit.Damageable.HealthPercentage;
            var foodInventory = unit.HowFullIsInventory;
            var foodNearby = mover.FoodIsNearby(out _);
            var enemyUnitsNearby = mover.EnemyUnitIsNearby(out _);
            var enemyStorageNearby = mover.EnemyStorageIsNearby(out _);

            sensor.AddObservation(health);
            sensor.AddObservation(foodInventory);
            sensor.AddObservation(foodNearby);
            sensor.AddObservation(enemyUnitsNearby);
            sensor.AddObservation(enemyStorageNearby);
        }


        public override void OnActionReceived(ActionBuffers actions)
        {
            ActionSegment<int> discrete = actions.DiscreteActions;
            var gather = discrete[0];
            var drop = discrete[1];
            var attack = discrete[2];
            var move = discrete[3];

            var request = new ActionRequest(ActionType.none);
            if (gather > 0)
            {
                request = new ActionRequest(ActionType.gather);
            }
            else if (drop > 0)
            {
                request = new ActionRequest(ActionType.dropoff);
            }
            else if (attack == 1)
            {
                request = new ActionRequest(ActionType.attackenemy);
            }
            else if (attack == 2)
            {
                request = new ActionRequest(ActionType.attackstorage);
            }
            else if (move > 0)
            {
                var newPosition = FindMoveToPoint(move);
                request = new ActionRequest(ActionType.move, newPosition);
            }
            OnReceivedActions?.Invoke(request);
        }

        private Vector3 FindMoveToPoint(int move)
        {
            Vector3 change = Vector3.zero;
            switch (move)
            {
                case 1:
                    change = Vector3.forward;
                    break;
                case 2:
                    change = Vector3.forward + Vector3.right;
                    break;
                case 3:
                    change = Vector3.right;
                    break;
                case 4:
                    change = Vector3.right + Vector3.back;
                    break;
                case 5:
                    change = Vector3.back;
                    break;
                case 6:
                    change = Vector3.back + Vector3.left;
                    break;
                case 7:
                    change = Vector3.left;
                    break;
                case 8:
                    change = Vector3.left + Vector3.forward;
                    break;
            }
            return change * MovementScale;
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var discrete = actionsOut.DiscreteActions;
            var gather = Input.GetKeyDown(KeyCode.G) ? 1 : 0;
            var drop = Input.GetKeyDown(KeyCode.D) ? 1 : 0;
            var attack = Input.GetKeyDown(KeyCode.A) ? 1 : 0;
            if (attack == 0)
            {
                attack = Input.GetKeyDown(KeyCode.S) ? 2 : 0;
            }

            var move = 0;
            for (int i = 0; i < 9; i++)
            {
                var key = KeyCode.Keypad1 + i;
                if (key != KeyCode.Keypad5 && Input.GetKeyDown(key))
                {
                    move = i;
                }
            }

            discrete[0] = gather;  // gather
            discrete[1] = drop;  // drop
            discrete[2] = attack;  // attack
            discrete[3] = move;  // move
        }
    }
}
