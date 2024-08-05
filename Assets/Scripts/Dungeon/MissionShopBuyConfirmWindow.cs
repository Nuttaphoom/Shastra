using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Vanaring
{
    public enum ItemAmountDigit
    {
        Tens,
        Units
    }
    public class MissionShopBuyConfirmWindow : MonoBehaviour, IInputReceiver
    {
        private int tenDigit = 0;
        private int unitDigit = 0;
        [SerializeField] private TextMeshProUGUI tenText;
        [SerializeField] private TextMeshProUGUI unitText;
        [SerializeField] private TextMeshProUGUI priceText;
        
        private ItemAmountDigit digit;
        private int itemPrice;

        [SerializeField] private GameObject tensDigitArrow;
        [SerializeField] private GameObject unitsDigitArrow;
        //[SerializeField] private Button cancelButton;
        //[SerializeField] private Button confirmButton;

        private MissionShopWindowGUI _shopWindow;
        private DungeonShopManager _shopManager;
        private int tmpIndex;
        private int totalAmount;
        public void Init(MissionShopWindowGUI window, DungeonShopManager manager, int itemPrice, int index)
        {
            totalAmount = 1;
            tenDigit = 0;
            unitDigit = 1;
            tmpIndex = index;
            _shopWindow = window;
            _shopManager = manager;
            digit = ItemAmountDigit.Units;
            tensDigitArrow.SetActive(false);
            unitsDigitArrow.SetActive(true);
            tenText.text = tenDigit.ToString();
            unitText.text = unitDigit.ToString();
            priceText.text = itemPrice.ToString();
            this.itemPrice = itemPrice;
        }

        public void AdjustAmount(bool isUp)
        {
            switch (digit)
            {
                case ItemAmountDigit.Tens:
                    if (isUp)
                    {
                        if (tenDigit < 9)
                        {
                            tenDigit++;
                        }
                    }
                    else
                    {
                        if (tenDigit > 0)
                        {
                            tenDigit--;
                        }
                    }
                    tenText.text = tenDigit.ToString();
                    break;
                case ItemAmountDigit.Units:
                    if (isUp)
                    {
                        if (unitDigit < 9)
                        {
                            unitDigit++;
                        }
                    }
                    else
                    {
                        if (unitDigit > 0)
                        {
                            unitDigit--;
                        }
                    }
                    unitText.text = unitDigit.ToString();
                    break;
            }
            totalAmount = (tenDigit * 10) + unitDigit;
            priceText.text = (itemPrice * totalAmount).ToString();
            if (PersistentPlayerPersonalDataManager.Instance.GetBackpack.GetCurrentCash >= (itemPrice * totalAmount))
            {
                priceText.color = Color.white;
            }
            else
            {
                priceText.color = Color.red;
            }
        }

        public IEnumerator OpenWindow()
        {
            yield return new WaitForEndOfFrame();
            //cancelButton.Select();
        }
        public void CloseWindow()
        {
            CentralInputReceiver.Instance.RemoveInputReceiverIntoStack(this);
            CentralInputReceiver.Instance.ClearStack();
            CentralInputReceiver.Instance.AddInputReceiverIntoStack(_shopWindow);
            gameObject.SetActive(false);
        }

        public void ReceiveKeys(InputCode key)
        {
            if(key == InputCode.Up)
            {
                AdjustAmount(true);
            }
            else if (key == InputCode.Down)
            {
                AdjustAmount(false);
            }
            else if (key == InputCode.Left)
            {
                digit = ItemAmountDigit.Tens;
                tensDigitArrow.SetActive(true);
                unitsDigitArrow.SetActive(false);
            }
            else if (key == InputCode.Right)
            {
                digit = ItemAmountDigit.Units;
                tensDigitArrow.SetActive(false);
                unitsDigitArrow.SetActive(true);
            }
            else if (key == InputCode.Select)
            {
                if(PersistentPlayerPersonalDataManager.Instance.GetBackpack.GetCurrentCash >= (itemPrice * totalAmount))
                {
                    for (int i = 0; i < totalAmount; i++)
                    {
                        _shopManager.BuyProduct(tmpIndex);
                        _shopWindow.UpdateCurrentCash();
                    }
                }
                //confirmButton.onClick.Invoke();
            }
            else if (key == InputCode.DeSelect)
            {
                CloseWindow();
            }
        }
    }
}