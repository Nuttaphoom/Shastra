#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Vanaring
{
    [CustomPropertyDrawer(typeof(SceneField))] 
    public class SceneFieldPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty sceneAsset = property.FindPropertyRelative("_sceneAsset");
            SerializedProperty sceneName = property.FindPropertyRelative("_sceneName"); 

            position = EditorGUI.PrefixLabel(position,GUIUtility.GetControlID(FocusType.Passive),label) ;
            
            if (sceneAsset != null)
            {
                sceneAsset.objectReferenceValue = EditorGUI.ObjectField(position,
                    sceneAsset.objectReferenceValue, typeof(SceneAsset), false); 

                if (sceneAsset.objectReferenceValue != null)
                {
                    sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset).name; 
                }
            }

            EditorGUI.EndProperty(); 
        }
    }
}
#endif 