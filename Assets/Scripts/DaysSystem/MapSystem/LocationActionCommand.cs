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

        [SerializeField,AllowNesting,NaughtyAttributes.ShowIf("_commandType", ELoadLocationCommandType.Activity)] 
        private ActivityActionCommand _loadLocationCommand;

        [SerializeField, AllowNesting, NaughtyAttributes.ShowIf("_commandType", ELoadLocationCommandType.LectureParticipation)]
        private LectureParticipationActionCommand _lectureParticipationActionCommand;

        [SerializeField, AllowNesting, NaughtyAttributes.ShowIf("_commandType", ELoadLocationCommandType.Bonding)]
        private NPCBondingActionCommand _npcBondingActionCommand ;

        [SerializeField]
        private bool EnableAtMorning;
        [SerializeField]
        private bool EnableAtNoon;
        [SerializeField]
        private bool EnableAtNight;
        
        public BaseLocationActionCommand FactorizeLocationSelectionCommand()
        {
            if (_commandType == ELoadLocationCommandType.Activity)
            {
                return new ActivityActionCommand(_loadLocationCommand);
            }
            else if (_commandType == ELoadLocationCommandType.LectureParticipation)
            {
                return new LectureParticipationActionCommand(_lectureParticipationActionCommand);
            }else if (_commandType == ELoadLocationCommandType.Bonding)
            {
                return new NPCBondingActionCommand(_npcBondingActionCommand); 
            }

            throw new Exception("_commandType is never set "); 
        }

        public bool EnableAtGivenDayTime(EDayTime daytime)
        {
            switch (daytime)
            {
                case (EDayTime.Morning):
                    if (EnableAtMorning)
                        return true;
                    break;
                case (EDayTime.Noon):
                    if (EnableAtNoon)
                        return true;
                    break;
                case (EDayTime.Night):
                    if (EnableAtNight)
                        return true;
                    break;

                default:
                    break;

            }

            return false;
        }
    }

    public enum ELoadLocationCommandType
    {
        None,
        Activity,
        LectureParticipation,
        Bonding,
       
    }

    public abstract class BaseLocationActionCommand
    {
        [Header("General Information of Event")]

        [SerializeField]
        private string _actionName;
        [SerializeField]
        private string _actionDescription;
        [SerializeField]
        private Sprite _actionIconAsset;
        

        public Sprite GetActionIconSprite => _actionIconAsset;
        public string GetActionName => _actionName;
        public string GetActionDescription => _actionDescription;

        public BaseLocationActionCommand(BaseLocationActionCommand copied)
        {
            _actionIconAsset = copied._actionIconAsset;
            _actionName = copied._actionName;
            _actionDescription = copied._actionDescription;
        }
        public abstract void ExecuteCommand () ;

    }


    //[Serializable]
    //public abstract class LoadSceneCommand : BaseLocationActionCommand
    //{
    //    [SerializeField]
    //    protected SceneDataSO _sceneField;
    //}
    


  
}
