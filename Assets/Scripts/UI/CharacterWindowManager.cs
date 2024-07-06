using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring 
{
    public class CharacterWindowManager : MonoBehaviour
    {
        //private CombatEntity _combatEntity;
        [SerializeField]
        private CharacterSocketGUI _templatePrefab;
        [SerializeField]
        private Transform _socketVerticalLayout;
        [SerializeField] private GameObject indicatorA;
        [SerializeField] private GameObject indicatorD;
        [SerializeField] private Animator gfxAnim;
        //private GameObject _entities;
        private List<CombatEntity> entities = new List<CombatEntity>();

        [SerializeField]
        private EntityInpectWindowGUI inspectWindow;

        private List<CharacterSocketGUI> characterSocketList = new List<CharacterSocketGUI>();
        private List<CombatEntity> combatEntityList = new List<CombatEntity>();

        private void Start()
        {
            CombatReferee.Instance.SubOnCombatPreparation(SetUpCharacterHUD);
            inspectWindow.SubOnCloseInspectWindow(ShowPanel);
            inspectWindow.SubOnEntityInspect(HidePanel);

        }
        private void SetUpCharacterHUD(Null n)
        {
            entities = CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Ally);
            indicatorA.SetActive(true);
            indicatorD.SetActive(true);
            for (int i = entities.Count-1; i >= 0; i--)
            {
                CharacterSocketGUI newSocket = Instantiate(_templatePrefab, _socketVerticalLayout.transform);
                CombatEntity _cet = entities[i];
                newSocket.transform.localScale = _templatePrefab.transform.localScale;
                newSocket.Init(_cet);
                characterSocketList.Add(newSocket);
                combatEntityList.Add(_cet);
                newSocket.transform.SetAsFirstSibling();
            }
        }

        public void ShowPanel(Null n)
        {
            gfxAnim.Play("SlideUp");
        }

        public void HidePanel(CombatEntity entity)
        {
            gfxAnim.Play("SlideDown");
        }


        #region InTurnSet
        public void SetActiveEntityGUI(CombatEntity entity)
        {
            for (int i = 0; i < combatEntityList.Count ; i++)
            {
                if (combatEntityList[i] == entity)
                {
                    //characterSocketList[i].ToggleTurnStatusDisplay(true);
                    characterSocketList[i].ToggleExpandSizeUI();
                }
            }
        }

        public void DeSetActiveEntityGUI(CombatEntity entity)
        {
            for (int i = 0; i < combatEntityList.Count; i++)
            {
                if (combatEntityList[i] == entity)
                {
                    //characterSocketList[i].ToggleTurnStatusDisplay(false);
                    characterSocketList[i].ToggleShrinkSizeGUI();
                }
            }
        }
        #endregion

        #region Arrow Pointer 
        public void DisplayArrowOnTargetCharacter(List<CombatEntity> entities)
        {
            for (int i = 0; i < combatEntityList.Count; i++)
            {
                if (entities.Contains(combatEntityList[i]))
                {
                    //characterSocketList[i].ToggleTurnStatusDisplay(true);
                    characterSocketList[i].DisplayArrowOnTargetCharacter();
                }else
                {
                    characterSocketList[i].HideArrowOnTargetCharacter() ; 
                }
            }
        }

        public void HideAllArrowTargetCharacter()
        {
            for (int i  = 0; i < characterSocketList.Count; i++)
            {
                characterSocketList[i].HideArrowOnTargetCharacter(); 
            }
        }

       
        #endregion

        #region SizingSet
        public void SetSizeExpandGUI(CombatEntity entity)
        {
            for (int i = 0; i < combatEntityList.Count; i++)
            {
                if (combatEntityList[i] == entity)
                {
                    characterSocketList[i].ToggleExpandSizeUI();
                    return;
                }
            }
        }

        public void SetSizeShrinkGUI(CombatEntity entity)
        {
            for (int i = 0; i < combatEntityList.Count; i++)
            {
                if (combatEntityList[i] == entity)
                {
                    characterSocketList[i].ToggleShrinkSizeGUI();
                    return;
                }
            }
        }
        #endregion
    }
}
