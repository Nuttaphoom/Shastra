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
            

            for (int i = 0; i < _entities.transform.childCount; i++)
            {
                CombatEntity _cet = _entities.transform.GetChild(i).GetComponent<CombatEntity>();

                CharacterSocketGUI newSocket = Instantiate(_templatePrefab, _templatePrefab.transform.position, _templatePrefab.transform.rotation);
                newSocket.transform.parent = _socketVerticalLayout.transform;
                newSocket.transform.localScale = _templatePrefab.transform.localScale;
                newSocket.Init(Random.Range(0, 100), _entities.transform.GetChild(i).name, _cet);
            }
        }
    }
}
