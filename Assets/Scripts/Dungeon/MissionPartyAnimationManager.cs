using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class MissionPartyAnimationManager : MonoBehaviour
    {
        [SerializeField] private List<Animator> animatorList = new List<Animator>();

        [SerializeField] private List<Sprite> idleSpriteList = new List<Sprite>();
        [SerializeField] private List<Sprite> walkSpriteList = new List<Sprite>();



        public void LootBox()
        {
            //foreach (Animator anim in animatorList)
            //{
            //    anim.Play("testLoot");
            //}
        }

        public void PerformMoveAnimation(Vector3 direction)
        {
            int i = 0;
            foreach (Animator anim in animatorList)
            {
                //anim.SetBool("IsMoving", true);
                anim.speed = 5f;
                anim.GetComponent<Image>().sprite = walkSpriteList[i];
                anim.SetFloat("horizontalMove", direction.x);
                anim.SetFloat("verticalMove", direction.z);
                Debug.Log("dir: " + direction.x + " " + direction.z);
                if(direction.x == -1 || direction.z == 1 || direction.z > direction.x)
                {
                    Debug.Log("Flip Left");
                    anim.GetComponent<PixelCrushers.AlwaysFaceCamera>().rotate180 = true;
                }
                else
                {
                    Debug.Log("Flip Right");
                    anim.GetComponent<PixelCrushers.AlwaysFaceCamera>().rotate180 = false;
                }
                i++;
            }
        }

        public void PerformIdleAnimation()
        {
            int i = 0;
            foreach (Animator anim in animatorList)
            {
                anim.GetComponent<Image>().sprite = idleSpriteList[i];
                //anim.SetBool("IsMoving", false);
                anim.speed = 1f;
                //anim.SetFloat("horizontalMove", 0.0f);
                //anim.SetFloat("verticalMove", 0.0f);
                i++;
            }
        }

        public IEnumerator OnCharacterMoveToNode(BaseMissionNode nodeToVisit)
        {
            Vector3 direction = (nodeToVisit.transform.position - transform.position).normalized;
            //Debug.Log("Direction - x: " + direction.x + ", y: " + direction.y + ", z: " + direction.z);

            Vector3 prevUIPos = transform.position;
            float progression = 0;

            Vector3 destinationToVisit = new Vector3(nodeToVisit.transform.position.x, 0.4f, nodeToVisit.transform.position.z);

            PerformMoveAnimation(direction);

            while (progression < 1)
            {

                transform.position = Vector3.Lerp(prevUIPos, destinationToVisit, progression);
                progression += Time.deltaTime / 1.7f;

                yield return null;

            }

            //transform.position = nodeToVisit.transform.position;

            //transform.position = new Vector3(transform.position.x + 2, 0, transform.position.z);

            PerformIdleAnimation();
        }
    }
}
