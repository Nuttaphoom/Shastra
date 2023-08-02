

using System;
using System.CodeDom;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Vanaring_DepaDemo;


[Serializable]
public class VFXEntity  
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

    [SerializeField]
    private GameObject _VFXAnimationPrefabs;

    public float TimeToLive => _ttl;
    public float SpawnDelay => _spawnDelay;  
    public float CallbackDelay => _callbackDelay; 

    public GameObject VFXAnimationPrefabs => _VFXAnimationPrefabs; 
}


/// <summary>
/// Mostly used for "Play vfx for certain amount of time and do something"  
/// </summary>
public class VFXCallbackHandler<T>  
{
    private VFXEntity _vfxEntity ;
    private GameObject _target;
    private float _waitDuration = 0.0f; 
    private Vector3 _spawnPosition = Vector3.zero; 

    private VFXCallback _action; 

    public delegate IEnumerator VFXCallback(T obj);
    private GameObject _instantiatedVFX; 

    public VFXCallbackHandler(GameObject target, VFXEntity vfxEntity, Vector3 vfxSpawnPosition, VFXCallback argc)
    {
        _target = target;
        _vfxEntity = vfxEntity;
        _instantiatedVFX = MonoBehaviour.Instantiate(_vfxEntity.VFXAnimationPrefabs);
        _instantiatedVFX.transform.position = vfxSpawnPosition; 
         _action = argc ;
        _spawnPosition = vfxSpawnPosition;
    }

  
    public IEnumerator PlayVFX(T arugment)
    {
        _instantiatedVFX.gameObject.SetActive(false);

        yield return new WaitForSeconds(_vfxEntity.SpawnDelay);
        _instantiatedVFX.gameObject.SetActive(true);

        yield return new WaitForSeconds(_vfxEntity.CallbackDelay) ;

        if (_action != null)
            yield return _action(arugment); 

        MonoBehaviour.Destroy(_instantiatedVFX.gameObject);

    }

}