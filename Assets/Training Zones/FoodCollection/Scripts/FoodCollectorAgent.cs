using System;
using UnityEngine;
using Unity.MLAgents;

using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class FoodCollectorAgent : Agent
{
    public GameObject area;
    //FoodCollectorArea MyArea;
    Rigidbody RigidBody;
    
    // Speed of agent rotation.
    public float TurnSpeed = 300;

    // Speed of agent movement.
    public float MoveSpeed = 2;
    public Material NormalMaterial;
    public Material BadMaterial;
    public Material GoodMaterial;
    public Material FrozenMaterial;

    EnvironmentParameters ResetParams;

    public override void Initialize()
    {
        RigidBody = GetComponent<Rigidbody>();
        //MyArea = area.GetComponent<FoodCollectorArea>();
        ResetParams = Academy.Instance.EnvironmentParameters;
        SetResetParameters();
    }

    public override void CollectObservations(VectorSensor sensor)
    {

    }

    

    public void MoveAgent(float[] act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var forwardAxis = (int)act[0];
        var rightAxis = (int)act[1];
        var rotateAxis = (int)act[2];

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

    public override void OnActionReceived(float[] actions)
    {
        MoveAgent(actions);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        actionsOut[1] = 0;
        actionsOut[2] = 0;
        if (Input.GetKey(KeyCode.D))
        {
            actionsOut[2] = 2;
        }
        if (Input.GetKey(KeyCode.W))
        {
            actionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            actionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            actionsOut[0] = 2;
        }
    }

    public override void OnEpisodeBegin()
    {
        RigidBody.velocity = Vector3.zero;
        //transform.position = new Vector3(Random.Range(-MyArea.range, MyArea.range),
        //    2f, Random.Range(-MyArea.range, MyArea.range))
        //    + area.transform.position;
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

        SetResetParameters();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("food"))
        {
            AddReward(ResetParams.GetWithDefault("food_reward", 1f));
            Destroy(other.gameObject);
        }
    }

    public void SetAgentScale()
    {
        float agentScale = ResetParams.GetWithDefault("agent_scale", 1.0f);
        gameObject.transform.localScale = new Vector3(agentScale, agentScale, agentScale);
    }

    public void SetResetParameters()
    {
        SetAgentScale();
    }
}
