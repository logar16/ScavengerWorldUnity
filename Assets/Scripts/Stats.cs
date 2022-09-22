using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    [System.Serializable]
    public class Stats
    {
        public int attackDamage;

        [Tooltip("Amount gathered every time gather action executes")]
        public int gatherRate;
    }
}