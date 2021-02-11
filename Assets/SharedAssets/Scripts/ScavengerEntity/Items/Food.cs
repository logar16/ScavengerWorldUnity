using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    public enum Quality { Poor, Fair, Good, Excellent };
    public enum DecayMethod { Linear, Exponential }
    public class Food : Item
    {
        [Tooltip("The initial food quality (defaults to 'Excellent').")]
        public Quality StartQuality = Quality.Excellent;
        public Quality Quality { get => (Quality)Value; }

        private float Value;

        [Tooltip("Allow the food to decay over time (each frame).")]
        public bool AllowDecay = true;
        
        [Tooltip("The Food is decayed every frame using the following decay method and decay rate. " +
            "'Linear' should use a small value and 'Exponential' a large value to slow the decay (and vice versa).")]
        public DecayMethod DecayMethod = DecayMethod.Linear;
        
        [Range(0, 1)]
        public float DecayRate = 0.005f;


        private void Awake()
        {
            Value = (int)StartQuality;
        }

        public override EntitySummary Summarize()
        {
            var summary = base.Summarize();
            summary.Custom1 = (float)Quality;
            return summary;
        }

        private void Update()
        {
            if (AllowDecay)
                Decay();
        }

        private void Decay()
        {
            if (Value <= 0)
            {
                Value = 0;
                return;
            }

            switch (DecayMethod)
            {
                case DecayMethod.Linear:
                    Value -= DecayRate;
                    break;
                case DecayMethod.Exponential:
                    Value *= DecayRate;
                    break;
            }
        }
    }
}
