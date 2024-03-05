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

        public IEnumerator BeginPlayerTurn()
        {
            playerTurn.Play();
            if (playerTurn.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }

        public IEnumerator BeginEnemyTurn()
        {
            enemyTurn.Play();
            if(enemyTurn.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }
    }
}
