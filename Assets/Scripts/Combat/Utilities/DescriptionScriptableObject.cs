
using UnityEngine;


public abstract class DescriptionScriptableObject : ScriptableObject {

    [TextArea(10,10)]
    [SerializeField]
    private string m_Description ; 
}