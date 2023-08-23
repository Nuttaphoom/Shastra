using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionBaseSO : ScriptableObject
{
    [TextArea(10,10)]
    [SerializeField]
    private string _destcription; 
}
