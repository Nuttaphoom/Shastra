using PixelCrushers.DialogueSystem;
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

        [SerializeField]
        private EntityInpectWindowGUI _entityInspectWindowGUI; 

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
                    BindEntityEvent(entity); 
                }
            }

            CombatReferee.Instance.SubOnCompetitorEnterCombat(BindEntityEvent);
            CombatReferee.Instance.SubOnNewRoundBegin(OnNewRoundBegin);

            DirectorManager.Instance.SubOnPlayTimelineWithActor(PrepareEntityMeshForTimelineAnimation);

            TargetSelectionFlowControl.Instance.SubOnTargetSelectionEnd(OnTargetSelectionEnd_HideAllyVisualMesh);

            if (_entityInspectWindowGUI == null)
                throw new Exception("_entityInspectWindowGUI hasn't never been assigned");

            _entityInspectWindowGUI.SubOnEntityInspect(OnEntityInspection);
            _entityInspectWindowGUI.SubOnCloseInspectWindow(OnEntityInspectionEnd);
        }

        private CombatEntity lastInspectEntity; 
        private void OnEntityInspection(CombatEntity inspectOnThisEntity)
        {
            HideAllEntityMesh();
            ShowEntityMesh(new List<CombatEntity>() { inspectOnThisEntity });
            inspectOnThisEntity.GetComponent<EntityCameraManager>().EnableFaceCamera();

            lastInspectEntity = inspectOnThisEntity; 
        }

        private void OnEntityInspectionEnd(Null n)
        {
            HideAllEntityMesh(); 

            ShowAllEntitMesh(ECompetatorSide.Hostile);
            lastInspectEntity.GetComponent<EntityCameraManager>().DisableAllAttachedCamera(); 

            ShowEntityMesh(new List<CombatEntity>() { CombatReferee.Instance.GetCurrentActor() } ) ;

            (CombatReferee.Instance.GetCurrentActor() as ControlableEntity).SetUpCameraAndPositio();  

        }

        private void PrepareEntityMeshForTimelineAnimation((List<CombatEntity>, ActionTimelinePrefab) data)
        {
            List<CombatEntity> actors = data.Item1;
            //If we have actiontimeline prefab data 
          
            HideAllEntityMesh(actors);
            ShowEntityMesh(actors);

            
            if (data.Item2 != null)
            {
                ActionTimelinePrefab timeline = data.Item2;

                //If use initial look at, don't restore look at 
                if (timeline.IsThisTimelineUseInitialCamera)
                {
                    return;
                }
            }

            RestoreRotateMeshLookAt( );
        }

        private void OnTargetSelectionEnd_HideAllyVisualMesh(TargetSelectingData data)
        {
            if (!data.targetSelector.TargetAllyTeam || CombatReferee.Instance.GetCompetatorSide(data.caster) != ECompetatorSide.Ally)
                return;

            List<CombatEntity> entityException = new List<CombatEntity>();
            entityException.Add(data.caster); 

            HideAllEntityMesh(ECompetatorSide.Ally,entityException);

        }
        private void BindEntityEvent(CombatEntity entity)
        {
            entity.SubOnPerformAction(OnEntityPerformAction);
            entity.SubOnTakeControlEvent(OnEntityTakeControl);
        }

        private void OnEntityTakeControl(CombatEntity entity)
        {
            List<CombatEntity> entitiesTakeControl = new List<CombatEntity>() {  entity };
            
            if (CombatReferee.Instance.GetCompetatorSide(entity) == ECompetatorSide.Ally)
                RestoreRotateMeshLookAt();

            if (CombatReferee.Instance.GetCompetatorSide(entity) == ECompetatorSide.Ally)
            {

                ShowEntityMesh(entitiesTakeControl);
                ShowAllEntitMesh(ECompetatorSide.Hostile) ;
                HideAllEntityMesh(ECompetatorSide.Ally, entitiesTakeControl); 
                
                RotateMeshToLookToThisPosition(entity.transform.position, ECompetatorSide.Hostile); 
            }
        }

        private void OnEntityPerformAction(EntityActionPair entityActionPair)
        {
            List<CombatEntity> entityPerformAction = new List<CombatEntity>();
            entityPerformAction.Add(entityActionPair.PerformedAction.GetActionCaster()); 

            foreach (var entity in entityActionPair.PerformedAction.GetActionTargets())
                entityPerformAction.Add(entity);

            //PrepareEntityMeshForTimelineAnimation((entityPerformAction, null));

        }

        private void OnNewRoundBegin(ECompetatorSide newRoundSide)
        {
            //if (newRoundSide  == ECompetatorSide.Ally)
            //    RestoreRotateMeshLookAt();

            if (newRoundSide == ECompetatorSide.Hostile)
            {
                HideAllEntityMesh(ECompetatorSide.Ally);
            }
            
            //ShowAllEntitMesh();
        }

        private void RotateMeshToLookToThisPosition(Vector3 worldPosition, ECompetatorSide side)
        {
            foreach (var entity in GetAllCompetators(side))
            {
                entity.GetComponent<CombatEntityAnimationHandler>().RotateMeshLookAtToThisPosition(worldPosition);
            }
        }

        private void RestoreRotateMeshLookAt()
        {
            foreach (var entity in GetAllCompetators())
            {
                entity.GetComponent<CombatEntityAnimationHandler>().RestoreLookAt(); 
            }
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
        private List<CombatEntity> GetAllCompetators(ECompetatorSide side)
        {
            List<CombatEntity> ret = new List<CombatEntity>();
            
            foreach (var entity in CombatReferee.Instance.GetCompetatorsBySide(side))
            {
                ret.Add(entity);
            }
            
            return ret;
        }
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

        public void HideAllEntityMesh (ECompetatorSide side, List<CombatEntity> entityException = null)
        {
            
            foreach (var mesh in GetAllCompetators(side))
            {
                if (entityException != null)
                {
                    if (entityException.Contains(mesh))
                        continue;
                }

                mesh.GetComponent<CombatEntityAnimationHandler>().HideVisualMesh() ;//.SetActive(false);
            }
        }
        public void HideAllEntityMesh(List<CombatEntity> entityException = null)
        {
            foreach (var mesh in GetAllCompetators())
            {
                if (entityException != null && entityException.Contains(mesh))
                    continue;

                mesh.GetComponent<CombatEntityAnimationHandler>().HideVisualMesh();//.SetActive(false);
            }
        }

        public void ShowEntityMesh(List<CombatEntity> entityToShow)
        {

            foreach (var entity in entityToShow)
            {
                entity.GetComponent<CombatEntityAnimationHandler>().ShowVisualMesh();//.SetActive(false);
            }
        }
 

        public void ShowAllEntitMesh(ECompetatorSide side)
        {
            foreach (var mesh in GetAllCompetators(side))
            {
                mesh.GetComponent<CombatEntityAnimationHandler>().ShowVisualMesh();//.SetActive(false);
            }
        }
    }
}
