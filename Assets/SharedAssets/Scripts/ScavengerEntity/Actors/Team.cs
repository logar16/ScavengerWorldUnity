using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    //[ExecuteInEditMode]
    public class Team: MonoBehaviour
    {
        public int Id;

        public StorageDepot StorageDepot;

        public int Budget;

        //TODO: Prefabs for the different unit types
        [SerializeField]
        List<UnitData> UnitClasses = new List<UnitData>();

        public Color Color;

        private List<Unit> Units;
        private IEnumerable<UnitAgent> Agents 
            { get => Units.Select(u => u.gameObject.GetComponent<UnitAgent>()); }

        private bool NeedsReset;

        public delegate void RequestReset(Team requester);
        public event RequestReset OnRequestReset;

        private void Awake()
        {
            StorageDepot = Instantiate(StorageDepot, transform);
            StorageDepot.Color = Color;

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

        private Unit CreateUnit(Unit prototype)
        {
            Vector3 position = PositionUnit();
            var unit = Instantiate(prototype, position, Quaternion.identity, transform);
            unit.Color = Color;
            var agent = GetAgentFrom(unit);
            agent.OnNewEpisode += OnNewAgentEpisode;
            return unit;
        }

        public void Reset()
        {
            NeedsReset = false;
            StorageDepot.Reset();
            foreach (var unit in Units)
            {
                unit.Reset();
                unit.transform.position = PositionUnit();
            }
        }

        private UnitAgent GetAgentFrom(Unit unit)
        {
            return unit.gameObject.GetComponent<UnitAgent>();
        }

        private Vector3 PositionUnit()
        {
            var x = Random.Range(-4f, 4f);
            var z = Random.Range(-4f, 4f);
            return new Vector3(x, 0.2f, z) + transform.position;
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

    [Serializable]
    public class UnitData
    {
        public Unit Unit;
        [Range(0, 100)]
        public int Count = 1;
    }
}
