using System;
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

    private SpellAbilityRuntime _latestSelectedSpell; 

    private RuntimeEffectFactorySO _latestAction ; 
    
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
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("W"); 
                _currentSelectIndex = (_currentSelectIndex + 1) % _validTargets.Count;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("S");
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
            _latestSelectedSpell = null; 
        }
        return (_latestAction, _selectedTarget);
    }

    //TODO : Properly separate Spell action so that we don't need to return the spell like this
    public SpellAbilityRuntime IsLatedActionSpell()
    {
        return _latestSelectedSpell ; 
    }
 

    public bool PrepareAction()
    {
        return _activlySelecting && _latestAction != null ; 
    }
    
    public  IEnumerator InitializeTargetSelectionScheme(CombatEntity caster, RuntimeEffectFactorySO action, bool randomTarget = false)
    {

        if (_activlySelecting)
            throw new Exception("Try to active selection scheme while it is already active");

        ColorfulLogger.LogWithColor("Initialize Target Selection", Color.green); 
        _activlySelecting = true;
        _latestAction = null;


        ValidateData();
        AssignPossibleTargets(caster, action); 

        while (_selectedTarget.Count < action.TargetSelect.MaxTarget )
        {
            if (randomTarget)
            {
                _currentSelectIndex = UnityEngine.Random.Range(0, _validTargets.Count);
                _selectedTarget.Add(_validTargets[_currentSelectIndex]);
                ColorfulLogger.LogWithColor("AI Action target is " + _validTargets[_currentSelectIndex], Color.yellow);  
                _validTargets.RemoveAt(_currentSelectIndex); 
                
                continue ;
            }

            yield return new WaitForEndOfFrame();
        }

        _latestAction = action;

        yield return null ;  
    }
    
    public IEnumerator InitializeSpellTargetSelectionScheme(CombatEntity caster, SpellAbilityRuntime spell, bool randomTarget = false)
    {
        _latestSelectedSpell = spell;

        yield return InitializeTargetSelectionScheme(caster,spell.EffectFactory,randomTarget) ;
         

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
