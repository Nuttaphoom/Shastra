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
        private float _displayIntervalInSecond = 0.0f;

        [SerializeField]
        private GameObject _actionTab;

        private Queue<IEnumerator> _displayActionTabQueue = new Queue<IEnumerator>(); 

        public void DisplayPerformedActionBacklog(EntityActionPair entityActionPair)
        {
            CombatEntity entity = entityActionPair.Actor;
            ActorAction action = entityActionPair.PerformedAction;
            //string comment = action.GetActionComment().GetComment(entity) ;
            //if (comment == null || comment == "")
            //    return; 

            string comment = action.GetDescription().FieldName;

            _displayActionTabQueue.Enqueue(DisplayActionTabCoroutine(comment)); 
            
        }

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

        private void Update()
        {
            if (_displayActionTabQueue.Count > 0)
                TryToDIsplayActionQueue(); 

        }
    }
}
