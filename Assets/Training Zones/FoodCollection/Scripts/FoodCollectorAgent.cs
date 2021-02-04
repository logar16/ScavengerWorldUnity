using System;
using UnityEngine;
using Unity.MLAgents;

using Random = UnityEngine.Random;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Assets.SharedAssets.Scripts.ScavengerEntity.Actors.Units;

public class FoodCollectorAgent : Agent
{
    Rigidbody RigidBody;
    FoodCollectorSettings Settings;

    public FoodCollectorArea Arena;
    
    // Speed of agent rotation.
    public float TurnSpeed = 300f;

    // Speed of agent movement.
    public float MoveSpeed = 2f;

    // Reward from food
    public float FoodReward = 1f;

    // Move Penalty (energy required to move)
    public float MovementPenalty = -0.001f;

    public float StepPenalty = -0.005f;

    EnvironmentParameters ResetParams;

    private Unit Unit;

    public override void Initialize()
    {
        RigidBody = GetComponent<Rigidbody>();
        Unit = GetComponent<Unit>();
        ResetParams = Academy.Instance.EnvironmentParameters;
        Settings = FindObjectOfType<FoodCollectorSettings>();
    }

    public override void OnEpisodeBegin()
    {
        RigidBody.velocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
        Arena.Reset();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (Unit.CheckForTarget())
        {
            //sensor.AddOneHotObservation()
        }
    }

    //private void Update()
    //{
    //    Debug.DrawRay(transform.position, transform.forward * 2, Color.green);
    //}

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var forwardAxis = act[0];
        var rightAxis = act[1];
        var rotateAxis = act[2];

        print($"(forward, right, rotate): ({forwardAxis}, {rightAxis}, {rotateAxis})");

        var penalty = ResetParams.GetWithDefault("move_penalty", MovementPenalty);
        penalty *= Convert.ToInt32(forwardAxis > 0) + Convert.ToInt32(rightAxis > 0) + Convert.ToInt32(rotateAxis > 0);
        AddReward(penalty);
        Settings.TotalScore += penalty;

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward;
                break;
            case 2:
                dirToGo = -transform.forward;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right;
                break;
            case 2:
                dirToGo = -transform.right;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = -transform.up;
                break;
            case 2:
                rotateDir = transform.up;
                break;
        }
           
        RigidBody.AddForce(dirToGo * MoveSpeed, ForceMode.VelocityChange);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * TurnSpeed);
        

        if (RigidBody.velocity.sqrMagnitude > 25f) // slow it down
        {
            RigidBody.velocity *= 0.95f;
        }
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        //print($"Received actions: {string.Join(",", actions.DiscreteActions)}");
        MoveAgent(actions.DiscreteActions);
        AddReward(StepPenalty);
        Settings.TotalScore += StepPenalty;
        //print($"reward: {GetCumulativeReward()}");
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discrete = actionsOut.DiscreteActions;
        discrete[0] = 0;
        discrete[1] = 0;
        discrete[2] = 0;
        if (Input.GetKey(KeyCode.D))
        {
            discrete[2] = 2;
        }
        if (Input.GetKey(KeyCode.W))
        {
            discrete[0] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            discrete[2] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discrete[0] = 2;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            discrete[1] = 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            discrete[1] = 2;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("food"))
        {
            var reward = ResetParams.GetWithDefault("food_reward", FoodReward);
            AddReward(reward);
            Settings.TotalScore += reward;
            Arena.RemoveFood(other.gameObject);
        }
    }

    
}
