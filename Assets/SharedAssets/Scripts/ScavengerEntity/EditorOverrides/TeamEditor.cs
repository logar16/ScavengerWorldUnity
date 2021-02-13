using UnityEditor;
using UnityEngine;

namespace Assets.SharedAssets.Scripts.ScavengerEntity
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Team))]
    public class TeamEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ComponentsDisplay();
            InformationalDisplay();
        }

        private void ComponentsDisplay()
        {
            var so = serializedObject;
            so.Update();

            EditorGUILayout.PropertyField(so.FindProperty("Id"));
            EditorGUILayout.PropertyField(so.FindProperty("Color"));
            EditorGUILayout.PropertyField(so.FindProperty("StorageDepot"));
            EditorGUILayout.PropertyField(so.FindProperty("Budget"));
            var classes = so.FindProperty("UnitClasses");
            EditorGUILayout.LabelField("Classes", EditorStyles.boldLabel);

            for (int i = 0; i < classes.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("X", GUILayout.MaxWidth(20)))
                {
                    classes.DeleteArrayElementAtIndex(i);
                    i--;
                }

                if (i < 0 || i >= classes.arraySize)
                    continue;

                var item = classes.GetArrayElementAtIndex(i);
                EditorGUILayout.ObjectField(item.FindPropertyRelative("Unit"), GUIContent.none);

                var count = item.FindPropertyRelative("Count");
                EditorGUILayout.PropertyField(count, GUIContent.none);
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Class", GUILayout.MaxWidth(80)))
            {
                classes.InsertArrayElementAtIndex(classes.arraySize);
            }

            so.ApplyModifiedProperties();
        }

        private void InformationalDisplay()
        {
            var team = target as Team;
            var budgetDiff = team.CalculateBudgetDiff();
            if (budgetDiff < 0)
            {
                EditorGUILayout.HelpBox($"Budget exceeded by {-budgetDiff}!", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox($"Remaining budget: {budgetDiff}", MessageType.Info);
            }
        }
    }
}
