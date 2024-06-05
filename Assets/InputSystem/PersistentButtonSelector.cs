using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class PersistentButtonSelector : PersistentInstantiatedObject<PersistentButtonSelector>
    {
        [SerializeField]
        public List<Button> previousButtons;

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
