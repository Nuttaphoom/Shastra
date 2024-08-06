using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Vanaring
{
    public class PersistentButtonSelector : PersistentInstantiatedObject<PersistentButtonSelector>
    {
        [SerializeField]
        public List<Button> previousButtons = new List<Button>();

        [SerializeField]
        private Button initialButton;

        [SerializeField]
        private Button capturedButton;

        [SerializeField]
        private Button dummyButton;

        [SerializeField]
        public List<Button> ButtonsPool = new List<Button>();

        private bool initialButtonSelected = false;

        private bool isSub = false;

        private void Awake() 
        {
            SubSelectSceneLoader(); 
        }

        private void FixedUpdate()
        {
            SubSelectSceneLoader();
        }

        private void GetButtonsPool()
        {
            Button[] Buttons = FindObjectsOfType<Button>();
            for (int i = 0; i < Buttons.Length; i++)
            {
                if (!ButtonsPool.Contains(Buttons[i]))
                {
                    ButtonsPool.Add(Buttons[i]);
                }
            }
        }
        private void ClearButtonsPool(Null n)
        {
            ButtonsPool.Clear();
        }

        private void SubSelectSceneLoader()
        {
            if (isSub)
                return;
    
            if (PersistentSceneLoader.Instance.GetTransitionManager != null)
            {
                isSub = true; 
                PersistentSceneLoader.Instance.GetTransitionManager.SubOnSceneLoaderComplete(ResetInitialButtonSelected);
                PersistentSceneLoader.Instance.GetTransitionManager.SubOnSceneLoaderComplete(ClearPreviousButtons);
                PersistentSceneLoader.Instance.GetTransitionManager.SubOnSceneLoaderComplete(ClearButtonsPool);
            }
        }

        public void ResetInitialButtonSelected(Null n)
        {
            initialButtonSelected = false;
        }

        public void ClearPreviousButtons(Null n)
        {
            previousButtons.Clear();
        }

        public void AssignInitialButtons(Button button)
        {
            initialButton = button;
        }
        public void SelectInitialButtons()
        {
            if (initialButton == null)
            {
                return;
            }
            if (!initialButtonSelected)
            {
                initialButton.Select();
                initialButtonSelected = true;
            }
        }

        public void CaptureCurrentSelectedButton()
        {
            capturedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

            //Debug.Log("capturedButton : " + capturedButton); 
        }

        public void DeSelectedButton()
        {
            dummyButton.Select();
        }

        public void RestoreCaptureButton()
        {
            capturedButton.Select();
        }

        public void AddPreviousButton(Button button)
        {
            previousButtons.Add(button);
        }

        public void RemoveLastPreviousButton()
        {
            if (previousButtons.Count > 0)
            {
                //if exist remove the last previous button
                previousButtons.RemoveAt(previousButtons.Count - 1);
            }
            else
            {
                Debug.LogError("Can't RemoveLastPreviousButton previousButtons is empty.");
            }
            
        }

        public void SelectPreviousButton()
        {
            if (previousButtons.Count > 0)
            {
                //if exist select the last button
                previousButtons[previousButtons.Count - 1].Select();
            }
            else
            {
                Debug.LogError("Can't SelectPreviousButton previousButtons is empty.");
            }
        }
    }
}
