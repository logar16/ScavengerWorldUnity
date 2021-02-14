using Assets.SharedAssets.Scripts.ScavengerEntity;
using System.Linq;
using Unity.MLAgents;

namespace Assets.TrainingZones.Fighting
{
    public class FightingArena : Arena
    {
        protected override void Awake()
        {
            base.Awake();
            Academy.Instance.AgentPreStep += OnAgentPreStep;
        }

        private void OnAgentPreStep(int obj)
        {
            if (Teams.Any(t => t.AllUnitsAreDestroyed()))
                EndEpisodeForAll();
        }
    }
}
