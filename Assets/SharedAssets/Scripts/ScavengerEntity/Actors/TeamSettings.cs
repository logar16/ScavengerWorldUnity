using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    [ExecuteInEditMode]
    public class TeamSettings : MonoBehaviour
    {
        [Range(1, 20)]
        public int NumTeams = 1;

        public int Budget = 100;

        public Team Default;

        public Team[] Teams { get => transform.GetComponentsInChildren<Team>(); }

        private void Update()
        {
            if (Application.isPlaying || NumTeams <= 0)
                return;

            var allTeams = Teams;
            int count = allTeams.Length;
            //Add as needed
            while (count < NumTeams)
            {
                CreateTeam(count);
                count++;
            }
            //Or Remove as needed
            while (count > NumTeams)
            {
                count--;
                DestroyImmediate(allTeams[count].gameObject);
            }
        }

        private void CreateTeam(int count)
        {
            var t = Instantiate(Default, transform);
            t.name = $"Team{count}";
            t.Id = count;
            t.Budget = Budget;
            t.Color = NextColor(count);
        }

        //private static readonly Color[] StandardColors = new[]
        //    { Color.blue, Color.green, Color.red, Color.cyan, Color.magenta, Color.yellow };
        private Color NextColor(int count)
        {
            //TODO: Color more reliably using index
            //  e.g. Red, Blue, Green; Purple, Cyan, Yellow; etc.
            //if (count < StandardColors.Length)
            //    return StandardColors[count];
            return new Color(Random.value, Random.value, Random.value);
        }
    }
}
