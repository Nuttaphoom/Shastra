using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public class ItemWindowManager : MonoBehaviour
    {
        [SerializeField]
        private CombatEntity _combatEntity;

        [SerializeField]
        private ItemSocketGUI _templatePrefab;

        [SerializeField]
        private Transform _socketVerticalLayout;

        private List<ItemAbilitySO> _inventory;
        private List<ItemSocketGUI> _GUIinventory;
        // Start is called before the first frame update
        void Awake()
        {
            _GUIinventory = new List<ItemSocketGUI>();
            _inventory = _combatEntity.ItemUser.ItemAbilities;
            foreach (ItemAbilitySO itemAbility in _inventory)
            {
                if (ItemIsContained(itemAbility))
                {
                    continue;
                }
                ItemSocketGUI newSocket = Instantiate(_templatePrefab, _templatePrefab.transform.position, _templatePrefab.transform.rotation);
                newSocket.transform.parent = _socketVerticalLayout.transform;
                newSocket.transform.localScale = _templatePrefab.transform.localScale;
                newSocket.Init(itemAbility, _combatEntity);
                _GUIinventory.Add(newSocket);
            }

            _templatePrefab.gameObject.SetActive(false);
        }

        private bool ItemIsContained(ItemAbilitySO itemAbility)
        {
            bool found = false;
            foreach (ItemSocketGUI eSocket in _GUIinventory)
            {
                if (eSocket.IsSameItem(itemAbility.AbilityName.ToString()))
                {
                    eSocket.AddItem(1);
                    found = true;
                    break;
                }
            }
            return found;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
