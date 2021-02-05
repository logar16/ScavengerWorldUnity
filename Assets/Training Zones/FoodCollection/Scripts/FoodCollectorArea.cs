using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.SharedAssets.Scripts.ScavengerEntity;

public class FoodCollectorArea : MonoBehaviour
{
    public GameObject Food;
    [Range(1, 400)]
    public int NumFood;
    //TODO: Respawn food as it is taken away
    //public bool RespawnFood;
    //TODO: Dynamically decide where food should go
    public float XRange;
    public float ZRange;

    private HashSet<GameObject> FoodPieces;
    private GameObject Agent;
    private StorageDepot StorageDepot;
    private Entity[] Entities;

    private void Start()
    {
        Agent = transform.Find("Agent").gameObject;
        StorageDepot = GetComponentInChildren<StorageDepot>();
        FoodPieces = new HashSet<GameObject>();
        Entities = GetComponentsInChildren<Entity>();
    }

    void CreateFood(int num, GameObject type)
    {
        // Only add pieces as needed
        num -= FoodPieces.Count;

        // We need to shuffle around the pieces already down since more than 10 are missing
        foreach (var f in FoodPieces)
        {
            f.transform.position = GenerateChildPosition();
        }

        for (int i = 0; i < num; i++)
        {
            GameObject f = Instantiate(type, GenerateChildPosition(), Quaternion.identity);
            f.SetActive(true);

            FoodPieces.Add(f);
        }

        if (Entities.Length < num)
        {
            var e = FoodPieces.Select(p => p.GetComponent<Food>());
            Entities = e.Concat(Entities).ToArray();
        }
    }

    private Vector3 GenerateChildPosition()
    {
        var x = Random.Range(-XRange, XRange);
        var z = Random.Range(-ZRange, ZRange);
        var position = new Vector3(x, 0.2f, z);
        return position + transform.position;
    }

    public void Reset()
    {
        foreach (var entity in Entities)
        {
            entity.Reset();
        }

        Agent.transform.position = GenerateChildPosition();
        StorageDepot.transform.position = GenerateChildPosition();
        CreateFood(NumFood, Food);
    }
    
    public bool AllGatheredIn()
    {
        return StorageDepot.Count == NumFood;
    }
}
