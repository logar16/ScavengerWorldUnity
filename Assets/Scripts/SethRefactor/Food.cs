using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    public class Food : MonoBehaviour, IInteractable
    {
        [SerializeField] private int amount;
        [SerializeField] private List<Action> actionsAvailable;

        public Transform Transform => transform;

        public void InitActions()
        {
            foreach (Action a in actionsAvailable)
            {
                a.Target = this;
            }
        }
    }
}