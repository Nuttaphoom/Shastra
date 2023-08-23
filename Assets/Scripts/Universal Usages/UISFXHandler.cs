using Hellmade.Sound;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public class UISFXHandler : MonoBehaviour
    {
        [Serializable]
        public struct SoundEffectData
        {
            [SerializeField]
            public string soundID;
            [SerializeField]
            public AudioClip clip;
            [SerializeField]
            public float audioScale;
        }

        [SerializeField]
        List<SoundEffectData> _SoundDataBase;

        public void PlayUISound(string ID)
        {
            for(int i = 0; i < _SoundDataBase.Count; i++)
            {
                if (_SoundDataBase[i].soundID == ID)
                {
                    EazySoundManager.PlayUISound(_SoundDataBase[i].clip, _SoundDataBase[i].audioScale);
                    return;
                }
            }

            Debug.Log("Can't find Sound in SoundDataBase. SoundID: " + ID);
        }
    }
}
