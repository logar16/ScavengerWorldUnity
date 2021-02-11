using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    //[ExecuteInEditMode]
    public class Team: MonoBehaviour
    {
        public int Id;

        public int Budget;

        [SerializeField]
        List<UnitData> UnitClasses = new List<UnitData>();
        public List<UnitData> Classes { get => UnitClasses; }  //TODO: Prefabs for the different unit types

        public Color Color;

        private void Awake()
        {
            print($"team awake with {Classes.Count} unit types");
            foreach (var item in Classes)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    var unit = Instantiate(item.Unit, transform.parent.parent);
                    print($"created {unit}");
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
