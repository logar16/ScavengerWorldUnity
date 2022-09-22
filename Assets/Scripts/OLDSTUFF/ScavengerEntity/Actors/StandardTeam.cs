using System;
using System.Linq;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public class StandardTeam : BaseTeam
    {
        public StorageDepot StorageDepot;

        protected override void Awake()
        {
            StorageDepot = Instantiate(StorageDepot, transform);
            StorageDepot.Color = Color;
            StorageDepot.Team = this;

            base.Awake();
        }

        
        public override void Reset()
        {
            base.Reset();
            StorageDepot.Reset();
        }

        protected override Vector3 GeneratePosition()
        {
            Vector3 position;
            do
            {
                position = base.GeneratePosition();
            } while (Vector3.Distance(position, StorageDepot.transform.position) < 2);
            return position;
        }
    }
}
