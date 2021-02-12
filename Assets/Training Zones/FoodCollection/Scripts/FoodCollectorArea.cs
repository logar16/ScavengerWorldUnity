using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.SharedAssets.Scripts.ScavengerEntity;

[ExecuteInEditMode]
public class FoodCollectorArea : MonoBehaviour
{
    public GameObject Food;

    [Range(1, 500)]
    [Tooltip("Number of food items to generate at the beginning of every episode (max 500).")]
    public int NumFood = 20;

    [Range(1, 100)]
    [Tooltip("Number of Agents per team.")]
    public int NumAgents = 1;
    
    [Range(8, 200)]
    public float XRange;
    [Range(8, 200)]
    public float ZRange;

    private HashSet<GameObject> FoodPieces;
    private Team[] Teams;

    private GameObject Platform;

    private void Awake()
    {
        Platform = transform.Find("Platform").gameObject;
    }

    private void Start()
    {
        Teams = GetComponentsInChildren<Team>();
        FoodPieces = new HashSet<GameObject>();
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
        foreach (var team in Teams)
        {
            team.Reset();
        }

        CreateFood(NumFood, Food);
    }

    private void Update()
    {
        if (Application.isPlaying)
            return;

        if (Platform)
            Platform.transform.localScale = new Vector3(2 * XRange / 100, 1, 2 * ZRange / 100);
    }

    public bool AllGatheredIn()
    {
        return false; //TODO: check if any team is gathered in
    }
}
