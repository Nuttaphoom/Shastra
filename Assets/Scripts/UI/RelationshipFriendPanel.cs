using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Vanaring
{
    public class RelationshipFriendPanel : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private Button talkButton;
        [SerializeField] private Button eventButton;
        [SerializeField] private Button bondButton;
        [SerializeField] private TextMeshProUGUI friendName;
        [SerializeField] private TextMeshProUGUI friendDescription;

        private CharacterRelationshipDataSO charRelationDataSO;

        private void Awake()
        {
            
        }
        private void Start()
        {
            
        }
        public void InitPanel(CharacterRelationshipDataSO so)
        {
            charRelationDataSO = so;
            RemoveFriendButtonListener();
            friendName.text = charRelationDataSO.GetCharacterName;
            friendDescription.text = charRelationDataSO.GetCharacterName;
            if(talkButton.onClick.GetPersistentEventCount() <= 0)
            {
                talkButton.interactable = false;
                talkButton.GetComponent<EventTrigger>().enabled = false;
            }
            if (eventButton.onClick.GetPersistentEventCount() <= 0)
            {
                eventButton.interactable = false;
                eventButton.GetComponent<EventTrigger>().enabled = false;
            }
            if (bondButton.onClick.GetPersistentEventCount() <= 0)
            {
                bondButton.interactable = false;
                bondButton.GetComponent<EventTrigger>().enabled = false;
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
        }

        public void SetEventButtonListener(UnityAction action)
        {
            eventButton.onClick.AddListener(action);
            eventButton.GetComponent<EventTrigger>().enabled = true;
        }
        public void SetBondButtonListener(UnityAction action)
        {
            bondButton.onClick.AddListener(action);
            bondButton.GetComponent<EventTrigger>().enabled = true;
        }

        public void ExpandUp(RectTransform transform)
        {
            transform.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        public void ShrinkButton(RectTransform transform)
        {
            transform.transform.localScale = Vector3.one;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //ExpandUp();
        }

        public void OpenPanel()
        {
            gameObject.SetActive(true);
        }
    }
}
