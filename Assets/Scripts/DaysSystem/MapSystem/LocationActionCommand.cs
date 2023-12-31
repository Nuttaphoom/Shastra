using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using NaughtyAttributes; 

namespace Vanaring
{
    [Serializable]
    public class LocationActionCommandRegister
    {
        [SerializeField]
        private ELoadLocationCommandType _commandType;

        [SerializeField,AllowNesting,NaughtyAttributes.ShowIf("_commandType", ELoadLocationCommandType.LoadLocation)] 
        private LoadLocationCommand _loadLocationCommand;
        
        //[SerializeField,AllowNesting, NaughtyAttributes.ShowIf("_commandType", ELoadLocationCommandType.LoadCutscene)]
        //private LoadCutsceneCommand _loadCutsceneCommand;
    
        public BaseLocationActionCommand FactorizeLocationSelectionCommand()
        {
            if (_commandType == ELoadLocationCommandType.LoadLocation)
            {
                return new LoadLocationCommand(_loadLocationCommand); 
            } 

            throw new Exception("_commandType is never set "); 
        }
    }

    public enum ELoadLocationCommandType
    {
        None,
        LoadLocation,
       
    }

    public abstract class BaseLocationActionCommand
    {
        [SerializeField]
        private Sprite _actionIconAsset;

        public BaseLocationActionCommand(BaseLocationActionCommand copied)
        {
            _actionIconAsset = copied._actionIconAsset;
        }

        public Sprite GetActionIconSprite => _actionIconAsset;

        public abstract void ExecuteCommand () ;
    }


    [Serializable]
    public abstract class LoadSceneCommand : BaseLocationActionCommand
    {
        public LoadSceneCommand(LoadSceneCommand copied): base(copied)
        {

        }
        [SerializeField]
        protected SceneDataSO _sceneField;
    }
    


    [Serializable]
    public class LoadLocationCommand : LoadSceneCommand
    {
        public LoadLocationCommand(LoadLocationCommand copied): base(copied)
        {
            _sceneField = copied._sceneField; 
        }
        public override void ExecuteCommand()
        { 
            PersistentSceneLoader.Instance.LoadLocation<LoadLocationCommandData>(_sceneField, new LoadLocationCommandData()) ; 
        }

        public struct LoadLocationCommandData
        {

        }
    }
}
