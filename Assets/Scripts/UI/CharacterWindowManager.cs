using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring 
{
    public class CharacterWindowManager : MonoBehaviour
    {
        [SerializeField]
        private CombatEntity _combatEntity;
        [SerializeField]
        private CharacterSocketGUI _templatePrefab;
        [SerializeField]
        private Transform _socketVerticalLayout;
        [SerializeField]
        private GameObject _entities;
        private List<CombatEntity> entities = new List<CombatEntity>();

        private List<CharacterSocketGUI> characterSocketList = new List<CharacterSocketGUI>();
        private List<CombatEntity> combatEntityList = new List<CombatEntity>();

   
        private void Start()
        {
            entities = CombatReferee.instance.GetCompetatorsBySide(ECompetatorSide.Ally);
            for (int i = 0; i < entities.Count; i++)
            {
                CharacterSocketGUI newSocket = Instantiate(_templatePrefab, _templatePrefab.transform.position, _templatePrefab.transform.rotation);
                //CombatEntity _cet = _entities.transform.GetChild(i).GetComponent<CombatEntity>();
                CombatEntity _cet = entities[i];
                newSocket.transform.parent = _socketVerticalLayout.transform;
                newSocket.transform.localScale = _templatePrefab.transform.localScale;
                newSocket.Init(_cet);
                characterSocketList.Add(newSocket);
                combatEntityList.Add(_cet);
                newSocket.transform.SetAsFirstSibling();
            }
        }


        #region InTurnSet
        public void SetActiveEntityGUI(CombatEntity entity)
        {
            for (int i = 0; i < combatEntityList.Count ; i++)
            {
                if (combatEntityList[i] == entity)
                {
                    characterSocketList[i].ToggleTurnStatusDisplay(true);
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
                    characterSocketList[i].ToggleTurnStatusDisplay(false);
                    characterSocketList[i].ToggleShrinkSizeGUI();
                }
            }
        }
        #endregion

        #region HighlightSet
        public void SetHighlightActiveEntity(CombatEntity entity)
        {
            for (int i = 0; i < combatEntityList.Count; i++)
            {
                if (combatEntityList[i] == entity)
                {
                    characterSocketList[i].ToggleOnTurnHighlightDisplay(true);
                    return;
                }
            }
        }

        public void SetUnHighlightActiveEntity(CombatEntity entity)
        {
            for (int i = 0; i < combatEntityList.Count; i++)
            {
                if (combatEntityList[i] == entity)
                {
                    characterSocketList[i].ToggleOnTurnHighlightDisplay(false);
                    return; 
                }
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
