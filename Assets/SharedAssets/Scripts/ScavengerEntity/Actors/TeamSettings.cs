using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    [ExecuteInEditMode]
    public class TeamSettings : MonoBehaviour
    {
        [Range(1, 20)]
        public int NumTeams = 1;

        [Tooltip("Each unit has a cost associated with it and this budget " +
            "limits the strength of the team for balancing purposes.")]
        public int Budget = 100;

        [Tooltip("The Prototype team to use when adding new teams")]
        public BaseTeam Default;

        public BaseTeam[] Teams { get => transform.GetComponentsInChildren<BaseTeam>(); }

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
