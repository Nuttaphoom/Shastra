using Hellmade.Sound;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public class BGMHandler : MonoBehaviour
    {

        [SerializeField]
        private AudioClip _bgmAudioSource;

        private Audio _audio; 
        [SerializeField]
        private float _audioScaling = 1.0f; 
        private void Start()
        {
            int audioID = EazySoundManager.PlayMusic(_bgmAudioSource, _audioScaling, true, false);
            _audio = EazySoundManager.GetAudio(audioID);

        }

        private void Update()
        {
            _audio.SetVolume(_audioScaling); 
        }
    }
}
