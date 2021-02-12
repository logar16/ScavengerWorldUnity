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
        private bool NeedsReset;

        public delegate void RequestReset(Team requester);
        public event RequestReset OnRequestReset;

        private void Awake()
        {
            Units = new List<Unit>();
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

        private void Start()
        {
            StorageDepot = Instantiate(StorageDepot, transform);
            StorageDepot.Color = Color;

            foreach (var item in UnitClasses)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    Vector3 position = PositionUnit();
                    var unit = Instantiate(item.Unit, position, Quaternion.identity, transform);
                    unit.Color = Color;
                    Units.Add(unit);
                    var agent = unit.gameObject.GetComponent<UnitAgent>();
                    agent.OnNewEpisode += OnNewAgentEpisode;
                }
            }
        }

        private Vector3 PositionUnit()
        {
            var x = Random.Range(-4f, 4f);
            var z = Random.Range(-4f, 4f);
            return new Vector3(x, 0.2f, z) + transform.position;
        }

        private void OnNewAgentEpisode()
        {
            if (NeedsReset)
                return;

            NeedsReset = true;
            OnRequestReset?.Invoke(this);
        }

        public void EndEpisode()
        {
            print($"Ending Episodes for team {Id}");
            foreach (var unit in Units)
            {
                var agent = unit.gameObject.GetComponent<UnitAgent>();
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
