using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class EmbarkConfirmWindow : MonoBehaviour, IInputReceiver
    {
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        public Button ConfirmButton
        {
            get
            {
                return confirmButton;
            }
        }
        public Button CancelButton
        {
            get
            {
                return cancelButton;
            }
        }

        private void Start()
        {
            cancelButton.onClick.AddListener(CloseWindow);
        }

        public void OpenWindow()
        {
            //CentralInputReceiver.Instance.AddInputReceiverIntoStack(this);
            cancelButton.Select();
        }
        public void CloseWindow()
        {
            CentralInputReceiver.Instance.RemoveInputReceiverIntoStack(this);
            CentralInputReceiver.Instance.ClearStack();
            CentralInputReceiver.Instance.AddInputReceiverIntoStack(_shopWindow);
            gameObject.SetActive(false);
        }

        public void ReceiveKeys(InputCode key)
        {
            if(key == InputCode.Right)
            {
                confirmButton.Select();
            }
            if (key == InputCode.Left)
            {
                cancelButton.Select();
            }
        }

        private MissionShopWindowGUI _shopWindow;
        public void Init(MissionShopWindowGUI window)
        {
            _shopWindow = window;
        }
    }
}
