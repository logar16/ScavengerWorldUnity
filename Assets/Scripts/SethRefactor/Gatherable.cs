using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [RequireComponent(typeof(Interactable))]
    public class Gatherable : MonoBehaviour
    {
        [SerializeField] private int amountAvailable;

        private Interactable interactable;

        public int AmountAvailable => amountAvailable;
        public Interactable Interactable => interactable;

        private void Awake()
        {
            interactable = GetComponent<Interactable>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (amountAvailable <= 0)
            {
                Destroy(gameObject);
            }
        }

        public int Take(int amount)
        {
            int amountToGive = 0;
            int amountLeft = amountAvailable - amount;
            if (amountLeft >= 0)
            {
                amountToGive = amount;
                amountAvailable = amountLeft;
            }
            else
            {
                amountToGive = amountAvailable;
                amountAvailable = 0;                
            }

            return amountToGive;
        }

        public int TakeAll()
        {
            int amountToGive = amountAvailable;
            amountAvailable = 0;
            return amountToGive;
        }
    }
}