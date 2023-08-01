

using System.CodeDom;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Vanaring_DepaDemo;


public class VFXEntity : MonoBehaviour
{
    [Header("Time before destroy this VFX (1 cycle)")]
    [SerializeField]
    private float _ttl = 0.0f;

    [Header("Delay before this VFX is spawn after creating the object")]
    [SerializeField]
    private float _spawnDelay = 0.0f ;

    [Header("Delay before Callback (for VFXCallbackHandler)")]
    [SerializeField]
    private float _callbackDelay = 0.0f ;

    public float TimeToLive => _ttl;
    public float SpawnDelay => _spawnDelay;  
    public float CallbackDelay => _callbackDelay; 

}


/// <summary>
/// Mostly used for "Play vfx for certain amount of time and do something"  
/// </summary>
public class VFXCallbackHandler<T>  
{
    private VFXEntity _vfxPrefab;
    private GameObject _target;
    private float _waitDuration = 0.0f; 
    private Vector3 _spawnPosition = Vector3.zero; 

    private VFXCallback _action; 

    public delegate IEnumerator VFXCallback(T obj);
    public VFXCallbackHandler(GameObject target, VFXEntity vfxPrefab, Vector3 vfxSpawnPosition, float waitDuration, VFXCallback argc)
    {
        _target = target;  
        _vfxPrefab = vfxPrefab; 
        _waitDuration = waitDuration ;
         _action = argc ;
        _spawnPosition = vfxSpawnPosition;
    }

  
    public IEnumerator PlayVFX(T arugment)
    {
        _vfxPrefab.gameObject.SetActive(false);

        yield return new WaitForSeconds(_vfxPrefab.SpawnDelay);
        _vfxPrefab.gameObject.SetActive(true);

        yield return new WaitForSeconds(_vfxPrefab.CallbackDelay) ;

        yield return _action(arugment); 

    }

}