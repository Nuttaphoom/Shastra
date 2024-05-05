using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring
{
    public class EffectIconGUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeToLiveText;
        [SerializeField] private Image effectImage;
        public int cur_ttl = 99;
        public void SetTimetoLiveText(string ttl)
        {
            timeToLiveText.text = ttl;
        }
        
        public void Init(StatusRuntimeEffect sre)
        {
            timeToLiveText.text = sre.TimeToLive.ToString();
            cur_ttl = sre.TimeToLive;
        }

    }
}
