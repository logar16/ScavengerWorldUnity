using UnityEngine;

public class FoodCollectorArea : MonoBehaviour
{
    public GameObject Food;
    public int NumFood;
    public bool RespawnFood;
    public float XRange;
    public float ZRange;

    void CreateFood(int num, GameObject type)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject f = Instantiate(type, 
                new Vector3(Random.Range(-XRange, XRange), 0.2f, Random.Range(-ZRange, ZRange)) + transform.position,
                Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 90f)));
            //f.GetComponent<FoodLogic>().respawn = RespawnFood;
            //f.GetComponent<FoodLogic>().myArea = this;
        }
    }

    public void ResetFoodArea(GameObject[] agents)
    {
        foreach (GameObject agent in agents)
        {
            if (agent.transform.parent == gameObject.transform)
            {
                agent.transform.position = new Vector3(Random.Range(-XRange, XRange), 2f,
                    Random.Range(-ZRange, ZRange))
                    + transform.position;
                agent.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
            }
        }

        CreateFood(NumFood, Food);
    }

}
