

using CustomYieldInstructions;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace Vanaring
{

    [Serializable]
    public class VFXEntity
    {
        [Header("Time before destroy this VFX after callback is called(1 cycle)")]
        [SerializeField]
        private float _destroyAfterCallbackDelay = 0.0f;

        [Header("Delay before this VFX is spawn after creating the object")]
        [SerializeField]
        private float _spawnDelay = 0.0f;

        [Header("Delay before Callback (for VFXCallbackHandler)")]
        [SerializeField]
        private float _callbackDelay = 0.0f;

        [SerializeField]
        private GameObject _VFXAnimationPrefabs;

        public float DestroyDelay => _destroyAfterCallbackDelay;
        public float SpawnDelay => _spawnDelay;
        public float CallbackDelay => _callbackDelay;

        public GameObject VFXAnimationPrefabs => _VFXAnimationPrefabs;

        public bool IsValid() => _VFXAnimationPrefabs != null;
    }


    /// <summary>
    /// Mostly used for "Play vfx for certain amount of time and do something"  
    /// </summary>
    public class VFXCallbackHandler<T>
    {
        private VFXEntity _vfxEntity;
        private CombatEntity _target;
        private float _waitDuration = 0.0f;
        private Vector3 _spawnPosition = Vector3.zero;

        private VFXCallback _action;

        public delegate IEnumerator VFXCallback(T obj);
        private GameObject _instantiatedVFX;

        public VFXCallbackHandler(CombatEntity target, VFXEntity vfxEntity, Vector3 vfxSpawnPosition, VFXCallback argc)
        {
            _target = target;
            _vfxEntity = vfxEntity;
            _instantiatedVFX = MonoBehaviour.Instantiate(_vfxEntity.VFXAnimationPrefabs);
            _instantiatedVFX.transform.position = vfxSpawnPosition;
            _action = argc;
            _spawnPosition = vfxSpawnPosition;
        }


        public IEnumerator PlayVFX(T arugment)
        {
            _instantiatedVFX.gameObject.SetActive(false);

            yield return new WaitForSeconds(_vfxEntity.SpawnDelay);

            _instantiatedVFX.gameObject.SetActive(true);
            if (_instantiatedVFX.GetComponent<ParticleSystem>() != null)
            {
                _instantiatedVFX.GetComponent<ParticleSystem>().Play();
            }
            else if (_instantiatedVFX.GetComponent<VisualEffect>() != null)
            {
                _instantiatedVFX.GetComponent<VisualEffect>().Play();
            }

            yield return new WaitForSeconds(_vfxEntity.CallbackDelay);

            List<IEnumerator> coroutines = new List<IEnumerator>();

            if (_action != null)
                coroutines.Add(_action(arugment));

            coroutines.Add(WaitAndDestroy(_vfxEntity.DestroyDelay));

            yield return new WaitAll(_target, coroutines.ToArray());

        }

        public IEnumerator PlayVFX(T arugment, Vector3 casterpos, Vector3 targetpos)
        {
            StartMovingProjectile(casterpos, targetpos, _vfxEntity.SpawnDelay + _vfxEntity.CallbackDelay);

            yield return new WaitForSeconds(_vfxEntity.SpawnDelay);

            if (_instantiatedVFX.GetComponent<ParticleSystem>() != null)
            {
                _instantiatedVFX.GetComponent<ParticleSystem>().Play();
            }
            else if (_instantiatedVFX.GetComponent<VisualEffect>() != null)
            {
                _instantiatedVFX.GetComponent<VisualEffect>().Play();
            }

            yield return new WaitForSeconds(_vfxEntity.CallbackDelay);

            _instantiatedVFX.gameObject.SetActive(false);

            List<IEnumerator> coroutines = new List<IEnumerator>();

            if (_action != null)
                coroutines.Add(_action(arugment));

            coroutines.Add(WaitAndDestroy(_vfxEntity.DestroyDelay));

            yield return new WaitAll(_target, coroutines.ToArray());

        }

        private IEnumerator WaitAndDestroy(float time)
        {
            yield return new WaitForSeconds(time);
            _instantiatedVFX.gameObject.SetActive(false);

        }

        private IEnumerator MovingProjectile(Vector3 startpos, Vector3 targetpos, float maxtime)
        {
            Vector3 direction = targetpos - startpos;
            //float speed = (direction.magnitude)/* / (maxtime)*/;
            float speed = 1.0f;
            //SphereCollider sc = gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
            Rigidbody rb = _instantiatedVFX.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rb.useGravity = false;

            rb.velocity = new Vector3(speed * direction.normalized.x, speed * direction.normalized.y, speed * direction.normalized.z);
            //_instantiatedVFX.transform.position += new Vector3(speed * direction.normalized.x, speed * direction.normalized.y, speed * direction.normalized.z);
            yield return new WaitForSeconds(maxtime);
            _instantiatedVFX.gameObject.SetActive(false);
        }

        public void StartMovingProjectile(Vector3 startpos, Vector3 targetpos, float maxtime)
        {
            _instantiatedVFX.gameObject.SetActive(true);
            Vector3 direction = targetpos - startpos;
            float speed = (direction.magnitude) / (maxtime);
            //float speed = 0.0f;
            Rigidbody rb = _instantiatedVFX.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rb.useGravity = false;
            rb.velocity = new Vector3(speed * direction.normalized.x, speed * direction.normalized.y, speed * direction.normalized.z);
        }

    }
}