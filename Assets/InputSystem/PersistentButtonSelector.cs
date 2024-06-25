using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
        private Button dummyButton;

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
        private void SubSelectSceneLoader()
        {
            if (isSub)
                return;
    
            if (PersistentSceneLoader.Instance.GetTransitionManager != null)
            {
                isSub = true; 
                PersistentSceneLoader.Instance.GetTransitionManager.SubOnSceneLoaderComplete(ResetInitialButtonSelected);
            }
        }

        public void ResetInitialButtonSelected(Null n)
        {
            initialButtonSelected = false;
        }

        public void AssignInitialButtons(Button button)
        {
            initialButton = button;
        }
        public void SelectInitialButtons()
        {
            if (initialButton == null)
            {
                //Debug.LogWarning("initialButton is null");
                return;
            }
            if (!initialButtonSelected)
            {
                initialButton.Select();
                initialButtonSelected = true;
            }
        }
        public void DeselectButton()
        {
            if (dummyButton == null)
            {
                //Debug.LogWarning("dummyButton is null");
                return;
            }

            dummyButton.Select();
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
