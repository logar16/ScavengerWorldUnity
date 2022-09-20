using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    public class Interactable : MonoBehaviour
    {
        private Unit unit;
        private Damageable damageable;
        private Gatherable gatherable;

        public Unit Unit => unit;
        public Damageable Damageable => damageable;
        public Gatherable Gatherable => gatherable;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            damageable = GetComponent<Damageable>();
            gatherable = GetComponent<Gatherable>();
        }
    }
}