using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Vanaring  { 
    public class SpellWindowGUI : WindowGUI
    {
        [SerializeField] private SpellSocketGUI _spellSocket;
        [SerializeField] private Transform[] spellTransformList;
        private List<SpellSocketGUI> spellSocketGUIList = new List<SpellSocketGUI>();
        private List<int> displayingSpellIndexList = new List<int>();
        [SerializeField] private TextMeshProUGUI spellLogText;
        [SerializeField] private GameObject spellTransform;
        [SerializeField] private GameObject arrowUp;
        [SerializeField] private GameObject arrowDown;
        [SerializeField] private GameObject notificationBox;
        [SerializeField] private TextMeshProUGUI notificationText;
        private int spellIndexFocusUpMin = 0;
        private int spellIndexFocusUpMax = 3;
        private int spellIndexFocusDownMin = 0;
        private int spellIndexFocusDownMax = 2;
        private int currentSelectedIndex = 0;

        public override void OnWindowActive()
        {
            StartCoroutine(PersistentTutorialManager.Instance.CheckTuitorialNotifier("SpellCastExplain"));
        }

        public override void OnWindowDeActive()
        {

        }

        public override void ClearData()
        {
            for (int index = spellSocketGUIList.Count - 1; index >= 0; index--)
            {
                Destroy(spellSocketGUIList[index].gameObject);
                spellSocketGUIList.RemoveAt(index);
            }
            for (int index = displayingSpellIndexList.Count - 1; index >= 0; index--)
            {
                displayingSpellIndexList[index] = 0;
                displayingSpellIndexList.RemoveAt(index);
            }
            spellSocketGUIList.Clear();
            displayingSpellIndexList.Clear();
        }
        public override void LoadWindowData(CombatEntity entity)
        {
            ColorfulLogger.LogWithColor("SpellWindow Loaded", Color.green);
            ClearData();
            _spellSocket.gameObject.SetActive(true);
            notificationBox.SetActive(false);
            spellIndexFocusUpMin = 0;
            spellIndexFocusUpMax = 3;
            spellIndexFocusDownMin = 0;
            spellIndexFocusDownMax = 2;
            currentSelectedIndex = 0;

            int i = 3;
            if (entity is ControlableEntity controlableEntity)
            {
                foreach (SpellActionSO spellAction in controlableEntity.GetControlableEntityActionRegistry.GetSpellAction)
                {
                    SpellSocketGUI newSocket = Instantiate(_spellSocket, spellTransform.transform);
                    newSocket.transform.SetAsFirstSibling();
                    newSocket.Init(spellAction, entity);
                    spellSocketGUIList.Add(newSocket);
                    if (spellSocketGUIList.Count > 3)
                    {
                        newSocket.transform.position = spellTransformList[6].transform.position;
                        displayingSpellIndexList.Add(6);
                    }
                    else
                    {
                        newSocket.transform.position = spellTransformList[i].transform.position;
                        displayingSpellIndexList.Add(i);
                    }
                    newSocket.UnHighlightedButton();
                    if (!newSocket.IsEnergySufficeientToUseThisSpell())
                    {
                        //notificationBox.SetActive(true);
                        newSocket.FadeBlackButton(true);
                    }
                    else { newSocket.FadeBlackButton(false); }
                    i++;
                }
                DisplayArrowIndicator();
            }

            if (spellSocketGUIList.Count == 0)
                throw new Exception("No spell found in SpellRegister (due to empty spell) of " + entity.CombatCharacterSheet.CharacterName);

            _spellSocket.gameObject.SetActive(false);
            spellLogText.text = spellSocketGUIList[currentSelectedIndex].GetSpellDescription();
            spellSocketGUIList[currentSelectedIndex].HightlightedButton();
            //DisplaySocketHighlight();
        }
        public override void ReceiveKeysFromWindowManager(KeyCode key)
        {

            if (key == KeyCode.Q)
            {
                _windowManager.OpenWindow(EWindowGUI.Main);
            }
            else if (key == KeyCode.Space)
            {
                spellSocketGUIList[currentSelectedIndex].CallButtonCallback();
                if (!spellSocketGUIList[currentSelectedIndex].IsEnergySufficeientToUseThisSpell())
                {
                    notificationBox.SetActive(true);
                }
            }
            else if(key == KeyCode.S)
            {
                if (currentSelectedIndex < spellSocketGUIList.Count - 1 && spellSocketGUIList.Count > 1)
                {
                    ScrollToNext();
                }
                notificationBox.SetActive(false);
            }
            else if (key == KeyCode.W)
            {
                if (currentSelectedIndex > 0 && spellSocketGUIList.Count > 1)
                {
                    ScrollToPrevious();
                }
                notificationBox.SetActive(false);
            }
        }
        private void ScrollToNext()
        {
            //select below index
            int i = 0;
            foreach (SpellSocketGUI spell in spellSocketGUIList)
            {
                if (spell != null)
                {
                    if (displayingSpellIndexList[i] < 0 || displayingSpellIndexList[i] > spellTransformList.Length - 1)
                    {
                        throw new Exception("Can't assign socket to transform that out of range");
                    }
                    if (spellTransformList[displayingSpellIndexList[i]] == null)
                    {
                        throw new Exception("No Transform can be assigned");
                    }

                    if(i >= spellIndexFocusUpMin && i <= spellIndexFocusUpMax)
                    {
                        displayingSpellIndexList[i] = displayingSpellIndexList[i] - 1;
                        spell.GetComponent<RectTransform>().DOAnchorPos(spellTransformList[displayingSpellIndexList[i]].localPosition, 0.1f);
                    }
                }
                else
                {
                    Debug.Log("Spell null");
                }
                i++;
            }
            spellSocketGUIList[currentSelectedIndex].UnHighlightedButton();
            currentSelectedIndex++;
            DisplaySocketHighlight();
            spellLogText.text = spellSocketGUIList[currentSelectedIndex].GetSpellDescription();
            UpdateIndexFocusOnInputCall();
            DisplayArrowIndicator();
        }
        private void ScrollToPrevious()
        {
            //select above index
            int i = 0;
            foreach (SpellSocketGUI spell in spellSocketGUIList)
            {
                if (spell != null)
                {
                    if (displayingSpellIndexList[i] <= spellTransformList.Length-1 && i >= spellIndexFocusDownMin && i <= spellIndexFocusDownMax)
                    {
                        displayingSpellIndexList[i] = displayingSpellIndexList[i] + 1;
                        spell.GetComponent<RectTransform>().DOAnchorPos(spellTransformList[displayingSpellIndexList[i]].localPosition, 0.1f);
                    }
                }
                else
                {
                    Debug.Log("Spell null");
                }
                i++;
            }
            spellSocketGUIList[currentSelectedIndex].UnHighlightedButton();
            currentSelectedIndex--;
            DisplaySocketHighlight();
            spellLogText.text = spellSocketGUIList[currentSelectedIndex].GetSpellDescription();
            UpdateIndexFocusOnInputCall();
            DisplayArrowIndicator();
        }
        private void DisplaySocketHighlight()
        {
            if (!spellSocketGUIList[currentSelectedIndex].IsEnergySufficeientToUseThisSpell())
            {
                notificationBox.SetActive(true);
                spellSocketGUIList[currentSelectedIndex].RedHightlightedButton();
            }
            else { spellSocketGUIList[currentSelectedIndex].HightlightedButton(); }
            if (spellLogText == null)
            {
                Debug.Log("null 1");
            }
            if (spellSocketGUIList[currentSelectedIndex] == null)
            {
                Debug.Log("null 2");
            }
        }
        private void DisplayArrowIndicator()
        {
            if (currentSelectedIndex < spellSocketGUIList.Count - 1 && spellSocketGUIList.Count > 1) {
                arrowDown.SetActive(true);
            }
            else
            {
                arrowDown.SetActive(false);
            }
            if (currentSelectedIndex > 0 && spellSocketGUIList.Count > 1) {
                arrowUp.SetActive(true);
            }
            else
            {
                arrowUp.SetActive(false);
            }
        }
        private void UpdateIndexFocusOnInputCall()
        {
            switch (currentSelectedIndex)
            {
                case 0:
                    spellIndexFocusUpMin = 0; //force
                    spellIndexFocusUpMax = 3; //force
                    spellIndexFocusDownMin = 0; //force
                    spellIndexFocusDownMax = 2; //force
                    break;
                case 1:
                    spellIndexFocusUpMin = 0; //force
                    spellIndexFocusUpMax = 4; //+1
                    spellIndexFocusDownMin = 0; 
                    spellIndexFocusDownMax = 3;
                    break;
                case 2:
                    spellIndexFocusUpMin = 0;
                    spellIndexFocusUpMax = 4;
                    spellIndexFocusDownMin = 0;
                    spellIndexFocusDownMax = 4;
                    break; 
                case 3:
                    spellIndexFocusUpMin = 1;
                    spellIndexFocusUpMax = 4;
                    spellIndexFocusDownMin = 0;
                    spellIndexFocusDownMax = 4;
                    break;
                case 4:
                    spellIndexFocusUpMin = 2;
                    spellIndexFocusUpMax = 4;
                    spellIndexFocusDownMin = 1;
                    spellIndexFocusDownMax = 4;
                    break;

            }
            //Debug.Log("FocusUpMin: " + spellIndexFocusUpMin + " FocusUpMax: " + spellIndexFocusUpMax);
            //Debug.Log("FocusDownMin: " + spellIndexFocusDownMin + " FocusDownMax: " + spellIndexFocusDownMax);
        }
        private void ReloadSocketHighlight()
        {

        }
    }
}
