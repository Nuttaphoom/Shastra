using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine; 

namespace Vanaring
{
    /// <summary>
    /// EntityPositionManager will change position of the "Root GameObject" of that entity 
    /// </summary>
    public class EntityPositionManager : MonoBehaviour  
    {
        [Serializable]
        private struct EnemyOccupierData
        {
            public StandingLocationOccupierData StandingLocationData;
            [Header("Enemy size need to match to use this location")]
            public int EnemySize;
        }
        #region Singleton 
        private static EntityPositionManager _instance ; 
        public static EntityPositionManager Instance
        {
            get
            {
                if (_instance == null ) 
                    FindObjectOfType<EntityPositionManager>();

                if (_instance == null)
                    throw new Exception("Object of type EntityPositionManager could not be found"); 

                return _instance;
            }
        }


       

        #endregion

        [Header("Transform is easier assiged manually, no need to create Tag for them")]
        [SerializeField]
        private StandingLocationOccupierData _allyMainStandLocation ;

        [SerializeField]
        private List<StandingLocationOccupierData> _allyFullTeamLocation ;  
        
        [SerializeField]
        private List<EnemyOccupierData> _enemyOccupierData; 

        private int _currentEnemySize = -1;

        public int CurrentEnemySize
        {
            get
            {
                if (_currentEnemySize == -1)
                    throw new Exception("Current Enemy Size hasn't been set");

                return _currentEnemySize;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
                throw new System.Exception("There are more than one instance of ntityPositionManager in the scene");

            _instance = this;

            CombatReferee.Instance.SubOnCombatPreparation(Initialization);
            

        }
        private void Initialization(Null DontUse)
        {
            foreach (ECompetatorSide side in Enum.GetValues(typeof(ECompetatorSide)))
            {
                foreach (var entity in CombatReferee.Instance.GetCompetatorsBySide(side))
                {
                    BindActionEvent(entity);
                }
            }
            TargetSelectionFlowControl.Instance.SubOnTargetSelectionEnter(OnTargetSelectionStart_AdjustAllyPosition);
            TargetSelectionFlowControl.Instance.SubOnTargetSelectionEnd(OnTargetSelectionEnd_ReturnOccupiedAllyPosition);
            CombatReferee.Instance.SubOnCompetitorEnterCombat(BindActionEvent);

        }
        #region Observer Methods
        private void OnTargetSelectionEnd_ReturnOccupiedAllyPosition(TargetSelectingData data)
        {
            if (!data.targetSelector.TargetAllyTeam || CombatReferee.Instance.GetCompetatorSide(data.caster) != ECompetatorSide.Ally)
                return;

            foreach (var entity in CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Ally))
            {                   

                if (data.caster == entity && ! data.isSucesfullySelected)
                {
                    Debug.Log("Auto Occupie new location");

                    OccupieLocation(ECompetatorSide.Ally, 0, entity);

                    continue; 
                }
                
                ReleasePosition(entity);
                 
            }

        }

        private void OnTargetSelectionStart_AdjustAllyPosition(TargetSelectingData data)
        {
            if (!data.targetSelector.TargetAllyTeam || CombatReferee.Instance.GetCompetatorSide(data.caster) != ECompetatorSide.Ally)
                return;

            int index = 0; 
            foreach (var entity in CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Ally))
            {
                index++; 
                OccupieLocation(ECompetatorSide.Ally, index, entity);
            }
        }

        private void BindActionEvent(CombatEntity entity)
        {
            entity.SubOnTakeControlEvent(OnEntityTakeControl);
        }

        private void OnEntityTakeControl(CombatEntity entity)
        {
            
            RelocateEntityToitsOccupiedPosition(); 
        }

        #endregion

        /// <summary>
        /// if EnemySize hasn't been specified 
        /// </summary>
        /// <param name="enemySize"></param>
        /// <returns></returns>
        private List<StandingLocationOccupierData> GetEnemyStandingLocations(int enemySize = -1)
        {
            List<StandingLocationOccupierData> ret = new List<StandingLocationOccupierData>(); 
            foreach (var enemyOccupieData in _enemyOccupierData)
            {
                if (enemyOccupieData.EnemySize == enemySize)
                {
                    ret.Add(enemyOccupieData.StandingLocationData) ; 
                }
            }

            return ret; 
        }
       
        public void SetNewEnemyCurrentSize(int newSize)
        {
            if (_currentEnemySize == newSize)
                return ; 

            if (_currentEnemySize == -1)
            {
                _currentEnemySize = newSize;
                return;
            }


            //TODO :a if the enemy size changed, change all of the position of the enemy, reattach prev one and attach to the new location

            List<StandingLocationOccupierData> oldLocationData = GetEnemyStandingLocations(_currentEnemySize);

            _currentEnemySize = newSize; 
            
            int i = 0; 
            foreach (var data in oldLocationData)
            {
                if (data.EntityStandingHere == null)
                    continue;

                CombatEntity combatEntity = data.EntityStandingHere;
                OccupieLocation(ECompetatorSide.Hostile, i, combatEntity);
                i++;
            }
        }
        

