using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring_DepaDemo
{
    public class SpellWindowManager : HierarchyUIWindow, IInputReceiver 
    {
        [SerializeField]
        private CombatEntity _combatEntity;  

        [SerializeField]
        private SpellSocketGUI _templatePrefab;

        [SerializeField]
        private Transform _spellParent;

        [SerializeField]
        private Transform[] _spellSocketGUITransformPos;

        [SerializeField]
        private Image _upArrow;

        [SerializeField]
        private Image _downArrow;

        private CombatGraphicalHandler _graphicalHandler;

        private int currentPageIndex = 0;

        private int startIndex = 0;
        private int endIndex = 2;
        private int _currentIndex = 0;

        private List<SpellSocketGUI> _spellSockets = new List<SpellSocketGUI>();
        private List<GameObject> _pageIndexs = new List<GameObject>();

        // Start is called before the first frame update
        private CombatGraphicalHandler _combatGraphicalHandler ; 

        void Awake()
        {

            LoadSpellSocketGUI(startIndex, endIndex);
            _graphicalHandler = _combatEntity.GetComponent<CombatGraphicalHandler>();
            _templatePrefab.gameObject.SetActive(false);
            GenerateArrow();
        }
        private void GenerateArrow() {
            
            if(_upArrow == null || _downArrow == null)
            {
                return;
            }
            _upArrow.gameObject.SetActive(false);
            _downArrow.gameObject.SetActive(false);
            if(_combatEntity.SpellCaster.SpellAbilities.Count <= 3)
            {
                _upArrow.gameObject.SetActive(false);
                _downArrow.gameObject.SetActive(false);
            }
            else
            {
                if (startIndex == 0 && endIndex == 2)
                {
                    _upArrow.gameObject.SetActive(false);
                    _downArrow.gameObject.SetActive(true);
                }
                else if (_combatEntity.SpellCaster.SpellAbilities.Count - 1 != endIndex)
                {
                    _upArrow.gameObject.SetActive(true);
                    _downArrow.gameObject.SetActive(true);
                }
                else if (endIndex == _combatEntity.SpellCaster.SpellAbilities.Count - 1)
                {
                    _upArrow.gameObject.SetActive(true);
                    _downArrow.gameObject.SetActive(false);
                }
            }
        }

        //private void GeneratePageIndex()
        //{
        //    currentPageIndex = 0;
        //    _pageIndexs = new List<GameObject>();
        //    float spellCount = (float)_combatEntity.SpellCaster.SpellAbilities.Count / 3;
        //    for (int i = 0; i < Mathf.CeilToInt(spellCount); i++)
        //    {
        //        GameObject newPageIndex = Instantiate(_pageIndexTemplate, _pageIndexParent.transform);
        //        _pageIndexs.Add(newPageIndex);
        //        newPageIndex.transform.localScale = _pageIndexTemplate.transform.localScale;
        //        ChangeOpacity(newPageIndex.gameObject.GetComponent<Image>(), 0.2f);
        //    }
        //    ChangeOpacity(_pageIndexs[currentPageIndex].gameObject.GetComponent<Image>(), 1.0f);
        //}
        //private void ChangeOpacity(Image image, float opacity)
        //{
        //    Color imageColor = image.color;
        //    imageColor.a = opacity;
        //    image.color = imageColor;
        //}

        //private void NextPageIndex()
        //{
        //    currentPageIndex++;
        //    foreach (GameObject pi in _pageIndexs)
        //    {
        //        ChangeOpacity(pi.gameObject.GetComponent<Image>(), 0.2f);
        //    }
        //    ChangeOpacity(_pageIndexs[currentPageIndex].gameObject.GetComponent<Image>(), 1.0f);
        //}

        //private void PreviousPageIndex()
        //{
        //    currentPageIndex--;
        //    foreach (GameObject pi in _pageIndexs)
        //    {
        //        ChangeOpacity(pi.gameObject.GetComponent<Image>(), 0.2f);
        //    }
        //    ChangeOpacity(_pageIndexs[currentPageIndex].gameObject.GetComponent<Image>(), 1.0f);
        //}

        private void LoadSpellSocketGUI(int start, int end)
        {
            int tmpNum = 0;
            int tmpSlotIndex = 0;
            _spellSockets = new List<SpellSocketGUI>(); 
            foreach (SpellAbilitySO spellAbility in _combatEntity.SpellCaster.SpellAbilities)
            {
                if (tmpNum >= start && tmpNum <= end)
                {
                    SpellSocketGUI newSocket = Instantiate(_templatePrefab, _spellParent.transform);
                    _spellSockets.Add(newSocket); 
                    newSocket.transform.position = _spellSocketGUITransformPos[tmpSlotIndex].transform.position;
                    newSocket.transform.localScale = _templatePrefab.transform.localScale;
                    newSocket.Init(spellAbility, _combatEntity);
                    newSocket.gameObject.SetActive(true);
                    newSocket.transform.SetAsFirstSibling();
                    tmpSlotIndex++;
                }
                tmpNum++;
            }
        }

        private bool LoadLowerSocketItem()
        {
            if(endIndex < _combatEntity.SpellCaster.SpellAbilities.Count - 1)
            {
                ClearSpellSocketGUI();
                startIndex++;
                endIndex++;
                LoadSpellSocketGUI(startIndex, endIndex);
                GenerateArrow();
                //NextPageIndex();
                return true;
            }
            return false;
        }

        private bool LoadUpperSocketItem()
        {
            if(startIndex > 0)
            {
                ClearSpellSocketGUI();
                startIndex--;
                endIndex--;
                LoadSpellSocketGUI(startIndex, endIndex);
                GenerateArrow();
                //PreviousPageIndex();
                return true;
            }
            return false;
        }

        private void ClearSpellSocketGUI()
        {
            foreach (Transform child in _spellParent.transform)
            {
                if (child.TryGetComponent(out SpellSocketGUI script))
                {
                    Destroy(child.gameObject);
                }
            }
        }

        public void ReceiveKeys(KeyCode key)
        {
            Debug.Log("spell window on receiver key");

            _spellSockets[_currentIndex].UnHighlightedButton();

            if (key == KeyCode.W)
            {
                _currentIndex -= 1;
                if (_currentIndex < 0)
                {
                    LoadUpperSocketItem();

                    _currentIndex = 0;
                }
                Debug.Log(_currentIndex);


            }
            else if (key == KeyCode.S)
            {
                _currentIndex += 1;

                if (_currentIndex > _spellSockets.Count - 1 )
                {
                    LoadLowerSocketItem();
                    _currentIndex -= 1; 

                }
                Debug.Log(_currentIndex);


            }
            else if (key == KeyCode.Escape)
            {
                this._combatGraphicalHandler.DisplayMainMenu(); 
            }
            else if (key == KeyCode.Space)
            {
                _spellSockets[_currentIndex].CallButtonCallback(); 
            }

            _spellSockets[_currentIndex].HightlightedButton(); 
        }

        //private void SelectingSpellSocketDisplay() {
        //    _spellSockets[_currentIndex].GetComponent<Button>().s
        //}

        public override void OnWindowDisplay(CombatGraphicalHandler graophicalHandler)
        {
            for (int i = 0; i < _spellSockets.Count; i++)
                _spellSockets[i].UnHighlightedButton();

            _currentIndex = 0; 
            _spellSockets[_currentIndex].HightlightedButton();

            this._combatGraphicalHandler = graophicalHandler;
            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);
            SetGraphicMenuActive(true); 
        }

        public override void OnWindowOverlayed()
        {
            for (int i  = 0;  i< _spellSockets.Count; i++)  
                _spellSockets[i].UnHighlightedButton();


            CentralInputReceiver.Instance().RemoveInputReceiverIntoStack(this);

            SetGraphicMenuActive(false); 
        }
    }
}
