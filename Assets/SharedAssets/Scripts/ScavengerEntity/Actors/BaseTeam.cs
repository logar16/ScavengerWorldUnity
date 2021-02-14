using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    [Serializable]
    public class UnitData
    {
        public Unit Unit;
        [Range(0, 100)]
        public int Count = 1;
    }

    public class BaseTeam : MonoBehaviour
    {
        [Tooltip("Team ID to distinguish teams for Unity ML-Agents to separate team behaviors.")]
        public int Id;

        public Color Color;

        [ReadOnly]
        public int Budget;

        //TODO: Prefabs for the different unit types
        [SerializeField]
        protected List<UnitData> UnitClasses = new List<UnitData>();

        private List<Unit> Units;
        private IEnumerable<UnitAgent> Agents
        { get => Units.Select(u => u.gameObject.GetComponent<UnitAgent>()); }

        private bool NeedsReset;
        public delegate void RequestReset(BaseTeam requester);
        public event RequestReset OnRequestReset;


        protected virtual void Awake()
        {
            Units = new List<Unit>();
            foreach (var item in UnitClasses)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    var unit = CreateUnit(item.Unit);
                    Units.Add(unit);
                }
            }
        }

        virtual protected Vector3 GeneratePosition()
        {
            var x = Random.Range(-5f, 5f);
            var z = Random.Range(-5f, 5f);
            return new Vector3(x, 0.2f, z) + transform.position;
        }

        private Unit CreateUnit(Unit prototype)
        {
            Vector3 position = GeneratePosition();
            var unit = Instantiate(prototype, position, Quaternion.identity, transform);
            unit.Color = Color;
            unit.Team = this;
            var agent = GetAgentFrom(unit);
            agent.OnNewEpisode += OnNewAgentEpisode;
            return unit;
        }

        public virtual void Reset()
        {
            NeedsReset = false;
            foreach (var unit in Units)
            {
                unit.Reset();
                unit.transform.position = GeneratePosition();
            }
        }

        public bool AllUnitsAreDestroyed()
        {
            return Units.All(u => !u.IsAlive);
        }

        protected UnitAgent GetAgentFrom(Unit unit)
        {
            return unit.gameObject.GetComponent<UnitAgent>();
        }

        public void SetMaxSteps(int maxSteps)
        {
            if (Units == null)
                return;

            foreach (var agent in Agents)
            {
                agent.MaxStep = maxSteps;
            }
        }

        private void OnNewAgentEpisode()
        {
            if (NeedsReset)
                return;

            NeedsReset = true;
            OnRequestReset?.Invoke(this);
        }

        public void EndEpisode(float reward)
        {
            foreach (var unit in Units)
            {
                var agent = GetAgentFrom(unit);
                if (reward != 0)
                    agent.AddReward(reward);
                agent.EndEpisode();
            }
        }

        public int CalculateBudgetDiff()
        {
            return Budget - CalculateUnitCost();
        }

        private int CalculateUnitCost()
        {
            return UnitClasses.Select(i => i.Unit).Sum(u => u ? u.Cost : 0);
        }
    }
}
