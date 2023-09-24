using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring 
{
    public class ButtonIndicatorWindow : MonoBehaviour
    {
        public enum IndicatorButtonShow
        {
            MAIN,
            LIST,
            TARGET
        }
        [SerializeField] private GameObject buttonIndicatorWindow;
        [SerializeField] private GameObject mainModeIndicator;
        [SerializeField] private GameObject listModeIndicator;
        [SerializeField] private GameObject targetModeIndicator;
        void Start()
        {
            SetIndicatorButtonShow(IndicatorButtonShow.MAIN, true);
        }

        public void SetIndicatorButtonShow(IndicatorButtonShow mode, bool isShow = false)
        {
            if (!isShow)
            {
                CloseAllIndicator();
                return;
            }
            CloseAllIndicator();
            buttonIndicatorWindow.SetActive(true);
            switch (mode)
            {
                case IndicatorButtonShow.MAIN:
                    mainModeIndicator.SetActive(isShow);
                    break;
                case IndicatorButtonShow.LIST:
                    listModeIndicator.SetActive(isShow);
                    break;
                case IndicatorButtonShow.TARGET:
                    targetModeIndicator.SetActive(isShow);
                    break;
            }
        }

        private void CloseAllIndicator()
        {
            mainModeIndicator.SetActive(false);
            listModeIndicator.SetActive(false);
            targetModeIndicator.SetActive(false);
        }

        public void ClosePanel()
        {
            buttonIndicatorWindow.SetActive(false);
        }
    }
}
