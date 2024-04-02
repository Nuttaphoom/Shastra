using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring
{
    public class MissionNodeObject : MonoBehaviour
    {
        private BaseMissionNode baseNode;
        [SerializeField] private GameObject floorGraphic;
        [SerializeField] private Image iconShown;

        private bool isVisisted;
        private bool isVisisting;
        //private BaseMissionNode.VisitationState state;

        private void Start()
        {
            isVisisted = baseNode.IsThisNodeVisited;
            isVisisting = baseNode.IsCurrentlyVisiting;
        }
    }
}
