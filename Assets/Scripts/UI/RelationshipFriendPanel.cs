using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Vanaring
{
    public class RelationshipFriendPanel : MonoBehaviour
    {
        [SerializeField] private Button talkButton;
        [SerializeField] private Button eventButton;
        [SerializeField] private Button bondButton;
        [SerializeField] private TextMeshProUGUI friendName;
        [SerializeField] private TextMeshProUGUI friendDescription;
        [SerializeField] private Image bondFilledBar;

        private CharacterRelationshipDataSO charRelationDataSO;

        public void InitPanel(CharacterRelationshipDataSO so)
        {
            charRelationDataSO = so;
            bondFilledBar.fillAmount = 0.5f;
            RemoveFriendButtonListener();
            friendName.text = charRelationDataSO.GetCharacterName;
            friendDescription.text = charRelationDataSO.GetCharacterName;
            DisableRelationshipButton(talkButton);
            DisableRelationshipButton(eventButton);
            DisableRelationshipButton(bondButton);
        }

        private void DisableRelationshipButton(Button button)
        {
            if (button.onClick.GetPersistentEventCount() <= 0)
            {
                button.interactable = false;
                button.GetComponent<EventTrigger>().enabled = false;
                button.GetComponentInChildren<Image>().gameObject.SetActive(true);
            }
        }

        private void RemoveFriendButtonListener()
        {
            talkButton.onClick.RemoveAllListeners();
            eventButton.onClick.RemoveAllListeners();
            bondButton.onClick.RemoveAllListeners();
        }

        public void SetTalkButtonListener(UnityAction action)
        {
            talkButton.onClick.AddListener(action);
            talkButton.GetComponent<EventTrigger>().enabled = true;
            talkButton.GetComponentInChildren<Image>().gameObject.SetActive(false);
        }

        public void SetEventButtonListener(UnityAction action)
        {
            eventButton.onClick.AddListener(action);
            eventButton.GetComponent<EventTrigger>().enabled = true;
            eventButton.GetComponentInChildren<Image>().gameObject.SetActive(false);
        }
        public void SetBondButtonListener(UnityAction action)
        {
            bondButton.onClick.AddListener(action);
            bondButton.GetComponent<EventTrigger>().enabled = true;
            bondButton.GetComponentInChildren<Image>().gameObject.SetActive(false);
        }

        public void ExpandUp(RectTransform transform)
        {
            transform.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        public void ShrinkButton(RectTransform transform)
        {
            transform.transform.localScale = Vector3.one;
        }

        public void OpenPanel()
        {
            gameObject.SetActive(true);
        }
    }
}
