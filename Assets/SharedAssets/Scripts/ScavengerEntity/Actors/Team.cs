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

        [SerializeField]
        List<UnitData> UnitClasses = new List<UnitData>();
        public List<UnitData> Classes { get => UnitClasses; }  //TODO: Prefabs for the different unit types

        public Color Color;

        private List<Unit> Units;

        private void Awake()
        {
            Units = new List<Unit>();
        }

        internal void Reset()
        {
            StorageDepot.Reset();
            Units.ForEach(u => u.Reset());
        }

        private void Start()
        {
            StorageDepot = Instantiate(StorageDepot, transform);
            StorageDepot.Color = Color;

            print($"team started with {Classes.Count} unit types");
            foreach (var item in Classes)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    var x = Random.Range(-4f, 4f);
                    var z = Random.Range(-4f, 4f);
                    Vector3 position = new Vector3(x, 0, z) + transform.position;
                    var unit = Instantiate(item.Unit, position, Quaternion.identity, transform);
                    unit.Color = Color;
                    Units.Add(unit);
                }
            }
        }

        public int CalculateBudgetDiff()
        {
            return Budget - CalculateUnitCost();
        }

        private int CalculateUnitCost()
        {
            return Classes.Select(i => i.Unit).Sum(u => u ? u.Cost : 0);
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
