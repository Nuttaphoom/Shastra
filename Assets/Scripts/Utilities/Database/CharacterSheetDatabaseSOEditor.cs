#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Vanaring
{
    [CustomEditor(typeof(CharacterSheetDatabaseSO))]
    public class CharacterSheetDatabaseSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            CharacterSheetDatabaseSO databaseSO = (CharacterSheetDatabaseSO)target;

            GUILayout.Space(10);

            GUILayout.Label("Actions:");

            // Add a button to call GenerateId()
            if (GUILayout.Button("Generate ID"))
            {
                databaseSO.GenerateId();
            }
            if (GUILayout.Button("Remove Duplicated Elements"))
            {
                databaseSO.RemoveDuplicatedRecord();
            }

            GUILayout.Space(10);

            GUILayout.Label("Character Sheets:");

            // Display Normal Character Sheets
            GUILayout.Label("Normal Character Sheets:");
            foreach (var characterSheet in databaseSO.GetNormalCharacterSheets())
            {
                EditorGUILayout.ObjectField(characterSheet, typeof(CharacterSheetSO), false);
            }

            GUILayout.Space(10);

            // Display Combat Character Sheets
            GUILayout.Label("Combat Character Sheets:");
            foreach (var combatCharacterSheet in databaseSO.GetCombatCharacterShhets())
            {
                EditorGUILayout.ObjectField(combatCharacterSheet, typeof(CombatCharacterSheetSO), false);
            }
        }
    }
}
#endif