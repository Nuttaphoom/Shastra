using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class DungeonShopManager : MonoBehaviour
    {

        [Serializable] 
        public struct ProductData
        {
            public string DisplayedProductName;
            public int Cost;
            public EventRewardData Reward;
        }

        [SerializeField] 
        private List<ProductData> _productsData = new List<ProductData>();

        public List<ProductData> ProductsData { get {  return _productsData; } }

        /// <summary>
        /// please change parameter to whatever you want to link ui with this function
        /// </summary>
        /// <param name="productName"></param>
        public void BuyProduct(int productIndex)
        {
            ProductData data = _productsData[productIndex];

            if (PersistentPlayerPersonalDataManager.Instance.GetBackpack.GetCurrentCash < data.Cost)
            {
                return;
            }


            StartCoroutine(BuyProductCO(data));
        }

        private IEnumerator BuyProductCO(ProductData data)
        {
            PersistentPlayerPersonalDataManager.Instance.GetBackpack.ModifyCash(-data.Cost);//        } 

            yield return data.Reward.GetReward(); 
        }
    }
}
