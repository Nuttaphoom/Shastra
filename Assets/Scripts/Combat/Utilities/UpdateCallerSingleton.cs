


using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEditorInternal;
using UnityEngine;
 
namespace Vanaring_DepaDemo
{
    public class UpdateCallerSingleton : MonoBehaviour
    {
        private void Update()
        {
            CentralInputReceiver.Instance().TickUpdate() ; 
        }
    }
}