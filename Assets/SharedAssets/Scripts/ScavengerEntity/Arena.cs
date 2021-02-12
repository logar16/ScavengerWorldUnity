using System.Collections.Generic;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public enum FoodDistribution { Random, PsuedoUniform, Clumped }

    [ExecuteInEditMode]
    public class Arena : MonoBehaviour
    {
        public Food Food;

        [Range(1, 500)]
        [Tooltip("Number of food items to generate at the beginning of every episode (max 500).")]
        public int NumFood = 20;

        [Tooltip("Method of food distribution across the arena.")]
        public FoodDistribution Distribution = FoodDistribution.Random;

        [Range(8, 200)]
        public float XRange = 20;
        [Range(8, 200)]
        public float ZRange = 20;

        private Food[] FoodPieces;
        private Team[] Teams;

        private GameObject Platform;
        private HaltonSequence Sequencer;

        private void Awake()
        {
            Platform = transform.Find("Platform").gameObject;
            Sequencer = new HaltonSequence();
            FoodPieces = new Food[NumFood];
        }

        private void Start()
        {
            Teams = GetComponentsInChildren<Team>();
            ArrangeTeams();
            CreateFood();
        }

        private void ArrangeTeams()
        {
            var len = Teams.Length;
            var radius = Mathf.Min(XRange, ZRange) - 2;
            var offset = Random.Range(0, Mathf.PI);
            for (int i = 0; i < len; i++)
            {
                var angle = i * Mathf.PI * 2 / len + offset;
                var x = Mathf.Cos(angle) * radius;
                var z = Mathf.Sin(angle) * radius;
                var position = new Vector3(x, 0, z);
                Teams[i].transform.position = position + transform.position;
            }
        }

        private void CreateFood()
        {
            if (!Application.isPlaying)
                return;

            for (int i = 0; i < NumFood; i++)
            {
                Food food = Instantiate(Food, FoodPosition(), Quaternion.identity, transform);
                FoodPieces[i] = food;
            }
        }

        void ResetFood()
        {
            if (Distribution == FoodDistribution.PsuedoUniform)
                Sequencer.Reset();

            foreach (var food in FoodPieces)
            {
                food.transform.position = FoodPosition();
                food.Reset();
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
                    var vec = Sequencer.Increment();
                    x = vec.x * XRange * 2 - XRange;
                    z = vec.z * ZRange * 2 - ZRange;
                    break;
            }
            
            return new Vector3(x, 0.2f, z) + transform.position;
        }

        public void Reset()
        {
            foreach (var team in Teams)
            {
                team.Reset();
            }

            ResetFood();
        }

        private void Update()
        {
            if (Application.isPlaying)
                return;

            if (Platform)
                Platform.transform.localScale = new Vector3(2 * (XRange + 4) / 100, 1, 2 * (ZRange + 4) / 100);
        }

        private void OnApplicationQuit()
        {
            print("Quitting");
        }

        private void OnDisable()
        {
            print("Disabling");
        }

        private void OnDestroy()
        {
            print("Destroying");
            foreach (var food in FoodPieces)
            {
                if (Application.isPlaying)
                    Destroy(food);
                else
                    DestroyImmediate(food);
            }
        }
    }
}
