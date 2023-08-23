using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NSCardSO : ScriptableObject
{
    public abstract NSRuntimeCard CreateRuntime();
}
