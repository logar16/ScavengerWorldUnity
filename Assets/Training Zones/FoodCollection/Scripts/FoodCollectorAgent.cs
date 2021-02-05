using System;
using UnityEngine;
using Unity.MLAgents;

using Random = UnityEngine.Random;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Assets.SharedAssets.Scripts.ScavengerEntity;

public class FoodCollectorAgent : Agent
{
    Rigidbody RigidBody;
    FoodCollectorSettings Settings;

    public FoodCollectorArea Arena;
    
    [Tooltip("Speed of agent rotation.")]
    public float TurnSpeed = 300f;

    [Tooltip("Speed of agent movement.")]
    public float MoveSpeed = 2f;

    [Tooltip("Reward for picking up food.")]
    public float FoodGatheredReward = 1f;
    
    [Tooltip("Reward for storing food in the depot.")]
    public float FoodStoredReward = 1f;

    [Tooltip("Move Penalty (energy required to move).")]
    public float MovementPenalty = -0.001f;

    [Tooltip("Penalty incurred for each step taken, used to incentivize speed")]
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
        var hasTarget = Unit.CheckForTarget();
        //print($"hasTarget: {hasTarget}");
        sensor.AddObservation(hasTarget);
    }


    public void MoveAgent(int forwardAxis, int rightAxis, int rotateAxis)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        //print($"(forward, right, rotate): ({forwardAxis}, {rightAxis}, {rotateAxis})");

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
                dirToGo += transform.right;
                break;
            case 2:
                dirToGo -= transform.right;
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
        //Discrete Actions:
        //  1. Move Forward Axis
        //  2. Move Side Axis
        //  3. Rotate 
        //  4. Targeting Actions (Gather, Transfer Food, Attack)
        //  5. Drop next item (Food, Other)

        //print($"Received actions: {string.Join(",", actions.DiscreteActions)}");
        ActionSegment<int> discrete = actions.DiscreteActions;
        MoveAgent(discrete[0], discrete[1], discrete[2]);
        ExecuteOnTarget(discrete[3]);
        Drop(discrete[4]);

        if (Arena.AllGatheredIn())
        {
            AddReward("completed", 10);
            EndEpisode();
        }
        
        AddReward(StepPenalty);
        Settings.TotalScore += StepPenalty;
        //print($"reward: {GetCumulativeReward()}");
    }

    private void Drop(int action)
    {
        switch (action)
        {
            case 1:
                Unit.Drop<Food>();
                //print($"dropping food {}");
                break;
            case 2:
                Unit.Drop<Item>();
                //print($"dropping item {}");
                break;
        }
    }

    private void ExecuteOnTarget(int action)
    {
        //print($"got execute action: {action}");
        switch (action)
        {
            case 1:
                if (Unit.Gather())
                {
                    AddReward("food_gathered", FoodGatheredReward);
                    //print("Gathered the food");
                }
                break;
            case 2:
                if (Unit.Transfer<Food>())
                {
                    AddReward("food_stored", FoodStoredReward);
                    //print("Stored the food");
                }
                break;
            case 3:
                var destroyed = Unit.Attack();
                if (destroyed)
                {
                    //print($"destroyed {destroyed}");
                }
                break;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discrete = actionsOut.DiscreteActions;
        for (int i = 0; i < discrete.Length; i++)
        {
            discrete[i] = 0;
        }

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

        if (Input.GetKey(KeyCode.J))
        {
            discrete[3] = 1;
        }
        if (Input.GetKey(KeyCode.K))
        {
            discrete[3] = 2;
        }
        if (Input.GetKey(KeyCode.H))
        {
            discrete[3] = 3;
        }
        if (Input.GetKey(KeyCode.L))
        {
            discrete[4] = 1;
        }
    }

    void AddReward(string name, float defaultValue)
    {
        var reward = ResetParams.GetWithDefault(name, defaultValue);
        AddReward(reward);
        Settings.TotalScore += reward;
    }

}
