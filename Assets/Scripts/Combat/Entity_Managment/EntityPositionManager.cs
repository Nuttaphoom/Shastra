using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditorInternal;
using UnityEngine; 

namespace Vanaring
{
    public class EntityPositionManager : MonoBehaviour  
    {
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


        [Header("Transform is easier assiged manually, no need to create Tag for them")]
        [SerializeField]
        private List<StandingLocationOccupierData> _allyStandingTransform ;

        [SerializeField]
        private List<StandingLocationOccupierData> _enemyStandingTransform ;

        [SerializeField]
        private Transform _arenaCenterTransform;


        public Transform GetAllyStandLocationTransform(int index)
        {
            return _allyStandingTransform[index].Location;
        }

        public Transform GetEnemyStandLocationTransform(int index)
        {
            return _enemyStandingTransform[index].Location;
        }

        public Transform GetLocationFromCombatEntity(CombatEntity entity)
        {
            foreach (var standingLocationData in _allyStandingTransform)
            {
                if (standingLocationData.EntityStandingHere.CombatCharacterSheet.CharacterName == entity.CombatCharacterSheet.CharacterName)
                    return standingLocationData.Location; 
            }

            foreach (var standingLocationData in _enemyStandingTransform)
            {
                if (standingLocationData.EntityStandingHere.CombatCharacterSheet.CharacterName == entity.CombatCharacterSheet.CharacterName)
                    return standingLocationData.Location;
            }

            throw new Exception(entity.gameObject.name + " 's location can't not be found");
        }

        /// <summary>
        /// bool => check if the location has been occpied (invalid) 
        /// </summary>
        /// <param name="ally"></param>
        /// <param name="index"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void OccupieLocation(ECompetatorSide side, int index,CombatEntity entity)
        {
            if (IsThisEntityOccupyLocation(entity))
                return   ;

            StandingLocationOccupierData data = null ;

            if (side == ECompetatorSide.Ally)
            {
                if (_allyStandingTransform[index].EntityStandingHere == null)
                {
                    _allyStandingTransform[index].EntityStandingHere = entity; 
                    data = _allyStandingTransform[index];
                }
            }
            else
            {
                if (_enemyStandingTransform[index].EntityStandingHere == null)
                {
                    _enemyStandingTransform[index].EntityStandingHere = entity;
                    data = _enemyStandingTransform[index];
                }
           
            }

            entity.transform.position = data.Location.transform.position ;
            entity.transform.forward = data.Location.transform.forward; 

            return  ; 

        }
        public void OccupieAnyValidLocation(ECompetatorSide side, CombatEntity entity)
        {
            if (IsThisEntityOccupyLocation(entity))
                return; 

            StandingLocationOccupierData validLocation = null ; 
            if (side == ECompetatorSide.Ally)
            {
                foreach (var data in _allyStandingTransform)
                {
                    if (data.EntityStandingHere == null)
                    {
                        validLocation = data;
                        break; 
                       
                        
                    }
                }
            }else
            {
                foreach (var data in _enemyStandingTransform)
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
                    data.EntityStandingHere = null;
                    return;
                }
            }

            foreach (var data in _enemyStandingTransform)
            {
                if (data.EntityStandingHere == entity)
                {
                    data.EntityStandingHere = null;
                    return;
                }
            }


        }

        public bool IsThisEntityOccupyLocation(CombatEntity entity)
        {
            foreach (var data in _allyStandingTransform)
            {
                if (data.EntityStandingHere == entity)
                {
                    return true ;
                }
            }

            foreach (var data in _enemyStandingTransform)
            {
                if (data.EntityStandingHere == entity)
                {
                    return true ;
                }
            }

            return false; 
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
