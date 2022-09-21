using Assets.SharedAssets.Scripts.ScavengerEntity;
using System.Linq;
using Unity.MLAgents;

namespace Assets.TrainingZones.FoodCollector
{
    public class FoodCollectionArena : Arena
    {
        protected override void Awake()
        {
            base.Awake();
            Academy.Instance.AgentPreStep += OnAgentPreStep;
        }

        private void OnAgentPreStep(int obj)
        {
            if (FoodPieces.All(f => f.Stored))
                EndEpisodeForAll(1f);
        }
    }
}
