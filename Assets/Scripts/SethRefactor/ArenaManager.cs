using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    // Arena reset = all food collected, one team remaining

    public class ArenaManager : MonoBehaviour
    {
        [SerializeField] private FoodSpawner foodSpawner;        

        private void Awake()
        {
            foodSpawner = GetComponent<FoodSpawner>();
        }        

        // Start is called before the first frame update
        void Start()
        {
            foodSpawner.CreateFood();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}