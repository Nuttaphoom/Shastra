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
        private bool isYes = false;

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
            isYes = false;
            cancelButton.onClick.AddListener(CloseWindow);
            cancelButton.onClick.AddListener(delegate { PersistentButtonSelector.Instance.DeSelectedButton(); });
        }

        public IEnumerator OpenWindow()
        {
            //CentralInputReceiver.Instance.AddInputReceiverIntoStack(this);
            yield return new WaitForEndOfFrame();
            isYes = false;
            cancelButton.Select();
            //gameObject.GetComponent<Animator>().Play("EmbarkNoButtonDeSelect");
            //gameObject.GetComponent<Animator>().Play("EmbarkYesButtonDeSelect");
            gameObject.GetComponent<Animator>().Play("EmbarkNoButton");
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
            if(key == InputCode.Right && isYes)
            {
                isYes = !isYes;
                cancelButton.Select();
                //gameObject.GetComponent<Animator>().Play("EmbarkYesButtonDeSelect");
                gameObject.GetComponent<Animator>().Play("EmbarkNoButton");
            }
            if (key == InputCode.Left && !isYes)
            {
                isYes = !isYes;
                confirmButton.Select();
                //gameObject.GetComponent<Animator>().Play("EmbarkNoButtonDeSelect");
                gameObject.GetComponent<Animator>().Play("EmbarkYesButton");
            }
            if (key == InputCode.DeSelect)
            {
                cancelButton.onClick.Invoke();
                cancelButton.Select();
                gameObject.GetComponent<Animator>().Play("EmbarkNoButton");
            }
        }

        private MissionShopWindowGUI _shopWindow;
        public void Init(MissionShopWindowGUI window)
        {
            _shopWindow = window;
        }
    }
}
