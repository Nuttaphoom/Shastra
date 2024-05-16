using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Vanaring
{
    public class SocketGUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI numText;
        [SerializeField] private Image iconImg;
        public void Init(string name, string num, Sprite img)
        {
            nameText.text = name;
            numText.text = num;
            iconImg.sprite = img;
        }
    }
}
