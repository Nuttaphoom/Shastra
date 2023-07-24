using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring_DepaDemo
{
    public class BarScaler : MonoBehaviour
    {
        [SerializeField]
        private CombatEntity _owner  ;

        public Image image;
        public Image targetpoint;
        private int lightNum = 50;
        private int darkNum = 50;
        private int lightNumA = 50;
        private int darkNumA = 50;
        public TextMeshProUGUI lightNumImg;
        public TextMeshProUGUI darkNumImg;
        private int _size;
        private int maxBar = 100;
        private float mul = 2.2f;

        private void Awake()
        {
            _owner.SpellCaster.SubOnModifyEnergy(OnEnergyModified); 
        }

        private void OnEnergyModified(RuntimeMangicalEnergy.EnergySide side , int val)
        { 

            if (side == RuntimeMangicalEnergy.EnergySide.LightEnergy)
            {
                increaseScale(val);
            }else
            {
                decreaseScale(val);
            }


        }

        private void Start()
        {
            _size = 110;
            lightNumImg.text = lightNum.ToString();
            darkNumImg.text = darkNum.ToString();
            ResizeImage(_size);
        }
        private void Update()
        {
            
        }
        public void ResizeImage(float newSize)
        {
            RectTransform rectTransform = image.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(newSize, newSize);
        }
        public void increaseScale(int num)
        {
            _size = _size + 10;
            lightNumA += num;
            darkNumA -= num;
            StartCoroutine(increaseDark());
            ResizeImage(_size);
        }
        public void decreaseScale(int num)
        {
            _size = _size - num;
            lightNumA -= num;
            darkNumA += num ;
            StartCoroutine(increaseDark());
            ResizeImage(_size);
        }

        private IEnumerator increaseDark()
        {
            while (lightNum < lightNumA)
            {
                lightNum += 1;
                darkNum -= 1;
                lightNumImg.text = lightNum.ToString();
                darkNumImg.text = darkNum.ToString();
                yield return new WaitForSeconds(0.1f);
            }
            while (darkNum < darkNumA)
            {
                darkNum += 1;
                lightNum -= 1;
                lightNumImg.text = lightNum.ToString();
                darkNumImg.text = darkNum.ToString();
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }

        private IEnumerator increaseLight()
        {
            
            yield return null;
        }


        //public void target()
        //{
        //    if(targetpoint.gameObject.active == tru[e)
        //    targetpoint.gameObject.SetActive(true);
        //}
    }
}
