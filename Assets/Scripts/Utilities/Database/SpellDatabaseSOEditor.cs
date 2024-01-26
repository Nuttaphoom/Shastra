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
    [CustomEditor(typeof(SpellDatabaseSO))]
    public class SpellDatabaseSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SpellDatabaseSO databaseSO = (SpellDatabaseSO)target;

            // Add a button to call GenerateId()
            if (GUILayout.Button("Generate ID"))
            {
                databaseSO.GenerateId();
            }
            if (GUILayout.Button("Remove Duplicated Elements"))
            {
                databaseSO.RemoveDuplicatedRecord(); 
            }
        }
    }
}
 