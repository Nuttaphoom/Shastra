using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Vanaring
{
    public class PauseMenuMainPanel : PauseMenuWindowGUI
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button characterButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button quitButton;
        private List<Button> pauseButtonList = new List<Button>();
        private int buttonSelectingIndex = 0;
        [SerializeField] private Image hlImage;
        public override void ClearData()
        {
            buttonSelectingIndex = 0;
        }

        public override void LoadWindowData(CombatEntity entity)
        {
            
        }

        public override void OnWindowActive()
        {
            pauseButtonList.Add(resumeButton);
            pauseButtonList.Add(characterButton);
            pauseButtonList.Add(settingButton);
            pauseButtonList.Add(quitButton);
        }

        public override void OnWindowDeActive()
        {
            pauseButtonList.Clear();
        }

        public override void ReceiveKeysFromWindowManager(KeyCode key)
        {
            if (key == KeyCode.W)
            {
                if (buttonSelectingIndex > 0)
                {
                    buttonSelectingIndex--;
                }
                
                SwitchOption();
            }
            if (key == KeyCode.S)
            {
                if (buttonSelectingIndex < pauseButtonList.Count - 1)
                {
                    buttonSelectingIndex++;
                }
                SwitchOption();
            }
            if (key == KeyCode.Space)
            {
                pauseButtonList[buttonSelectingIndex].onClick?.Invoke();
            }
        }

        private void SwitchOption()
        {
            float newY = (buttonSelectingIndex - 133) - (buttonSelectingIndex * 100) + 38f;
            hlImage.rectTransform.localPosition = new Vector3(hlImage.rectTransform.localPosition.x, newY, hlImage.rectTransform.localPosition.z);
        }
    }
}
