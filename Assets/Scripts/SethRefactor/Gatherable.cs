using System;
using UnityEngine;

namespace ScavengerWorld
{
    [RequireComponent(typeof(Interactable))]
    public class Gatherable : MonoBehaviour
    {
        [SerializeField] private int startingAmount;

        private Interactable interactable;

        private int currentAmount;
        public int AmountAvailable => currentAmount;
        public Interactable Interactable => interactable;

        private void Awake()
        {
            interactable = GetComponent<Interactable>();
            currentAmount = startingAmount;
        }

        // Update is called once per frame
        void Update()
        {
            if (currentAmount <= 0)
            {
                //Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }

        public int Take(int amount)
        {
            int amountLeft = currentAmount - amount;
            int amountToGive;
            if (amountLeft >= 0)
            {
                amountToGive = amount;
                currentAmount = amountLeft;
            }
            else
            {
                amountToGive = currentAmount;
                currentAmount = 0;
            }

            return amountToGive;
        }

        public int TakeAll()
        {
            int amountToGive = currentAmount;
            currentAmount = 0;
            return amountToGive;
        }

        internal void ResetFood()
        {
            gameObject.SetActive(true);
            currentAmount = startingAmount;
        }
    }
}