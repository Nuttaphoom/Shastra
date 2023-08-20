using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public class CharacterWindowManager : MonoBehaviour
    {
        [SerializeField]
        CombatEntity _combatEntity;
        [SerializeField]
        private CharacterSocketGUI _templatePrefab;
        [SerializeField]
        private Transform _socketVerticalLayout;
        [SerializeField]
        private GameObject _entities;

        private List<CharacterSocketGUI> characterSocketList = new List<CharacterSocketGUI>();
        private List<CombatEntity> combatEntityList = new List<CombatEntity>();

   
        private void Awake()
        {
            for (int i = 0; i < _entities.transform.childCount; i++)
            {
                CharacterSocketGUI newSocket = Instantiate(_templatePrefab, _templatePrefab.transform.position, _templatePrefab.transform.rotation);
                CombatEntity _cet = _entities.transform.GetChild(i).GetComponent<CombatEntity>();
                newSocket.transform.parent = _socketVerticalLayout.transform;
                newSocket.transform.localScale = _templatePrefab.transform.localScale;
                newSocket.Init(Random.Range(0, 100), _entities.transform.GetChild(i).name, _cet);
                characterSocketList.Add(newSocket);
                combatEntityList.Add(_cet);
                newSocket.transform.SetAsFirstSibling();
            }
        }

     

        public void SetActiveEntityGUI(CombatEntity entity)
        {
            for (int i = 0; i < combatEntityList.Count ; i++)
            {
                if (combatEntityList[i] == entity)
                {
                    characterSocketList[i].ToggleOnTurnHighlightDisplay(true);
                    characterSocketList[i].ToggleTurnStatusDisplay(true);
                }
            }
        }

        public void DeSetActiveEntityGUI(CombatEntity entity)
        {
            for (int i = 0; i < combatEntityList.Count  ; i++)
            {
                if (combatEntityList[i] == entity)
                {
                    characterSocketList[i].ToggleOnTurnHighlightDisplay(false);
                    characterSocketList[i].ToggleTurnStatusDisplay(false);
                }
            }
        }
    }
}
