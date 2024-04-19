using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vanaring
{
    public class EntityMeshManipulator : MonoBehaviour
    {
        private List<GameObject> _allyEntityMesh;
        private List<GameObject> _enemyEntityMesh; 


        public List<GameObject> GetEntityMesh(ECompetatorSide side)
        {   
            List<GameObject> mesh = new List<GameObject>();
           
            List<CombatEntity> entities = CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Ally);
           
            foreach (var entity in entities)
            {
                mesh.Add(entity.GetComponent<CombatEntityAnimationHandler>().GetVisualMesh());
            }

            return mesh;  
        }

        public List<GameObject> GetAllEntityMesh()
        {
            List<GameObject> meshes = new List<GameObject>(); 
            foreach (ECompetatorSide side in Enum.GetValues(typeof(ECompetatorSide)))
            {
                foreach (var mesh in GetEntityMesh(side))
                {
                    meshes.Add(mesh); 
                }
            }

            return meshes; 
        }

        public void HideAllEntityMesh ()
        {
            foreach (var mesh in GetAllEntityMesh())
            {
                mesh.SetActive(false);
            }
        }

        public void ShowAllEntitMesh()
        {
            foreach (var mesh in GetAllEntityMesh())
            {
                mesh.SetActive(true);
            }
        }
    }
}
