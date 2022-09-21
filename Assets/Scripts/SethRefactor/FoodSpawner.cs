using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    public enum FoodDistribution { Random, PsuedoUniform, Clumped }

    public class FoodSpawner : MonoBehaviour
    {
        [Tooltip("Method of food distribution across the arena.")]
        [SerializeField] private FoodDistribution Distribution = FoodDistribution.Random;
        
        [Range(0, 500)]
        [Tooltip("Number of food items to generate at the beginning of every episode (max 500).")]
        [SerializeField] private int NumFood = 20;
        
        [Range(5, 200)]
        [SerializeField] private float XRange = 20;
        
        [Range(5, 200)]
        [SerializeField] private float ZRange = 20;

        [SerializeField] private Gatherable foodPrefab;

        private HaltonSequence sequencer;
        private List<Gatherable> gatherables = new();

        public void CreateFood()
        {
            if (!Application.isPlaying)
                return; //Don't add food if in Edit mode

            for (int i = 0; i < NumFood; i++)
            {
                Gatherable food = Instantiate(foodPrefab, FoodPosition(), Quaternion.identity, transform);
                gatherables.Add(food);
            }
        }

        public void ResetFood()
        {
            if (Distribution == FoodDistribution.PsuedoUniform && Random.value > 0.5)
                sequencer.Reset();

            foreach (var food in gatherables)
            {
                food.transform.position = FoodPosition();
                //food.Reset();  // Need to decide how reset will work
            }
        }

        private Vector3 FoodPosition()
        {
            float x = 0, z = 0;

            switch (Distribution)
            {
                case FoodDistribution.Clumped: //TODO: Better clumping
                case FoodDistribution.Random:
                    x = Random.Range(-XRange, XRange);
                    z = Random.Range(-ZRange, ZRange);
                    break;
                case FoodDistribution.PsuedoUniform:
                    var vec = sequencer.Increment();
                    x = vec.x * XRange * 2 - XRange;
                    z = vec.z * ZRange * 2 - ZRange;
                    break;
            }

            return new Vector3(x, 0.1f, z) + transform.position;
        }
    }
}