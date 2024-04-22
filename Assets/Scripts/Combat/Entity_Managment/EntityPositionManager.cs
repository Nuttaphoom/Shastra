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


        private void Awake()
        {
            if (_instance != null && _instance != this)
                throw new System.Exception("There are more than one instance of ntityPositionManager in the scene"); 

            _instance = this;

        }

        #endregion

        [Header("Transform is easier assiged manually, no need to create Tag for them")]
        [SerializeField]
        private List<StandingLocationOccupierData> _allyStandingTransform ;

        [SerializeField]
        private List<EnemyOccupierData> _enemyOccupierData;

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

        [SerializeField]
        private Transform _arenaCenterTransform;


        private int _currentEnemySize = -1 ;
        public int CurrentEnemySize
        {
            get
            {
                if (_currentEnemySize == -1)
                    throw new Exception("Current Enemy Size hasn't been set");

                return _currentEnemySize; 
            }
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
        
        
       
        //public List<StandingLocationOccupierData> GetAllOccupiedLocation(ECompetatorSide side, int enemySize = -1)
        //{
        //    List<StandingLocationOccupierData> ret = new List<StandingLocationOccupierData>(); 
        //    if (side == ECompetatorSide.Ally)
        //    {
        //        foreach (var data in _allyStandingTransform)
        //        {
        //            if (data.EntityStandingHere != null)
        //            {
        //                ret.Add(data);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (enemySize == -1)
        //            throw new Exception("Enemy Size is " + enemySize) ;
                    
        //        foreach (var data in GetEnemyStandingLocations(enemySize))
        //        {
        //            if (data.EntityStandingHere != null)
        //            {
        //                ret.Add(data);
        //            }
        //        }
        //    }

        //    return ret; 
        //}

        //public Transform GetAllyStandLocationTransform(int index)
        //{
        //    return _allyStandingTransform[index].Location;
        //}

        //public Transform GetEnemyStandLocationTransform(int index)
        //{
        //    return _enemyStandingTransform[index].Location;
        //}

        public Transform GetLocationFromCombatEntity(CombatEntity entity)
        {
            foreach (var standingLocationData in _allyStandingTransform)
            {
                if (standingLocationData.EntityStandingHere.CombatCharacterSheet.CharacterName == entity.CombatCharacterSheet.CharacterName)
                    return standingLocationData.Location; 
            }


            for (int i = 1; i <= 6; i++)
            {
                foreach (var standingLocationData in GetEnemyStandingLocations(i))
                {
                    if (standingLocationData.EntityStandingHere.CombatCharacterSheet.CharacterName == entity.CombatCharacterSheet.CharacterName)
                        return standingLocationData.Location;
                }
            }
            throw new Exception(entity.gameObject.name + " 's location can't not be found");
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
                if (_allyStandingTransform[0].EntityStandingHere != null)
                    ReleasePosition(_allyStandingTransform[0].EntityStandingHere); 
                
                _allyStandingTransform[0].EntityStandingHere = entity;
                data = _allyStandingTransform[0];
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
                foreach (var data in _allyStandingTransform)
                {
                    if (data.EntityStandingHere != null)
                        ReleasePosition(data.EntityStandingHere) ;
                    
                    validLocation = data;
                    break; 
                }
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


            validLocation.EntityStandingHere = entity;
            entity.transform.position = validLocation.Location.position;

            entity.transform.forward = validLocation.Location.forward; 
         }

        public void ReleasePosition(CombatEntity entity)
        {
            foreach (var data in _allyStandingTransform)
            {
                if (data.EntityStandingHere == entity)
                {
                    data.EntityStandingHere.GetComponent<CombatEntityAnimationHandler>().HideVisualMesh(); 
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
        #endregion 

        public StandingLocationOccupierData IsThisEntityOccupyLocation(CombatEntity entity)
        {
            foreach (var data in _allyStandingTransform)
            {
                if (data.EntityStandingHere == entity)
                {
                    return data ;
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

        [Serializable]
        public class StandingLocationOccupierData
        {
            [HideInInspector] 
            public CombatEntity EntityStandingHere;
            public Transform Location; 
        }
    }
}
