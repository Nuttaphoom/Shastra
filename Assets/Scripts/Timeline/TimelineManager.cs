using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Vanaring 
{
    public class TimelineManager : MonoBehaviour
    {
        public PlayableDirector timeline;
        public PlayableDirector sw1_tl;
        public PlayableDirector sw2_tl;
        public PlayableDirector sw3_tl;

        public PlayableDirector swe1_tl;
        public PlayableDirector swe2_tl;
        public PlayableDirector swe3_tl;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                NormalAttack();
            }
        }
        public void NormalAttack()
        {
            timeline.Stop();
            timeline.Play();
        }

        public void SwitchCharacter1()
        {
            sw1_tl.Stop();
            sw1_tl.Play();
        }

        public void SwitchCharacter2()
        {
            sw2_tl.Stop();
            sw2_tl.Play();
        }

        public void SwitchCharacter3()
        {
            sw3_tl.Stop();
            sw3_tl.Play();
        }

        public void SwitchEnemy1()
        {
            swe1_tl.Stop();
            swe1_tl.Play();
        }

        public void SwitchEnemy2()
        {
            swe2_tl.Stop();
            swe2_tl.Play();
        }

        public void SwitchEnemy3()
        {
            swe3_tl.Stop();
            swe3_tl.Play();
        }

    }
}
