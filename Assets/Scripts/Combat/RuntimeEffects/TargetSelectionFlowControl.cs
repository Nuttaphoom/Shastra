﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using Vanaring_DepaDemo;
 

public class TargetSelectionFlowControl : MonoBehaviour
{
    private  List<CombatEntity> _validTargets = new List<CombatEntity>();
    private List<CombatEntity> _selectedTarget = new List<CombatEntity>(); 

    public static TargetSelectionFlowControl Instance;

    private bool _activlySelecting = false;

    private int _currentSelectIndex = 0;

    private RuntimeEffectFactorySO _action; 
    
    private void Awake()
    {
        Instance = this; 
    }

    private void Update()
    {
        //TODO NOTED THAT
        // THIS WAY IS FREAKING UGLY
        //WE SHOULD CENTRALIZE THE INPUT 
        // MainInputSystem => TopOfTheRequireInputStack (stack of element that require input) and UpdateSelection should be at the top of the stack
        //maybe use IInputSomething to settle down this behavior

        if (_activlySelecting) {
            if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("D"); 
                _currentSelectIndex = (_currentSelectIndex + 1) % _validTargets.Count;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("A");
                _currentSelectIndex = (_currentSelectIndex - 1) < 0 ? _validTargets.Count - 1 : (_currentSelectIndex - 1);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                _selectedTarget.Add(_validTargets[_currentSelectIndex]);
                _validTargets.RemoveAt(_currentSelectIndex);
                if (_validTargets.Count != 0)
                    _currentSelectIndex = _currentSelectIndex % _validTargets.Count;
                
            }
        } 
    }

    public (RuntimeEffectFactorySO, List<CombatEntity>) GetLatestAction()
    {
        if (PrepareAction()) {
            _activlySelecting = false; 
        }
        return (_action, _selectedTarget);
    }

    public bool PrepareAction()
    {
        return _activlySelecting && _action != null ; 
    }
    
    public  IEnumerator InitializeTargetSelectionScheme(CombatEntity caster, RuntimeEffectFactorySO action)
    {
        if (_activlySelecting)
            throw new Exception("Try to active selection scheme while it is already active");

        _activlySelecting = true;
        _action = null; 

        ValidateData();
        AssignPossibleTargets(caster, action); 

        while (_selectedTarget.Count < action.TargetSelect.MaxTarget )
        {
            
            yield return new WaitForEndOfFrame();
        }


        _action = action;

  

        yield return null;  
    }
    
    private void AssignPossibleTargets(CombatEntity caster, RuntimeEffectFactorySO action)
    {
        ECompetatorSide eCompetatorSide = CombatReferee.instance.GetCharacterSide(caster);

        if (action.TargetSelect.TargetOppose)
            eCompetatorSide = (ECompetatorSide)(((int)eCompetatorSide + 1) % 2);

        foreach (CombatEntity target in CombatReferee.instance.GetCompetatorsBySide(eCompetatorSide))
            _validTargets.Add(target);
    }
    private void ValidateData()
    {
        _validTargets = new List<CombatEntity>();
        _selectedTarget = new List<CombatEntity>();
        _currentSelectIndex = 0;
    }
}


[Serializable]
public class TargetSelector    
{
    
    private enum TargetSide
    {
        Self,Oppose
    }
    [Header("Maximum target that can be assigned to")]
    [SerializeField]
    private int _maxTargetSize = 1  ;

    public int MaxTarget => _maxTargetSize;
    [SerializeField]
    private TargetSide _targetSide = TargetSide.Self ;

    public bool TargetOppose => (_targetSide == TargetSide.Oppose); 
    //Used when the target selection is requires  
    public IEnumerator TargetSelectionCoroutine(List<CombatEntity> assignedTarget)
    {
        yield return null; 

        assignedTarget.Clear();
       
        while (assignedTarget.Count < _maxTargetSize)
        {
            CombatEntity detected;
            if ((detected = TickTargetSelected()) != null)
            {
                assignedTarget.Add(detected);    
            }

            yield return null;
        }

    }

    public CombatEntity TickTargetSelected()
    {
        //TODO :: Right now we get access from Input.getmousedown here
        //Which is not correct since we want to centralize the input systme

        CombatEntity ret = null ; 
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            List<CombatEntity> hitTarget = new List<CombatEntity>();

            foreach (var hit in Physics.SphereCastAll(ray, 10.0f, Mathf.Infinity))
            {    
                if (hit.collider.TryGetComponent(out ret))
                {
                    break;    
                }
            }
        }

        return ret ;
    }

    public bool IsRequireSelectionQuery => _maxTargetSize > 0; 
}
