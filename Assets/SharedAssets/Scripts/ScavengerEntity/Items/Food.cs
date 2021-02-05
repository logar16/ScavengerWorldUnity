using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public enum Quality { POOR, FAIR, GOOD, EXCELLENT };
    public class Food : Item
    {
        public Quality Quality = Quality.GOOD;

        private void Update()
        {
            //TODO: Decay food over time?
        }
    }
}
