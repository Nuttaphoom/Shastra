using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Vanaring
{
    public class CombatBacklogDisplayer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _actionTabTMP ;
        [SerializeField]
        private TextMeshProUGUI _utilityTabTMP; 

        [SerializeField]
        private float _displayIntervalInSecond = 0.0f;

        [SerializeField]
        private GameObject _actionTab;

        [SerializeField]
        private GameObject _utilityTab; 

        private Queue<IEnumerator> _displayActionTabQueue = new Queue<IEnumerator>();
        private Queue<IEnumerator> _displayUtilityTabQueue = new Queue<IEnumerator>();


        public void EnqueueUtilityTab(string comment)
        {
            _displayUtilityTabQueue.Enqueue(DisplayUtilityTabCoroutine(comment));
        }

        public void EnqueueActionTab(string comment)
        {
            _displayActionTabQueue.Enqueue(DisplayActionTabCoroutine(comment));
        }

        #region UtilityTabs
       
        private void TryoToDisplayUtilityQueue()
        {
            if (_utilityTab.activeSelf)
                return;

            StartCoroutine(_displayUtilityTabQueue.Dequeue());
        }

        private IEnumerator DisplayUtilityTabCoroutine(string displayedText)
        {
            _utilityTab.SetActive(true);
            _utilityTabTMP.text = displayedText;


            yield return new WaitForSecondsRealtime(_displayIntervalInSecond);
            _utilityTab.SetActive(false);
            _utilityTabTMP.text = "";
        }
        #endregion


        #region ActionTabs
        private IEnumerator DisplayActionTabCoroutine(string displayedText)
        {
            _actionTab.SetActive(true); 
            _actionTabTMP.text = displayedText ;

            
            yield return new WaitForSecondsRealtime(_displayIntervalInSecond);
            _actionTab.SetActive(false);
            _actionTabTMP.text = "" ;

        }

        private void TryToDIsplayActionQueue()
        {
            if (_actionTab.activeSelf)
                return;

            StartCoroutine(_displayActionTabQueue.Dequeue()) ; 
        }
        #endregion


        private void Update()
        {
            if (_displayActionTabQueue.Count > 0)
                TryToDIsplayActionQueue();

            if (_displayUtilityTabQueue.Count > 0)
                TryoToDisplayUtilityQueue(); 

        }
    }
}
