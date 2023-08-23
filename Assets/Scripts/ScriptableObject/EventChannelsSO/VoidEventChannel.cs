using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events; 
public class VoidEventChannel : DescriptionBaseSO
{
    public UnityAction OnEvent;  
    
    public void RaiseEvent()
    {
        OnEvent?.Invoke(); 
    }
}
