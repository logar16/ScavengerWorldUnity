using UnityEngine;

public class FoodLogic : MonoBehaviour
{
    public bool respawn;
    public FoodCollectorArea myArea;

    public void OnEaten()
    {
        if (respawn)
        {
            transform.position = new Vector3(Random.Range(-myArea.XRange, myArea.XRange),
                3f,
                Random.Range(-myArea.XRange, myArea.XRange)) + myArea.transform.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
