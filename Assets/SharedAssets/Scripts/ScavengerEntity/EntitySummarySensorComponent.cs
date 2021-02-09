using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public class EntitySummarySensorComponent : SensorComponent
    {
        public string SensorName = "Entity Summary Sensor";

        [Range(0.1f, 100f)]
        [Tooltip("Length of the rays to cast.")]
        public float RayLength = 20f;

        [Range(0f, 10f)]
        [Tooltip("Radius of sphere to cast. Set to zero for raycasts.")]
        public float SphereCastRadius = 0.5f;

        [Range(0, 20)]
        [Tooltip("Number of rays to the left and right of center.")]
        public int RaysPerDirection = 5;

        [Range(0, 180)]
        [Tooltip("Cone size for rays. Using 90 degrees will cast rays to the left and right. " +
            "Greater than 90 degrees will go backwards.")]
        public float MaxRayDegrees = 70;

        [Tooltip("List of tags in the scene that should be considered a detectable entity. " +
            "If none are specified, the agent will not filter by tag.")]
        public List<string> DetectableTags;

        [Tooltip("Controls which layers the rays can hit.")]
        public LayerMask RayLayerMask;

        [Range(1, 30)]
        [Tooltip("Number of raycast results that will be stacked before being fed to the neural network.")]
        public int ObservationStacks = 1;

        public override int[] GetObservationShape()
        {
            var perStack = RaysPerDirection * EntitySummarySensor.NUM_DATAPOINTS;
            return new[] { perStack * ObservationStacks };
        }

        public override ISensor CreateSensor()
        {
            var sensor = new EntitySummarySensor(SensorName, CreateRayInput());
            if (ObservationStacks > 1)
                return new StackingSensor(sensor, ObservationStacks);
            else
                return sensor;
        }

        private SummaryRayInput CreateRayInput()
        {
            SummaryRayInput input = new SummaryRayInput
            {
                RayLength = RayLength,
                CastRadius = SphereCastRadius,
                Angles = GetRayAngles(RaysPerDirection, MaxRayDegrees),
                DetectableTags = DetectableTags,
                Transform = transform,
                LayerMask = RayLayerMask
            };

            return input;
        }

        /// <summary>
        /// Returns the specific ray angles given the number of rays per direction and the
        /// cone size for the rays.
        /// </summary>
        /// <param name="raysPerDirection">Number of rays to the left and right of center.</param>
        /// <param name="maxRayDegrees">
        /// Cone size for rays. Using 90 degrees will cast rays to the left and right.
        /// Greater than 90 degrees will go backwards.
        /// </param>
        /// <returns></returns>
        private static float[] GetRayAngles(int raysPerDirection, float maxRayDegrees)
        {
            // Example:
            // { 90, 90 - delta, 90 + delta, 90 - 2*delta, 90 + 2*delta }
            var anglesOut = new float[2 * raysPerDirection + 1];
            var delta = maxRayDegrees / raysPerDirection;
            anglesOut[0] = 90f;
            for (var i = 0; i < raysPerDirection; i++)
            {
                anglesOut[2 * i + 1] = 90 - (i + 1) * delta;
                anglesOut[2 * i + 2] = 90 + (i + 1) * delta;
            }
            return anglesOut;
        }
    }
}
