using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public enum FoodDistribution { Random, PsuedoUniform, Clumped }

    [ExecuteInEditMode]
    public class Arena : MonoBehaviour
    {
        [Range(100, 3000)]
        [Tooltip("Maximum number of steps before environment reset.")]
        public int MaxSteps = 1000;

        [Tooltip("Food Prefab to use for generating food.")]
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
        
        private bool ResetRequested { get => AlreadyEnded.Count > 0; }
        private bool ResetReady { get => AlreadyEnded.Count == Teams.Length; }
        private HashSet<Team> AlreadyEnded;

        private void Awake()
        {
            Platform = transform.Find("Platform").gameObject;
            Sequencer = new HaltonSequence();
            FoodPieces = new Food[NumFood];
            AlreadyEnded = new HashSet<Team>();
            Academy.Instance.AgentPreStep += OnAgentPrestep;
        }

        private void Start()
        {
            SetupTeams();
            CreateFood();
        }

        private void SetupTeams()
        {
            Teams = GetComponentsInChildren<Team>();
            ArrangeTeams();
            foreach (var team in Teams)
            {
                team.OnRequestReset += OnTeamRequestReset;
                team.SetMaxSteps(MaxSteps);
            }
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

        private void OnAgentPrestep(int stepCount)
        {
            if (ResetReady)
                Reset();
            else if (ResetRequested)
                EndEpisodeForAll();
        }

        private void OnTeamRequestReset(Team requester)
        {
            AlreadyEnded.Add(requester);
        }

        private void EndEpisodeForAll()
        {
            foreach (var team in Teams)
            {
                if (AlreadyEnded.Contains(team))
                    continue;

                team.EndEpisode();
                AlreadyEnded.Add(team);
            }
        }

        public void Reset()
        {
            AlreadyEnded.Clear();

            foreach (var team in Teams)
            {
                team.Reset();
            }

            ResetFood();
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
            if (Distribution == FoodDistribution.PsuedoUniform && Random.value > 0.5)
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

            return new Vector3(x, 0.5f, z) + transform.position;
        }


        private void Update()
        {
            if (!Application.isPlaying)
            { 
                if (Platform)
                {
                    var x = 2 * (XRange + 6) / 100;
                    var z = 2 * (ZRange + 6) / 100;
                    Platform.transform.localScale = new Vector3(x, 1, z);
                }
            }
        }

        private void OnDestroy()
        {
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
