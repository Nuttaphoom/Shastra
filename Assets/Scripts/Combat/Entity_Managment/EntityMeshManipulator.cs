using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vanaring
{
    public class EntityMeshManipulator : MonoBehaviour
    {



        //private void OnCombatEntityTakeControl(CombatEntity entity)
        //{
        //    if (entity is ControlableEntity)
        //    {
        //        HideAllEntityMesh(ECompetatorSide.Hostile); 
        //    }

        //}
        public List<CombatEntityAnimationHandler> GetEntityCombatEntityAnimationHandler(   )
        {
             
            List<CombatEntityAnimationHandler> mesh = new List<CombatEntityAnimationHandler>();

            foreach (ECompetatorSide side in Enum.GetValues(typeof(ECompetatorSide)))
            {
                List<CombatEntity> entities = CombatReferee.Instance.GetCompetatorsBySide(side);

                foreach (var entity in entities)
                {
                    mesh.Add(entity.GetComponent<CombatEntityAnimationHandler>());
                }

            }
            

            return mesh;
        }
        public List<CombatEntityAnimationHandler> GetEntityCombatEntityAnimationHandler(ECompetatorSide side)
        {   
            List<CombatEntityAnimationHandler> mesh = new List<CombatEntityAnimationHandler>();
           
            List<CombatEntity> entities = CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Ally);
           
            foreach (var entity in entities)
            {
                mesh.Add(entity.GetComponent<CombatEntityAnimationHandler>() );
            }

            return mesh;  
        }
        //public List<GameObject> GetAllEntityMesh(ECompetatorSide side)
        //{
        //    List<GameObject> meshes = new List<GameObject>();
             
        //    foreach (var mesh in GetEntityCombatEntityAnimationHandler(side))
        //    {
        //        mesh.hide
        //    }
             
        //    return meshes;
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

        public void HideAllEntityMesh()
        {
            foreach (var mesh in GetEntityCombatEntityAnimationHandler( ))
            {
                mesh.HideVisualMesh(); 
            }
        }
        public void HideAllEntityMesh (ECompetatorSide side)
        {
            foreach (var mesh in GetEntityCombatEntityAnimationHandler(side))
            {
                mesh.HideVisualMesh();
            }
        }
        public void ShowAllEntityMesh()
        {
            foreach (var mesh in GetEntityCombatEntityAnimationHandler())
            {
                mesh.ShowVisualMesh( );
            }
        }
        public void ShowAllEntityMesh(ECompetatorSide side)
        {
            foreach (var mesh in GetEntityCombatEntityAnimationHandler(side))
            {
                mesh.ShowVisualMesh( );
            }
        }
         
    }
}
