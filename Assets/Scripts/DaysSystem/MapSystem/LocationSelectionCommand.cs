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
    public class LocationSelectionCommandRegister
    {
        [SerializeField]
        private ELoadLocationCommandType _commandType;

        [SerializeField,AllowNesting,NaughtyAttributes.ShowIf("_commandType", ELoadLocationCommandType.LoadLocation)] 
        private LoadLocationCommand _loadLocationCommand;
        
        [SerializeField,AllowNesting, NaughtyAttributes.ShowIf("_commandType", ELoadLocationCommandType.LoadCutscene)]
        private LoadCutsceneCommand _loadCutsceneCommand;
    
        public BaseLocationSelectionCommand FactorizeLocationSelectionCommand()
        {
            if (_commandType == ELoadLocationCommandType.LoadLocation)
            {
                return new LoadLocationCommand(_loadLocationCommand); 
            }else if (_commandType == ELoadLocationCommandType.LoadCutscene)
            {
                return new LoadCutsceneCommand(_loadCutsceneCommand);
            }

            throw new Exception("_commandType is never set "); 
        }
    }

    public enum ELoadLocationCommandType
    {
        None,
        LoadLocation ,
        LoadCutscene 
    }

    public abstract class BaseLocationSelectionCommand
    {
        [SerializeField]
        private Sprite _actionIconAsset; 

        public Sprite GetActionIconSprite => _actionIconAsset;  

        public abstract void ExecuteCommand () ;
    }


    [Serializable]
    public class LoadLocationCommand : BaseLocationSelectionCommand
    {
        public LoadLocationCommand(LoadLocationCommand copied)
        {

        }
        public override void ExecuteCommand()
        {
            throw new System.NotImplementedException();
        }
    }

    [Serializable]
    public class LoadCutsceneCommand : BaseLocationSelectionCommand
    {
        public string test2; 
        public  LoadCutsceneCommand(LoadCutsceneCommand copied)
        {

        }
        public override void ExecuteCommand()
        {
            throw new System.NotImplementedException();
        }
    }
}
