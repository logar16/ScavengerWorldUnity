using System;
using UnityEngine;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Assets.SharedAssets.Scripts.ScavengerEntity;

public class UnitAgent : ActorAgent
{
    [Tooltip("Reward for picking up food.")]
    public float FoodGatheredReward = 1f;
    
    [Tooltip("Reward for storing food in the depot.")]
    public float FoodStoredReward = 1f;

    private Unit Unit;

    public override void Initialize()
    {
        base.Initialize();
        Unit = Actor as Unit;
        if (!Unit)
            throw new ArgumentNullException("Actor is not a Unit, this script doesn't fit!");
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);

        sensor.AddObservation(Unit.FoodCount);
        sensor.AddObservation(Unit.ItemCount);
    }


    protected override void TakeActions(ActionSegment<int> actions)
    {
        //Discrete Actions:
        //  5. Transfer/Drop next of (Food, Other Item)
        //  6. Create (Marker)

        TransferOrDrop(actions[4]);
        CreateItem(actions[5]);
    }

    protected override void Attack()
    {
        //TODO: Check if friendly fire using Unit.Team
        base.Attack();
    }

    private void CreateItem(int action)
    {
        switch (action)
        {
            case 1:
                //TODO: Unit.Create(Enum indicating Object Type?);
                break;
        }
    }

    /// <summary>
    /// If the Transfer action returns <see langword="true"/>, a transfer was made, 
    /// but if <see langword="false"/>, the item was dropped.
    /// </summary>
    /// <param name="action"></param>
    private void TransferOrDrop(int action)
    {
        switch (action)
        {
            case 1:
                if (Unit.Transfer<Food>() is StorageDepot)
                {
                    AddReward("food_stored", FoodStoredReward);
                    //print("Stored the food");
                }
                break;
            case 2:
                if (Unit.Transfer<Item>())
                {
                    //print("Transferred an item");
                }
                break;
        }
    }

    override protected void ExecuteOnTarget(int action)
    {
        //print($"got execute action: {action}");
        switch (action)
        {
            case 2:
                Gather();
                break;
            case 1:
                Attack();
                break;
        }
    }

    private void Gather()
    {
        if (Unit.Gather())
        {
            AddReward("food_gathered", FoodGatheredReward);
            //print("Gathered the food");
        }
    }

    override protected void AddInputs(ActionSegment<int> discrete)
    {
        base.AddInputs(discrete);

        if (Input.GetKey(KeyCode.K))
        {
            discrete[3] = 2;
        }
        if (Input.GetKey(KeyCode.L))
        {
            discrete[4] = 1;
        }
        if (Input.GetKey(KeyCode.Semicolon))
        {
            discrete[4] = 2;
        }
        if (Input.GetKey(KeyCode.H))
        {
            discrete[5] = 1;
        }
    }

}
