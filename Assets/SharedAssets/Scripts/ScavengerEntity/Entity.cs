using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public abstract class Entity : MonoBehaviour
    {

        public float Health = 10;

        [HideInInspector]
        public bool IsAlive { get => Health > 0; }
    }

}
