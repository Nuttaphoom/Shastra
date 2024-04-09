using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class MissionPartyHUDManager : MonoBehaviour
    {
        [SerializeField] private PartyCharacterSocketGUI socketTemplate;
        [SerializeField] private GameObject horizontalLayout;
        private List<PartyCharacterSocketGUI> socketList = new List<PartyCharacterSocketGUI>();

        private void Start()
        {
            Debug.LogWarning("Mission Party HUD manager is disable on purpose") ;
            //socketTemplate.gameObject.SetActive(true);
            //foreach (RuntimePartyMember member in MissionManagerSingleton.Instance.DungeonPartyHandler.PartyMembers)
            //{
            //    PartyCharacterSocketGUI newSocket = Instantiate(socketTemplate, horizontalLayout.transform);
            //    newSocket.Init(member);
            //    newSocket.gameObject.SetActive(true);
            //    socketList.Add(newSocket);
            //}
            //socketTemplate.gameObject.SetActive(false);
        }
    }
}