        #region Occupy & Release Location 
       
        public void OccupieLocation(ECompetatorSide side, int index,CombatEntity entity )
        {
            if (IsThisEntityOccupyLocation(entity) != null)
            {
                ReleasePosition(entity);
            }

            StandingLocationOccupierData data = null ;

            if (side == ECompetatorSide.Ally)
            {
                if (index == 0)
                {
                    data = _allyMainStandLocation;
                }else
                {
                    data = _allyFullTeamLocation[index - 1]; 
                }

                if (data.EntityStandingHere != null)
                    ReleasePosition(data.EntityStandingHere);

                data.EntityStandingHere = entity;
            }
            else
            {

                var enemyOccupation = GetEnemyStandingLocations(CurrentEnemySize);
                if (enemyOccupation[index].EntityStandingHere == null)
                {
                    enemyOccupation[index].EntityStandingHere = entity;
                    data = enemyOccupation[index];
                }

            }

            entity.GetComponent<CombatEntityAnimationHandler>().ShowVisualMesh();
            entity.transform.position = data.Location.transform.position ;
            entity.transform.forward = data.Location.transform.forward; 

            return  ; 

        } 
        public void OccupieAnyValidLocation(ECompetatorSide side, CombatEntity entity   )
        {
            
            if (IsThisEntityOccupyLocation(entity) != null) {
                ReleasePosition(entity);
            }

            StandingLocationOccupierData validLocation = null ;

            if (side == ECompetatorSide.Ally)
            {
                if (_allyMainStandLocation.EntityStandingHere != null)
                    ReleasePosition(_allyMainStandLocation.EntityStandingHere);

                validLocation = _allyMainStandLocation;
            }
            else
            {
                foreach (var data in GetEnemyStandingLocations(_currentEnemySize))
                {
                    if (data.EntityStandingHere == null)
                    {
                        validLocation = data;
                        break;
                    }
                }
            }

            if (validLocation == null)
                throw new Exception("validLocation is null");

            entity.GetComponent<CombatEntityAnimationHandler>().ShowVisualMesh();
            validLocation.EntityStandingHere = entity;
            entity.transform.position = validLocation.Location.position;

            entity.transform.forward = validLocation.Location.forward; 
         }

        public void ReleasePosition(CombatEntity entity)
        {
            foreach (var data in GetAllAllyStandingLocation( ))
            {
                if (data.EntityStandingHere == entity)
                {
                    //data.EntityStandingHere.GetComponent<CombatEntityAnimationHandler>().HideVisualMesh();
                    data.EntityStandingHere = null;
                    return;
                }
            }
           
            foreach (var data in GetEnemyStandingLocations(_currentEnemySize))
            {
                if (data.EntityStandingHere == entity)
                {
                    data.EntityStandingHere = null;
                    return;
                }
            } 
        }


        public void EnalbeFullAllyTeamCamera()
        {

        }
        
        #endregion 

        private StandingLocationOccupierData IsThisEntityOccupyLocation(CombatEntity entity)
        {
            foreach (var data in GetAllAllyStandingLocation())
            {
                if (data.EntityStandingHere == null)
                    continue; 

                if (data.EntityStandingHere == entity)
                {
                    return data;
                }
            }

            foreach (var data in GetEnemyStandingLocations(_currentEnemySize))
            {
                if (data.EntityStandingHere == entity)
                {
                    return data ;
                }
            }

            return null; 
        }

        private void RelocateEntityToitsOccupiedPosition()
        {
            foreach (var occupiedData in GetAllOccupiedLocation())
            {
                occupiedData.EntityStandingHere.transform.position = occupiedData.Location.position;
                occupiedData.EntityStandingHere.transform.rotation= occupiedData.Location.rotation;
            }
        }

        private List<StandingLocationOccupierData> GetAllAllyStandingLocation()
        {
            List<StandingLocationOccupierData> ret = new List<StandingLocationOccupierData>();

            ret.Add(_allyMainStandLocation); 

            foreach (var data in _allyFullTeamLocation)
            {
                ret.Add(data); 
            } 

            return ret;
        } 

        private List<StandingLocationOccupierData> GetAllOccupiedLocation()
        {
            List<StandingLocationOccupierData> ret = new List<StandingLocationOccupierData>(); 

            foreach (var standingData in GetAllAllyStandingLocation() )
            {
                if (standingData.EntityStandingHere == null)
                    continue;

                ret.Add(standingData);
            }

            foreach (var enemyOccupiation in _enemyOccupierData)
            {
                if (enemyOccupiation.EnemySize != _currentEnemySize)
                    continue;

                if (enemyOccupiation.StandingLocationData.EntityStandingHere == null)
                    continue;

                ret.Add(enemyOccupiation.StandingLocationData); 
            }

            return ret; 
        }

        [Serializable]
        public class StandingLocationOccupierData
        {
            [HideInInspector] 
            public CombatEntity EntityStandingHere;
            public Transform Location; 
        }
    }
}
