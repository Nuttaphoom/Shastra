using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public class SpellWindowManager : MonoBehaviour
    {
        [SerializeField]
        private CombatEntity _combatEntity;  

        [SerializeField]
        private SpellSocketGUI _templatePrefab;

        [SerializeField]
        private Transform _socketVerticalLayout;

        private Transform[] SocketEntrySlots;

        // Start is called before the first frame update
        void Awake()
        {
            foreach (SpellAbilitySO spellAbility in _combatEntity.SpellCaster.SpellAbilities)
            {
                SpellSocketGUI newSocket = Instantiate(_templatePrefab,_templatePrefab.transform.position, _templatePrefab.transform.rotation); 
                newSocket.transform.parent = _socketVerticalLayout.transform;
                newSocket.transform.localScale = _templatePrefab.transform.localScale;
                newSocket.Init(spellAbility, _combatEntity) ;
            }
            
            _templatePrefab.gameObject.SetActive(false);  
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
