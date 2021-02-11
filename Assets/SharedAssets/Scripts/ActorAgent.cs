using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Assets.SharedAssets.Scripts.ScavengerEntity;

public class ActorAgent : Agent
{
    Rigidbody RigidBody;
    
    [Tooltip("Speed of agent rotation.")]
    public float TurnSpeed = 300f;

    [Tooltip("Speed of agent movement.")]
    public float MoveSpeed = 2f;

    [Tooltip("Move Penalty (energy required to move).")]
    public float MovementPenalty = -0.001f;

    [Tooltip("Penalty incurred for each step taken, used to incentivize speed")]
    public float StepPenalty = -0.005f;

    private EnvironmentParameters ResetParams;

    protected Actor Actor;

    public override void Initialize()
    {
        RigidBody = GetComponent<Rigidbody>();
        ResetParams = Academy.Instance.EnvironmentParameters;
    }

    public override void OnEpisodeBegin()
    {
        RigidBody.velocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Actor.CheckForTarget());
        sensor.AddObservation(Actor.Health);

        var summary = Actor.Summarize();
        var color = summary.Color;
        sensor.AddObservation(color.r);
        sensor.AddObservation(color.g);
        sensor.AddObservation(color.b);

        //print("collected observations");
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        //Discrete Actions:
        //  1. Move Forward Axis (Forward, Backward)
        //  2. Move Side Axis (Right, Left)
        //  3. Rotate (Left, Right)
        //  4. Act on Target (Attack)

        //print($"Received actions: {string.Join(",", actions.DiscreteActions)}");
        ActionSegment<int> discrete = actions.DiscreteActions;
        MoveAgent(discrete[0], discrete[1], discrete[2]);
        ExecuteOnTarget(discrete[3]);
        TakeActions(discrete);

        AddReward(StepPenalty);
        //print($"reward: {GetCumulativeReward()}");
    }

    /// <summary>
    /// Override if you want to take actions other than <see cref="Move"/> and <see cref="ExecuteOnTarget"/>
    /// </summary>
    /// <param name="discrete"></param>
    protected virtual void TakeActions(ActionSegment<int> discrete)
    {
        
    }

    private void MoveAgent(int forwardAxis, int rightAxis, int rotateAxis)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

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

        Move(dirToGo, rotateDir);
    }

    virtual protected void Move(Vector3 dirToGo, Vector3 rotateDir)
    {
        if (dirToGo != Vector3.zero)
            AddReward("move_penalty", MovementPenalty);

        RigidBody.AddForce(dirToGo * MoveSpeed, ForceMode.VelocityChange);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * TurnSpeed);

        if (RigidBody.velocity.sqrMagnitude > 25f) // slow it down
        {
            RigidBody.velocity *= 0.95f;
        }
    }

    virtual protected void ExecuteOnTarget(int action)
    {
        //print($"got execute action: {action}");
        switch (action)
        {
            case 1:
                Attack();
                break;
        }
    }

    virtual protected void Attack()
    {
        var destroyed = Actor.Attack();
        if (destroyed)
        {
            print($"destroyed {destroyed}");
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discrete = actionsOut.DiscreteActions;
        discrete.Clear();

        AddMovementInput(discrete);
        AddInputs(discrete);
    }

    virtual protected void AddInputs(ActionSegment<int> discrete)
    {
        if (Input.GetKey(KeyCode.J))
        {
            discrete[3] = 1;
        }
    }

    private void AddMovementInput(ActionSegment<int> discrete)
    {
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

    protected void AddReward(string name, float defaultValue)
    {
        var reward = FindReward(name, defaultValue);
        AddReward(reward);
    }

    protected float FindReward(string name, float defaultValue)
    {
        return ResetParams.GetWithDefault(name, defaultValue);
    }

}
