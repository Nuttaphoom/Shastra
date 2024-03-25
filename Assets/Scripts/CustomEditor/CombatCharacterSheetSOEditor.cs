using UnityEditor;
using UnityEngine;

namespace Vanaring.Editor
{
    [CustomEditor(typeof(CombatCharacterSheetSO))]
    public class CombatCharacterSheetSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            CombatCharacterSheetSO characterSheet = (CombatCharacterSheetSO)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Secondary Attributes", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Max HP:", GUILayout.Width(100));
            EditorGUILayout.LabelField(characterSheet.GetSecondaryAttribute_MaxHP.ToString());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Max MP:", GUILayout.Width(100));
            EditorGUILayout.LabelField(characterSheet.GetSecondaryAttribute_MaxMP.ToString());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Physical ATK:", GUILayout.Width(100));
            EditorGUILayout.LabelField(characterSheet.GetSecondaryAttribute_PhysicalATK.ToString());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Magical ATK:", GUILayout.Width(100));
            EditorGUILayout.LabelField(characterSheet.GetSecondaryAttribute_MagicalATK.ToString());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ACC :", GUILayout.Width(100));
            EditorGUILayout.LabelField(characterSheet.GetSecondaryAttribute_ACC.ToString());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Evasion :", GUILayout.Width(100));
            EditorGUILayout.LabelField(characterSheet.GetSecondaryAttribute_Evasion.ToString());
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }
    }
}