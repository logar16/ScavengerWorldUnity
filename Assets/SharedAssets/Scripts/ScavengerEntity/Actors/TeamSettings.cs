using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    [ExecuteInEditMode]
    public class TeamSettings : MonoBehaviour
    {
        private const int MAX_TEAMS = 24;

        [Range(1, MAX_TEAMS)]
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

        private Color NextColor(int count)
        {
            var shift = MAX_TEAMS / 2;
            float saturation = 1f;
            float value = 1f;
            if (count > (2 * MAX_TEAMS / 3))
            {
                shift += 1;
                saturation = 0.3f;
                value = 0.6f;
            }
            else if (count > (MAX_TEAMS / 3))
            {
                saturation = 0.5f;
            }
            count = (count * 5 + shift) % MAX_TEAMS;
            float hue = (float)count / MAX_TEAMS;
            return Color.HSVToRGB(hue, saturation, value);
        }
    }
}
