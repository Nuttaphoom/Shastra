

using System.CodeDom;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Vanaring_DepaDemo;

/// <summary>
/// Mostly used for "Play vfx for certain amount of time and do something"  
/// </summary>
public class VFXEntity<T>  
{
    private GameObject _vfxPrefab;
    private GameObject _target;
    private float _waitDuration = 0.0f; 

    private VFXCallback _action; 

    public delegate IEnumerator VFXCallback(T obj);
    public VFXEntity(GameObject target, GameObject vfxPrefab, float waitDuration, VFXCallback argc)
    {
        _target = target;  
        _vfxPrefab = vfxPrefab; 
        _waitDuration = waitDuration ;
         _action = argc ; 
    }

  

    public IEnumerator PlayVFX(T arugment)
    {
        _vfxPrefab.transform.position = _target.transform.position;

        yield return new WaitForSeconds(_waitDuration);

        _vfxPrefab.SetActive(false);

        yield return _action(arugment);
    }

}