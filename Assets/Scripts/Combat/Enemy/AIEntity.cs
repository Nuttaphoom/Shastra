using Cinemachine;
using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Numerics;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;

namespace Vanaring
{
    //[RequireComponent(typeof(BaseEntityBrain))]
    public class AIEntity : CombatEntity
    {
        [SerializeField]
        private AIBehaviorHandler _aibehaviorHandler;


    }
}
