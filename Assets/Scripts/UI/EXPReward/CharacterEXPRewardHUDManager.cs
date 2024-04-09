using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class CharacterEXPRewardHUDManager : MonoBehaviour
    {
        [SerializeField] private CharacterEXPSocketGUI socketTemplate;
        [SerializeField] private GameObject hrzLayout;
        private List<RuntimeCombatMemberData> memberList = new List<RuntimeCombatMemberData>();

        private void Start()
        {
            //StartCoroutine(LoadCharacterEXPGainWindow());
        }


        public IEnumerator LoadCharacterEXPGainWindow()
        {
            if (PersistentPlayerPersonalDataManager.Instance.CombatMemberDataLocator.GetRuntimeCombatMembers.Count != 0)
            {
                memberList = PersistentPlayerPersonalDataManager.Instance.CombatMemberDataLocator.GetRuntimeCombatMembers;
            }
            else
            {
                Debug.Log("Can't load member");
            }
            foreach (RuntimeCombatMemberData member in memberList)
            {
                CharacterEXPSocketGUI newSocket = Instantiate(socketTemplate, hrzLayout.transform);
                newSocket.Init(member);
                newSocket.gameObject.SetActive(true);
            }
            socketTemplate.gameObject.SetActive(false);
            yield return null;
        }
    }
}
