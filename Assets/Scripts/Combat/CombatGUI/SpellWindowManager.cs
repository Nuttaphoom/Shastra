using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private CombatGraphicalHandler _graphicalHandler; 

        private int startIndex = 0;
        private int endIndex = 2;
        private int _currentIndex = 0;

        private List<SpellSocketGUI> _spellSockets = new List<SpellSocketGUI>();
        // Start is called before the first frame update

        private CombatGraphicalHandler _combatGraphicalHandler ; 
        void Awake()
        {

            LoadSpellSocketGUI(startIndex, endIndex);
            _graphicalHandler = _combatEntity.GetComponent<CombatGraphicalHandler>();
            _templatePrefab.gameObject.SetActive(false);  
        }

      

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

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    LoadUpperSocketItem();
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    LoadLowerSocketItem();
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha3))
            //{
            //    ClearSpellSocketGUI();
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha4))
            //{
            //    LoadSpellSocketGUI(startIndex, endIndex);
            //}

        }

      

        public void ReceiveKeys(KeyCode key)
        {
            Debug.Log("spell window on receiver key");

            if (key == KeyCode.W)
            {
                _currentIndex -= 1;
                if (_currentIndex < 0)
                {
                    LoadUpperSocketItem();

                    _currentIndex = 0;
                }

                
            }
            else if (key == KeyCode.S)
            {
                _currentIndex += 1;

                if (_currentIndex > _spellSockets.Count - 1 )
                {
                    LoadLowerSocketItem();
                    _currentIndex -= 1; 

                }
                 

            }else if (key == KeyCode.Escape)
            {
                this._combatGraphicalHandler.DisplayMainMenu(); 
            }
            else if (key == KeyCode.Space)
            {
                _spellSockets[_currentIndex].CallButtonCallback(); 
            }

           
        }

        public override void OnWindowDisplay(CombatGraphicalHandler graophicalHandler)
        {

            this._combatGraphicalHandler = graophicalHandler;
            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);
            SetGraphicMenuActive(true); 
        }

        public override void OnWindowOverlayed()
        {

            CentralInputReceiver.Instance().RemoveInputReceiverIntoStack(this);

            SetGraphicMenuActive(false); 
        }
    }
}
