using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Vanaring.Assets.Scripts.Utilities;

namespace Vanaring
{
    public class CombatTurnCounterUI : MonoBehaviour, ISceneLoaderWaitForSignal
    {
        private int _currentPlayerRound;

        [SerializeField]
        private TextMeshProUGUI _roundCoutnerText ;

        private void Awake()
        {
            if (EnableDebuggingChecker.Instance.IsDebugingModeEnable)
            {
                Debug.Log("Debug mode enable");
                StartCoroutine(OnNotifySceneLoadingComplete());
            }else
            {
                Debug.Log("Debug mode disable");
            }
        }
        public void IncreaseTurn(ECompetatorSide currentSide)
        {

            if (currentSide == ECompetatorSide.Ally)
            {
                _currentPlayerRound += 1;

                UpdateTurnCounter();
            }
        }

        private void UpdateTurnCounter()
        {
            if (_currentPlayerRound < 10)
            {
                _roundCoutnerText.text = "0" + _currentPlayerRound.ToString();
            }
        }

        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            yield return null;
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            _currentPlayerRound = 0;

            UpdateTurnCounter();

            CombatReferee.Instance.SubOnNewRoundBegin(IncreaseTurn);

            yield return null; 
        }
    }
}
