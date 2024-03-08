using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

namespace Vanaring
{
    public class SideTurnDisplayerManager : MonoBehaviour
    {
        [SerializeField] private PlayableDirector playerTurn;
        [SerializeField] private PlayableDirector enemyTurn;

        public IEnumerator DisplaySideRoundCoroutine(ECompetatorSide side)
        {
            if (side == ECompetatorSide.Ally)
            {
                playerTurn.Play();
                while (playerTurn.state == PlayState.Playing)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            else if (side == ECompetatorSide.Hostile)
            {
                enemyTurn.Play();
                while (enemyTurn.state == PlayState.Playing)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
           
            yield return null;
        }
 
    }
}
