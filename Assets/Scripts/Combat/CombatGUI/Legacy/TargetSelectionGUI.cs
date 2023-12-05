
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vanaring
{
    [Serializable]
    public class TargetSelectionGUI : RequireInitializationHandler<Transform, Null, Null>
    {
        [SerializeField]
        private TargetGUI targetGUI;

        [SerializeField]
        private Dictionary<CombatEntity, GameObject> _instantiatedTargetGUI = new Dictionary<CombatEntity, GameObject>();

        private Dictionary<CombatEntity, GameObject> _instantiatedVFXCircle = new Dictionary<CombatEntity, GameObject>();

        private Dictionary<CombatEntity, GameObject> _instantiatedBreakGUI = new Dictionary<CombatEntity, GameObject>();


        [SerializeField]
        private GameObject _vfxPrefabTemplate;

        private GameObject instantiatedVFXCircle;

        Transform _parent;

        public override void Initialize(Transform argc, Null argv = null, Null argg = null)
        {
            instantiatedVFXCircle = MonoBehaviour.Instantiate(_vfxPrefabTemplate, _vfxPrefabTemplate.transform.position, Quaternion.identity);
            _instantiatedVFXCircle = new Dictionary<CombatEntity, GameObject>();
            _parent = argc;

            SetInit(true);
        }

        public void HideTargetPointer(CombatEntity combatEntity)
        {
            _instantiatedTargetGUI[combatEntity].SetActive(false);
            _instantiatedVFXCircle[combatEntity].SetActive(false);
            _instantiatedBreakGUI[combatEntity].SetActive(false);
        }

        public void HideAllPointer()
        {
             foreach (var key in _instantiatedTargetGUI.Keys)
                _instantiatedTargetGUI[key].SetActive(false);

            foreach (var key in _instantiatedTargetGUI.Keys)
                _instantiatedVFXCircle[key].SetActive(false);
            
            foreach (var key in _instantiatedBreakGUI.Keys)
                _instantiatedBreakGUI[key].SetActive(false);

        }

        public void SelectTargetPointer(List<CombatEntity> entities)
        {
            //Debug.Log("enalbe target selection to " + combatEntity);
            if (!IsInit)
                throw new Exception("TargetSelectionGUI never been Initialized");

            //Disable old selection GUI 
            foreach (var key in _instantiatedTargetGUI.Keys)
            {
                if (! entities.Contains(key))
                    _instantiatedTargetGUI[key].SetActive(false);
            }
            foreach (var key in _instantiatedTargetGUI.Keys)
            {
                if (! entities.Contains(key))
                    _instantiatedVFXCircle[key].SetActive(false);
            }
            foreach (var key in _instantiatedBreakGUI.Keys)
            {
                if (!entities.Contains(key))
                    _instantiatedBreakGUI[key].SetActive(false);
            }

            foreach (var combatEntity in entities)
            {
                if (!_instantiatedTargetGUI.ContainsKey(combatEntity))
                {
                     _instantiatedTargetGUI.Add(combatEntity, targetGUI.InstantiateTargetGUI(UISpaceSingletonHandler.ObjectToUISpace(combatEntity.CombatEntityAnimationHandler.GetGUISpawnTransform()) , _parent));
                }

                if (!_instantiatedTargetGUI[combatEntity].activeSelf)
                {
                    _instantiatedTargetGUI[combatEntity].SetActive(true);
                }

                if (!_instantiatedVFXCircle.ContainsKey(combatEntity))
                {
                    _instantiatedVFXCircle.Add(combatEntity, MonoBehaviour.Instantiate(_vfxPrefabTemplate, _vfxPrefabTemplate.transform.position, Quaternion.identity));
                    Vector3 circleTranform = UISpaceSingletonHandler.ObjectToUISpace(combatEntity.CombatEntityAnimationHandler.GetGUISpawnTransform());//  combatEntity.CombatEntityAnimationHandler.GetGUISpawnPos();
                    _instantiatedVFXCircle[combatEntity].transform.position = new Vector3(circleTranform.x, 0.03f, circleTranform.z);
                }

                if (!_instantiatedVFXCircle[combatEntity].activeSelf)
                    _instantiatedVFXCircle[combatEntity].SetActive(true);

                targetGUI.SetTargetGUIPosition(_instantiatedTargetGUI[combatEntity], UISpaceSingletonHandler.ObjectToUISpace(combatEntity.CombatEntityAnimationHandler.GetGUISpawnTransform()));
            }
        }

        public void SelectBreakTarget(CombatEntity combatEntity)
        {
            if (!_instantiatedBreakGUI.ContainsKey(combatEntity))
            {
                _instantiatedBreakGUI.Add(combatEntity, targetGUI.InstantiateBreakGUI(combatEntity.CombatEntityAnimationHandler.GetGUISpawnTransform().position, _parent));

                Vector3 location = UISpaceSingletonHandler.ObjectToUISpace(combatEntity.CombatEntityAnimationHandler.GetGUISpawnTransform());
                _instantiatedBreakGUI[combatEntity].transform.position =  location ;// new Vector3(circleTranform.x, 0.03f, circleTranform.z);
            }

            if (!_instantiatedBreakGUI[combatEntity].activeSelf)
            {
                _instantiatedBreakGUI[combatEntity].SetActive(true);
            }

            targetGUI.SetBreakGUIPosition(_instantiatedBreakGUI[combatEntity], UISpaceSingletonHandler.ObjectToUISpace(combatEntity.GetComponent<CombatEntityAnimationHandler>().GetGUISpawnTransform() ));
        }

        public void EndSelectionScheme()
        {
            HideAllPointer(); 
            //foreach (var key in _instantiatedTargetGUI.Keys)
            //{
            //    if (!_poolTargetGUI.Contains(_instantiatedTargetGUI[key]))
            //        _poolTargetGUI.Add(_instantiatedTargetGUI[key]);
            //}



        }
    }
}