using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class MissionPartyHUDManager : MonoBehaviour, ISceneLoaderWaitForSignal
    {
        [SerializeField] private PartyCharacterSocketGUI socketTemplate;
        [SerializeField] private GameObject horizontalLayout;
        private List<PartyCharacterSocketGUI> socketList = new List<PartyCharacterSocketGUI>();

        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            yield return null; 
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            //Debug.LogWarning("Mission Party HUD manager is disable on purpose");
            StartCoroutine(PersistentTutorialManager.Instance.CheckTuitorialNotifier("DungeonExplain"));
            socketTemplate.gameObject.SetActive(true);
            foreach (RuntimePartyMember member in DungeonManagerSingleton.Instance.MissionManager.DungeonPartyHandler.PartyMembers)
            {
                PartyCharacterSocketGUI newSocket = Instantiate(socketTemplate, horizontalLayout.transform);
                newSocket.Init(member);
                newSocket.gameObject.SetActive(true);
                socketList.Add(newSocket);
            }
            socketTemplate.gameObject.SetActive(false);

            yield return null; 
        }
 
    }
}
