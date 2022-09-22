using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld.AI
{
    public class EntitySummary
    {
        public Vector3 Size;
        public Color Color;
        public float Health;
        public float Custom1;
        public float Custom2;

        //Not used in identification
        public Vector3 Position;

        public float[] ToArray()
        {
            return new float[]
            {
                Size.x, Size.y, Size.z, Color.r, Color.g, Color.b, Health, Custom1, Custom2
            };
        }

        public static implicit operator bool(EntitySummary es)
        {
            return es != null;
        }
    }
}