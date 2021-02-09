using System.Collections.Generic;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    /// <summary>
    /// Contains the elements that define a ray perception sensor.
    /// </summary>
    public struct SummaryRayInput
    {
        /// <summary>
        /// Length of the rays to cast. This will be scaled up or down based on the scale of the transform.
        /// </summary>
        public float RayLength;

        public float CastRadius;

        /// <summary>
        /// List of tags which correspond to object types agent can see.
        /// </summary>
        public IReadOnlyList<string> DetectableTags;

        /// <summary>
        /// List of angles (in degrees) used to define the rays.
        /// 90 degrees is considered "forward" relative to the game object.
        /// </summary>
        public IReadOnlyList<float> Angles;

        public int Rays { get => Angles.Count; }

        /// <summary>
        /// Transform of the GameObject.
        /// </summary>
        public Transform Transform;

        /// <summary>
        /// Filtering options for the casts.
        /// </summary>
        public int LayerMask;


        /// <summary>
        /// Get the cast start and end points for the given ray index
        /// </summary>
        /// <param name="rayIndex"></param>
        /// <returns>A tuple of the start and end positions in world space.</returns>
        public (Vector3 StartPositionWorld, Vector3 EndPositionWorld) RayExtents(int rayIndex)
        {
            var angle = Angles[rayIndex];
            Vector3 startPositionLocal = Vector3.zero; //Transform.forward * (.5f + CastRadius);
            Vector3 endPositionLocal = PolarToCartesian3D(RayLength, angle);

            var startPositionWorld = Transform.TransformPoint(startPositionLocal);
            var endPositionWorld = Transform.TransformPoint(endPositionLocal);

            return (StartPositionWorld: startPositionWorld, EndPositionWorld: endPositionWorld);
        }

        /// <summary>
        /// Converts polar coordinate to cartesian coordinate.
        /// </summary>
        static internal Vector3 PolarToCartesian3D(float radius, float angleDegrees)
        {
            var x = radius * Mathf.Cos(Mathf.Deg2Rad * angleDegrees);
            var z = radius * Mathf.Sin(Mathf.Deg2Rad * angleDegrees);
            return new Vector3(x, 0f, z);
        }
    }
}
