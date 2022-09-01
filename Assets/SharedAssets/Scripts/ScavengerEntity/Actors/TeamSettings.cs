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

        private void CreateTeam(int id)
        {
            var t = Instantiate(Default, transform);
            t.name = $"Team{id}";
            t.Id = id;
            t.Budget = Budget;
            t.Color = NextColor(id);
        }

        //private static readonly Color[] StandardColors = new[]
        //    { Color.blue, Color.green, Color.red, Color.cyan, Color.magenta, Color.yellow };
        private Color NextColor(int count)
        {
            var grades = 2;  // How many strengths for each color: [0, grades)
            var steps = (grades * 2) - 1;  // Number of grades reflected for a cycle (missing last one so zero is not repeated)
            var lengths = new int[] { 1, 2, 3 };  // How long each cycle lasts. For red, green, and blue, respectively
            var offsets = new int[] { 0, 2, 4 };  // Where in the cycle to begin
            var rgb = new float[3];
            for (int i = 0; i < 3; i++)
            {
                var length = lengths[i];
                var offset = offsets[i];
                var cycle = length * steps;
                var strength = ((count + offset) % cycle) / length;
                if (strength > grades)
                {
                    strength += grades - strength;  // Return toward zero the greater strength overshot grades
                }
                rgb[i] = (float)strength / grades;  // Normalize back to float [0, 1]
            }
            
            return new Color(rgb[0], rgb[1], rgb[2]);
        }
    }
}
