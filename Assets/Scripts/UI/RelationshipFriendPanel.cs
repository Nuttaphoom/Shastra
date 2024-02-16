using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Vanaring
{
    public class RelationshipFriendPanel : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private Button talkButton;
        [SerializeField] private Button eventButton;
        [SerializeField] private Button bondButton;
        [SerializeField] private TextMeshProUGUI friendName;
        [SerializeField] private TextMeshProUGUI friendDescription;
        [SerializeField] private Image eventBG;

        private CharacterRelationshipDataSO charRelationDataSO;

        private void Awake()
        {
            
        }
        private void Start()
        {
            
        }
        public void Init(CharacterRelationshipDataSO so)
        {
            charRelationDataSO = so;
            SetFriendButtonListener();
            friendName.text = charRelationDataSO.GetCharacterName;
            friendDescription.text = charRelationDataSO.GetCharacterName;
            eventBG.sprite = null;
        }

        private void SetFriendButtonListener()
        {
            talkButton.onClick.RemoveAllListeners();
            eventButton.onClick.RemoveAllListeners();
            bondButton.onClick.RemoveAllListeners();

            talkButton.onClick.AddListener(() => gameObject.SetActive(false));
            eventButton.onClick.AddListener(() => gameObject.SetActive(false));
            bondButton.onClick.AddListener(() => gameObject.SetActive(false));
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
    }
}
