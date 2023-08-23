using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NSTargetSelector
{
    protected Component _selectedTarget;

    public void SetSelectedTarget(Component comp)
    {
        _selectedTarget = comp;
    }

    public abstract System.Type GetRequiredType();
}

public class NSTargetSelector<T> : NSTargetSelector where T : Component
{
    public T SelectedTarget => _selectedTarget as T;

    public override System.Type GetRequiredType()
    {
        return typeof(T);
    }
}
