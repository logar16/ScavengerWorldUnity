using System;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public class EntitySummarySensor : ISensor
    {
        public string SensorName { get; }
        
        private SummaryRayInput RayInput;
        private int ObservationSize;
        private float[] Observations;
        private float[] NoHitData = new float[NUM_DATAPOINTS];
        private HashSet<string> DetectableTags;
        private bool FilterTags;

        /// <summary>
        /// Number of datapoints returned from the summary.
        /// Should be the length of:
        /// [normed-distance, width, height, depth, colorR, colorG, colorB, health, custom1, custom2]
        /// </summary>
        public static readonly int NUM_DATAPOINTS = 10;


        public EntitySummarySensor(string sensorName, SummaryRayInput rayInput)
        {
            SensorName = sensorName;
            RayInput = rayInput;
            ObservationSize = rayInput.Rays * NUM_DATAPOINTS;
            Observations = new float[ObservationSize];
            NoHitData[0] = 1.0f; //Normed-distance will equal 1.0 if no objects are hit
            DetectableTags = new HashSet<string>(RayInput.DetectableTags);
            FilterTags = DetectableTags.Count > 0;
        }


        /// <inheritdoc/>
        public string GetName()
        {
            return SensorName;
        }


        public ObservationSpec GetObservationSpec()
        {
            return ObservationSpec.Vector(ObservationSize);
        }

        /// <inheritdoc/>
        public int Write(ObservationWriter writer)
        {
            Array.Clear(Observations, 0, Observations.Length);

            var numRays = RayInput.Rays;
            for (int i = 0; i < numRays; i++)
            {
                var rayData = PerceiveRay(i, RayInput);
                Array.Copy(rayData, 0, Observations, i * NUM_DATAPOINTS, NUM_DATAPOINTS);
            }

            writer.AddList(Observations);
            return Observations.Length;
        }

        private float[] PerceiveRay(int index, SummaryRayInput input)
        {
            var (startPositionWorld, endPositionWorld) = input.RayExtents(index);

            var rayDirection = endPositionWorld - startPositionWorld;
            var scaledRayLength = Math.Max(rayDirection.magnitude, 0.1f);
            
            int layerMask = input.LayerMask;

            bool castHit;
            RaycastHit rayHitData;

            var radius = input.CastRadius;
            if (radius > 0)
            {
                castHit = Physics.SphereCast(startPositionWorld, radius, rayDirection, 
                    out rayHitData, scaledRayLength, layerMask);
            }
            else
            {
                castHit = Physics.Raycast(startPositionWorld, rayDirection,
                    out rayHitData, scaledRayLength, layerMask);
            }

            if (castHit)
            {
                var normedDistance = rayHitData.distance / scaledRayLength;
                return PerceiveDataPoints(normedDistance, rayHitData.collider.gameObject);
            }

            return NoHitData;
        }

        private float[] PerceiveDataPoints(float normedDistance, GameObject gameObject)
        {
            var datapoints = new float[NUM_DATAPOINTS];
            datapoints[0] = normedDistance;

            if (FilterTags)
            {
                var tag = gameObject.tag;
                if (string.IsNullOrEmpty(tag) || !DetectableTags.Contains(tag))
                    return datapoints;
            }

            var entity = gameObject.GetComponent<Entity>();
            if (entity)
            {
                var data = entity.Summarize().ToArray();
                Array.Copy(data, 0, datapoints, 1, data.Length);
            }

            return datapoints;
        }

        /// <inheritdoc/>
        public void Update()
        {
            //Not currently required
        }

        /// <inheritdoc/>
        public void Reset()
        {
            //Not currently required
        }
        
        /// <inheritdoc/>
        public byte[] GetCompressedObservation()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public SensorCompressionType GetCompressionType()
        {
            return SensorCompressionType.None;
        }

        public CompressionSpec GetCompressionSpec()
        {
            return CompressionSpec.Default();
        }
    }
}
