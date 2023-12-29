using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class Arm_SceneLoaderTest : MonoBehaviour
    {
        [SerializeField]
        private LoadClassroomCommand _loadLocationCommand;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _loadLocationCommand.ExecuteCommand(); 
            }
        }

    }
}
