using System;
using UnityEngine;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Assets.SharedAssets.Scripts.ScavengerEntity;

public class UnitAgent : ActorAgent
{
    protected Unit Unit;

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

        sensor.AddObservation((float)Unit.FoodCount / Unit.FoodLimit);
        sensor.AddObservation((float)Unit.ItemCount / Unit.ItemLimit);
    }


    protected override void TakeActions(ActionSegment<int> actions)
    {
        //Discrete Actions:
        //  5. Transfer/Drop next of (Food, Other Item)
        //  6. Create (Marker)

        TransferOrDrop(actions[4]);
        CreateItem(actions[5]);
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
    protected virtual void TransferOrDrop(int action)
    {
        switch (action)
        {
            case 1:
                Unit.Transfer<Food>();
                break;
            case 2:
                Unit.Transfer<Item>();
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

    protected virtual Item Gather()
    {
        return Unit.Gather();
    }

    protected bool SameTeam(Unit other)
    {
        return other ? SameTeam(other.Team.Id) : false;
    }

    protected bool SameTeam(int id)
    {
        return id == Unit.Team.Id;
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
