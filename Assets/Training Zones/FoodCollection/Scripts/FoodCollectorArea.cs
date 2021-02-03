using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class FoodCollectorArea : MonoBehaviour
{
    public GameObject Food;
    public int NumFood;
    public bool RespawnFood;
    public float XRange;
    public float ZRange;
    private HashSet<GameObject> FoodPieces;
    private GameObject Agent;

    private void Start()
    {
        Agent = transform.Find("Agent").gameObject;
        FoodPieces = new HashSet<GameObject>();
    }

    void CreateFood(int num, GameObject type)
    {
        // Only add pieces as needed
        num -= FoodPieces.Count;

        if (num > 10)
        {
            // We need to shuffle around the pieces already down since more than 10 are missing
            foreach (var f in FoodPieces)
            {
                f.transform.position = GenerateChildPosition();
            }
        }

        for (int i = 0; i < num; i++)
        {
            GameObject f = Instantiate(type, GenerateChildPosition(), Quaternion.identity);
            f.SetActive(true);

            FoodPieces.Add(f);
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
        Agent.transform.position = GenerateChildPosition();

        CreateFood(NumFood, Food);
    }


    public void RemoveFood(GameObject food)
    {
        FoodPieces.Remove(food);
        Destroy(food);
        if (FoodPieces.Count == 0)
        {
            Agent.GetComponent<Agent>().EndEpisode();
        }
    }


    private void RemoveAllFood()
    {
        foreach (var food in FoodPieces)
        {
            Destroy(food);
        }
        FoodPieces.Clear();
    }
}
