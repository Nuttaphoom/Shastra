using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

namespace Vanaring
{
    public class EntityMeshManipulator : MonoBehaviour
    {
        private List<GameObject> _allyEntityMesh;
        private List<GameObject> _enemyEntityMesh;

        private void Awake()
        {
            CombatReferee.Instance.SubOnCombatPreparation(Initialization); 
        }

        private void Initialization(Null DontUse)
        {
            foreach (ECompetatorSide side in Enum.GetValues(typeof(ECompetatorSide)))
            {
                foreach (var entity in CombatReferee.Instance.GetCompetatorsBySide(side))
                {
                    BindPerfromActionEvent(entity); 
                }
            }

            CombatReferee.Instance.SubOnCompetitorEnterCombat(BindPerfromActionEvent);

        }

        private void BindPerfromActionEvent(CombatEntity entity)
        {
            entity.SubOnPerformAction(OnEntityPerformAction);
            entity.SubOnPostPerformAction(OnEntityPostPerfromAction);
        }

        private void OnEntityPerformAction(EntityActionPair entityActionPair)
        {
            List<CombatEntity> entityException = new List<CombatEntity>();
            entityException.Add(entityActionPair.PerformedAction.GetActionCaster());
            foreach (var entity in entityActionPair.PerformedAction.GetActionTargets())
                entityException.Add(entity);

            TemporaryHideAllEntityMesh(entityException) ;
        }

        private void OnEntityPostPerfromAction(EntityActionPair entityActionPair)
        {
            RestoreTempHiddenEntities(); 
        }


        //public List<GameObject> GetEntityMesh(ECompetatorSide side)
        //{   
        //    List<GameObject> mesh = new List<GameObject>();
           
        //    List<CombatEntity> entities = CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Ally);
           
        //    foreach (var entity in entities)
        //    {
        //        mesh.Add(entity.GetComponent<CombatEntityAnimationHandler>().GetVisualMesh());
        //    }

        //    return mesh;  
        //}

        //public List<GameObject> GetAllEntityMesh()
        //{
        //    List<GameObject> meshes = new List<GameObject>(); 
        //    foreach (ECompetatorSide side in Enum.GetValues(typeof(ECompetatorSide)))
        //    {
        //        foreach (var mesh in GetEntityMesh(side))
        //        {
        //            meshes.Add(mesh); 
        //        }
        //    }

        //    return meshes; 
        //}

        private List<CombatEntity> GetAllCompetators()
        {
            List<CombatEntity> ret = new List<CombatEntity>();
            foreach (ECompetatorSide side in Enum.GetValues(typeof(ECompetatorSide)))
            {
                foreach(var entity in CombatReferee.Instance.GetCompetatorsBySide(side) ) {
                    ret.Add(entity); 
                }
            }
            return ret;
        }

        private List<CombatEntity> _lastHideEntities = new List<CombatEntity>() ; 
        public void TemporaryHideAllEntityMesh (List<CombatEntity> entityException = null)
        {
            foreach (var mesh in GetAllCompetators())
            {
                if (entityException != null && entityException.Contains(mesh)) 
                    continue;

                _lastHideEntities.Add(mesh);

                mesh.GetComponent<CombatEntityAnimationHandler>().HideVisualMesh() ;//.SetActive(false);
            }
        }

        public void RestoreTempHiddenEntities()
        {
            foreach (var entity in _lastHideEntities)
            {
                entity.GetComponent<CombatEntityAnimationHandler>().ShowVisualMesh() ;
            }
        }

        public void ShowAllEntitMesh(List<CombatEntity> entityException = null)
        {
            foreach (var mesh in GetAllCompetators())
            {
                if (entityException != null && entityException.Contains(mesh))
                    continue;

                mesh.GetComponent<CombatEntityAnimationHandler>().ShowVisualMesh();//.SetActive(false);
            }
        }
    }
}
