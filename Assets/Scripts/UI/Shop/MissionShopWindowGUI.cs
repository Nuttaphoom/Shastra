using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using TMPro;

namespace Vanaring
{
    public class MissionShopWindowGUI : MonoBehaviour, IInputReceiver, ISceneLoaderWaitForSignal
    {
        [SerializeField] private DungeonShopManager _dungeonShopManager;
        [SerializeField] private TextMeshProUGUI currentCashText;
        [SerializeField] private ShopItemSocketGUI shopItemSocketTemplate;
        [SerializeField] private GameObject itemListTransform;
        [SerializeField]
        private AssetReferenceT<EssentialSceneDataSO> _base_missionScene;
        private List<ShopItemSocketGUI> shopItemSocketList = new List<ShopItemSocketGUI>();
        private int selectingIndex;

        private void Start()
        {
            if(_dungeonShopManager == null)
            {
                Debug.LogError("no shop manager can be founded!");
            }
            selectingIndex = 0;
            UpdateCurrentCash();
            CentralInputReceiver.Instance.AddInputReceiverIntoStack(this);
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            
            yield return SetupShop();

            //yield return PersistentPlayerPersonalDataManager.
        }

        private IEnumerator SetupShop()
        {
            int i = 0;
            foreach (DungeonShopManager.ProductData itemData in _dungeonShopManager.ProductsData)
            {
                int index = i;
                ShopItemSocketGUI newSocket = Instantiate(shopItemSocketTemplate, itemListTransform.transform);
                newSocket.Init(itemData);
                newSocket.ItemButton.onClick.AddListener(() => _dungeonShopManager.BuyProduct(index));
                newSocket.ItemButton.onClick.AddListener(UpdateCurrentCash);
                newSocket.gameObject.SetActive(true);
                newSocket.OnDeSelectSocket();
                shopItemSocketList.Add(newSocket);
                i++;
            }
            shopItemSocketList[selectingIndex].OnSelectSocket();
            shopItemSocketTemplate.gameObject.SetActive(false);



            yield return new WaitForEndOfFrame();
        }
        public void UpdateCurrentCash()
        {
            currentCashText.text = PersistentPlayerPersonalDataManager.Instance.GetBackpack.GetCurrentCash.ToString();
        }

        public void ReceiveKeys(InputCode key)
        {
            if (key == InputCode.Down)
            {
                foreach (ShopItemSocketGUI socket in shopItemSocketList)
                {
                    socket.OnDeSelectSocket();
                }
                if (selectingIndex < shopItemSocketList.Count -1)
                {
                    selectingIndex++;
                }
                shopItemSocketList[selectingIndex].OnSelectSocket();
            }
            if (key == InputCode.Up)
            {
                foreach (ShopItemSocketGUI socket in shopItemSocketList)
                {
                    socket.OnDeSelectSocket();
                }
                if(selectingIndex > 0)
                {
                    selectingIndex--;
                }
                shopItemSocketList[selectingIndex].OnSelectSocket();
            }
            if(key == InputCode.Item)
            {
                shopItemSocketList[selectingIndex].ItemButton.onClick.Invoke();
            }
            if (key == InputCode.Select)
            {
                EnterDungeon();
            }
        }

        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            yield return new WaitForEndOfFrame();
        }
        [ContextMenu("GOGO")]
        public void EnterDungeon()
        {
            CentralInputReceiver.Instance.RemoveInputReceiverIntoStack(this);
            SceneDataSO newScene = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SceneDataSO>(_base_missionScene);
            PersistentSceneLoader.Instance.LoadGeneralScene(newScene);
        }
        
    }
}
