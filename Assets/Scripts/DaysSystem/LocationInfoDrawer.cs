//using System.Collections.Generic;
//using System.ComponentModel;
//using Unity.VisualScripting;
//using UnityEditor;
//using UnityEditor.UIElements;
//using UnityEngine;
//using UnityEngine.UIElements;
//using Vanaring;

//[CustomPropertyDrawer(typeof(LocationSelectionCommandCenter))]
//public class LocationInfoDrawer : PropertyDrawer
//{
//    SerializedProperty type;


//    // Draw the property inside the given rect
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        EditorGUI.BeginProperty(position, label, property);
//        type = (SerializedProperty)property.FindPropertyRelative("_commandType");

//        Debug.Log(type.intValue);
        
//        EditorGUI.PropertyField(position, property.FindPropertyRelative("_commandType"), label);
//        //EditorGUI.PropertyField(position, property.FindPropertyRelative("_loadLocationCommand"), label);


//        //// Using BeginProperty / EndProperty on the parent property means that
//        //// prefab override logic works on the entire property.
//        //

//        //// Draw label
//        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

//        //// Don't make child fields be indented
//        //var indent = EditorGUI.indentLevel;
//        //EditorGUI.indentLevel = 0;

//        //// Calculate rects
//        //var dropdownRect = new Rect(position.x, position.y, 100, position.height);
//        //var amountRect = new Rect(position.x + 105, position.y, 100, position.height);
//        ////var unitRect = new Rect(position.x + 210, position.y, 50, position.height);
//        ////var nameRect = new Rect(position.x + 265, position.y, position.width - 265, position.height);

//        ////// Draw the dropdown for selecting type
//        ////string[] options = { "0", "1", "2", "3" }; // Dropdown options
//        ////type = EditorGUI.Popup(dropdownRect, type, options);

//        //// Draw fields - pass GUIContent.none to each so they are drawn without labels
//        //EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("_loadLocationCommand"), GUIContent.none);
//        ////EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("_command"), GUIContent.none);

//        //if ( NewTypeChecker(type, prevType) )
//        //{

//        //} 

//        // Set indent back to what it was
//        //EditorGUI.indentLevel = indent;

//        EditorGUI.EndProperty();
//    }

//    private bool NewTypeChecker(int type, int prevType)
//    {
//        return (prevType != type); 
//    }
//}