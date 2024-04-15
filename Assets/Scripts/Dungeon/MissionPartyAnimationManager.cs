using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class MissionPartyAnimationManager : MonoBehaviour
    {
        [SerializeField] private List<Animator> animatorList = new List<Animator>();

        public void PerformMoveAnimation(Vector3 direction)
        {
            foreach (Animator anim in animatorList)
            {
                anim.SetBool("IsMoving", true);
                anim.SetFloat("horizontalMove", direction.x);
                anim.SetFloat("verticalMove", direction.z);
            }
        }

        public void PerformIdleAnimation()
        {
            
            foreach (Animator anim in animatorList)
            {
                anim.SetBool("IsMoving", false);
                //anim.SetFloat("horizontalMove", 0.0f);
                //anim.SetFloat("verticalMove", 0.0f);
            }
        }

        public IEnumerator OnCharacterMoveToNode(BaseMissionNode nodeToVisit)
        {
            Vector3 direction = (nodeToVisit.transform.position - transform.position).normalized;
            Debug.Log("Direction - x: " + direction.x + ", y: " + direction.y + ", z: " + direction.z);

            Vector3 prevUIPos = transform.position;
            float progression = 0;

            PerformMoveAnimation(direction);

            while (progression < 1)
            {

                transform.position = Vector3.Lerp(prevUIPos, nodeToVisit.transform.position, progression);
                progression += Time.deltaTime / 1.7f;

                yield return null;

            }

            PerformIdleAnimation();
        }
    }
}
