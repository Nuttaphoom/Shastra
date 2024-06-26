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
        [SerializeField]
        private Animator turnAnim;

        private void Awake()
        {
            if (EnableDebuggingChecker.Instance.IsDebugingModeEnable)
            {
                StartCoroutine(OnNotifySceneLoadingComplete());
            } 
        }
        public void IncreaseTurn(ECompetatorSide currentSide)
        {

            if (currentSide == ECompetatorSide.Ally)
            {
                _currentPlayerRound += 1;

                StartCoroutine(UpdateTurnCounter());
            }
        }

        private IEnumerator UpdateTurnCounter()
        {
            if (_currentPlayerRound < 10)
            {
                turnAnim.Play("NextTurnOut");
                //while (turnAnim.GetCurrentAnimatorStateInfo(0).IsName("NextTurnOut"))
                //{
                //    yield return new WaitForEndOfFrame();
                //}

                yield return new WaitForSeconds(0.15f);
                
                _roundCoutnerText.text = "0" + _currentPlayerRound.ToString();

                turnAnim.SetTrigger("TurnIn");
            }
        }

        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            yield return null;
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            _currentPlayerRound = 0;

            yield return UpdateTurnCounter();

            CombatReferee.Instance.SubOnNewRoundBegin(IncreaseTurn);

            yield return null; 
        }
    }
}
