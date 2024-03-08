using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Playables;

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
        [SerializeField] private PlayableDirector outTimeline;
        [SerializeField] private PlayableDirector inTimeline;

        private CharacterRelationshipDataSO charRelationDataSO;

        public void InitPanel(CharacterRelationshipDataSO so)
        {
            charRelationDataSO = so;
            Debug.Log(PersistentPlayerPersonalDataManager.Instance.RelationshipHandler.GetCurrentBondLevel(so.GetCharacterName));
            bondFilledBar.fillAmount = (float)(PersistentPlayerPersonalDataManager.Instance.RelationshipHandler.GetCurrentBondLevel(so.GetCharacterName)-1)/5f;
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
            talkButton.interactable = true;
            for (int i = 0; i < talkButton.transform.childCount; i++)
            {
                Transform child = talkButton.transform.GetChild(i);
                if (child.TryGetComponent(out Image image))
                {
                    image.gameObject.SetActive(false);
                }
            }
    
        }

        public void SetEventButtonListener(UnityAction action)
        {
            eventButton.onClick.AddListener(action);
            eventButton.interactable = true;
            for (int i = 0; i < eventButton.transform.childCount; i++)
            {
                Transform child = eventButton.transform.GetChild(i);
                if (child.TryGetComponent(out Image image))
                {
                    image.gameObject.SetActive(false);
                }
            }
            
        }
        public void SetBondButtonListener(UnityAction action)
        {
            bondButton.onClick.AddListener(action);
            bondButton.interactable = true ; 
            for (int i = 0; i < bondButton.transform.childCount ; i++){
                Transform child = bondButton.transform.GetChild(i); 
                if (child.TryGetComponent(out Image image))
                {
                    image.gameObject.SetActive(false); 
                }
            }
        }

        public void ExpandUp(RectTransform transform)
        {
            transform.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        public void ShrinkButton(RectTransform transform)
        {
            transform.transform.localScale = Vector3.one;
        }

        public void ButtonOpenPanel()
        {
            StartCoroutine(OpenPanel());
        }
        public void ButtonClosePanel()
        {
            StartCoroutine(ClosePanel());
        }

        private IEnumerator OpenPanel()
        {
            gameObject.SetActive(true);
            outTimeline.Stop();
            inTimeline.Play();
            while (inTimeline.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }
            //inTimeline.time = inTimeline.duration;
            yield return null;
        }

        public IEnumerator ClosePanel()
        {
            inTimeline.Stop();
            outTimeline.Play();
            while (outTimeline.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }
            outTimeline.time = outTimeline.duration;
            yield return null; 
        }
    }
}
