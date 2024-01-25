using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vanaring
{
    public class ButtonClicker : MonoBehaviour
    {
        UIDocument buttonDocument;
        Button uiButton;
        private void OnEnable()
        {
            buttonDocument = GetComponent<UIDocument>();

            if(buttonDocument == null)
            {
                Debug.LogWarning("No button document found.");
            }

            uiButton = buttonDocument.rootVisualElement.Q("TestButton") as Button;

            if(uiButton != null)
            {
                Debug.Log("UI button found");
            }

            uiButton.RegisterCallback<ClickEvent>(OnButtonClick);
        }

        public void OnButtonClick(ClickEvent evt)
        {
            Debug.Log("Click");
        }
    }
}
