
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Vanaring_DepaDemo;

[Serializable]
public class TargetSelectionGUI  : RequireInitializationHandler<Transform,Null,Null>
{
    [SerializeField]
    private TargetGUI targetGUI;

    [SerializeField]
    private Dictionary<CombatEntity, GameObject> _instantiatedTargetGUI = new Dictionary<CombatEntity, GameObject>(); 

    private List<GameObject> _poolTargetGUI = new List<GameObject>(); 

    Transform _parent ;

    public override void Initialize(Transform argc, Null argv = null, Null argg = null)
    {
        _parent = argc;
        SetInit(true) ; 
    }

    public void SelectTargetPointer ( CombatEntity combatEntity)
    {
        if (!IsInit)
        {
            throw new Exception("TargetSelectionGUI never been Initialized"); 
        }

        foreach (var key in _instantiatedTargetGUI.Keys )
        {
            if (key != combatEntity) 
                _instantiatedTargetGUI[key].SetActive(false);
        }

      
        if (!_instantiatedTargetGUI.ContainsKey(combatEntity))
        {
            if (_poolTargetGUI.Count > 0)
            {
                _instantiatedTargetGUI.Add(combatEntity , _poolTargetGUI[0]) ;
                _poolTargetGUI.RemoveAt(0); 
            } else
            {
                _instantiatedTargetGUI.Add(combatEntity, targetGUI.Init(combatEntity.CombatEntityAnimationHandler.GetGUISpawnPos(), _parent));
            }
        }

        if (!_instantiatedTargetGUI[combatEntity].activeSelf)
        {
            _instantiatedTargetGUI[combatEntity].transform.position = combatEntity.CombatEntityAnimationHandler.GetGUISpawnPos(); 
            _instantiatedTargetGUI[combatEntity].SetActive(true);
        }
    }

    public void EndSelectionScheme()
    {
        foreach (var key in _instantiatedTargetGUI.Keys)
        {
            if (! _poolTargetGUI.Contains(_instantiatedTargetGUI[key]))
                _poolTargetGUI.Add(_instantiatedTargetGUI[key]);

            _instantiatedTargetGUI[key].SetActive(false); 
        } 


        
    }
}