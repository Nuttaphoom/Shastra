using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public class SpellWindowManager : MonoBehaviour, IInputReceiver 
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

        // Start is called before the first frame update
        void Awake()
        {

            LoadSpellSocketGUI(startIndex, endIndex);
            _graphicalHandler = _combatEntity.GetComponent<CombatGraphicalHandler>();
            _templatePrefab.gameObject.SetActive(false);  
        }

        private void OnEnable()
        {
            TakeInputControl();
        }

        private void OnDisable()
        {
            ReleaseInputControl(); 
        }

        private void LoadSpellSocketGUI(int start, int end)
        {
            int tmpNum = 0;
            int tmpSlotIndex = 0;
            foreach (SpellAbilitySO spellAbility in _combatEntity.SpellCaster.SpellAbilities)
            {
                if (tmpNum >= start && tmpNum <= end)
                {
                    SpellSocketGUI newSocket = Instantiate(_templatePrefab, _spellParent.transform);
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

        private void LoadLowerSocketItem()
        {
            if(endIndex < _combatEntity.SpellCaster.SpellAbilities.Count - 1)
            {
                ClearSpellSocketGUI();
                startIndex++;
                endIndex++;
                LoadSpellSocketGUI(startIndex, endIndex);
            }
        }

        private void LoadUpperSocketItem()
        {
            if(startIndex > 0)
            {
                ClearSpellSocketGUI();
                startIndex--;
                endIndex--;
                LoadSpellSocketGUI(startIndex, endIndex);
            }
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
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ClearSpellSocketGUI();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                LoadSpellSocketGUI(startIndex, endIndex);
            }

        }

        public void TakeInputControl()
        {
            Debug.Log("take control");

            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);
        }

        public void ReleaseInputControl()
        {
            Debug.Log("disable control");

            CentralInputReceiver.Instance().RemoveInputReceiverIntoStack(this);
        }

        public void ReceiveKeys(KeyCode key)
        {
            if (key == KeyCode.W)
            {
                LoadUpperSocketItem() ;
            }
            else if (key == KeyCode.S)
            {
                LoadLowerSocketItem() ;
            }else if (key == KeyCode.Escape)
            {
                _graphicalHandler.EnableGraphicalElements(); 


            }
        }
    }
}
