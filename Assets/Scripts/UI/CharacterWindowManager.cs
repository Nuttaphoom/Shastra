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

        private void Awake()
        {
            if(_entities.transform.childCount == 0 || _combatEntity == null || _templatePrefab == null)
            {
                return;
            }

            for (int i = 0; i < _entities.transform.childCount; i++)
            {
                CharacterSocketGUI newSocket = Instantiate(_templatePrefab, _templatePrefab.transform.position, _templatePrefab.transform.rotation);
                CombatEntity _cet = _entities.transform.GetChild(i).GetComponent<CombatEntity>();
                newSocket.transform.parent = _socketVerticalLayout.transform;
                newSocket.transform.localScale = _templatePrefab.transform.localScale;
                newSocket.Init(Random.Range(0, 100), _entities.transform.GetChild(i).name, _cet);
            }
        }
    }
}
