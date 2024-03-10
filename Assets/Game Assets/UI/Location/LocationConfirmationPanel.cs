using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Vanaring
{
    public class LocationConfirmationPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI warningText;
        [SerializeField] private Button confirmButton;
        [SerializeField] private GameObject gfx;
        public GameObject GFX => gfx;

        private void Awake()
        {
            gameObject.GetComponent<Canvas>().sortingOrder = 10;

        }

        public TextMeshProUGUI WarningText
        {
            get { return warningText; }
            set { warningText = value; }
        }

        public void SetButtonListerner(UnityAction action, bool removeallListener = true)
        {
            if (removeallListener) 
                confirmButton.onClick.RemoveAllListeners();

            confirmButton.onClick.AddListener(action);
        }

    }
}
