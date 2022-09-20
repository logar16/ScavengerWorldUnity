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

        public void Take(int amount)
        {
            amountAvailable -= amount;
        }
    }
}