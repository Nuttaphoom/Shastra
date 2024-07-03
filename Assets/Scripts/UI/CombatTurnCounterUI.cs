using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField]
        private Animator sideAnim;
        [SerializeField]
        private Sprite allyTurnSprite;
        [SerializeField]
        private Sprite enemyTurnSprite;
        [SerializeField]
        private Image turnImage;
        [SerializeField]
        private TextMeshProUGUI sideText;

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
                StartCoroutine(UpdateTurnSide("Player's Turn", allyTurnSprite));
                _currentPlayerRound += 1;

                StartCoroutine(UpdateTurnCounter());
            }

            if (currentSide == ECompetatorSide.Hostile)
            {
                StartCoroutine(UpdateTurnSide("Enemy's Turn", enemyTurnSprite));
            }
        }

        private IEnumerator UpdateTurnSide(string t, Sprite s)
        {
            sideAnim.Play("NextTurnSide");

            yield return new WaitForSeconds(0.15f);
            sideText.text = t;
            turnImage.sprite = s;

            sideAnim.SetTrigger("TurnIn");
        }

        private IEnumerator UpdateTurnCounter()
        {
            if (_currentPlayerRound < 10)
            {
                turnAnim.Play("NextTurnOut");

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
