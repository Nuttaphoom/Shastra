using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    public class ObjectFindingTool
    {
        public static List<GameObject> QueryObjectInChildren(Transform parent, string tag)
        {
            List<GameObject> ret = new List<GameObject>();

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);

                //if (child.CompareTag(tag))
                //    ret.Add(child.gameObject);

                foreach (GameObject obj in RecursiveSetUpPivot(child, tag))
                {
                    ret.Add(obj);
                }
            }

            return ret;
        }
        private static List<GameObject> RecursiveSetUpPivot(Transform child, string tag)
        {
            List<GameObject> ret = new List<GameObject>();

            if (child.CompareTag(tag))
                ret.Add(child.gameObject);

            for (int i = 0; i < child.transform.childCount; i++)
            {
                foreach (GameObject obj in RecursiveSetUpPivot(child.GetChild(i), tag))
                {
                    ret.Add(obj);
                }
            }

            return ret;
        }

    }
}
