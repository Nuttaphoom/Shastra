using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring 
{
    public class EnergyBarOnSpellPanel : MonoBehaviour
    {
        [SerializeField]
        private CombatEntity _combatEntity;
        [SerializeField]
        private Image _lightBar;
        [SerializeField]
        private TextMeshProUGUI _lightNum;
        [SerializeField]
        private TextMeshProUGUI _darkNum;

        private float lightVal;
        private float darkVal;

        // Start is called before the first frame update
        void Start()
        {
            DisplayEnergy();
        }
        private void OnEnable()
        {
            Debug.Log("On Enable");
            lightVal = _combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy);
            darkVal = _combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy);
            DisplayEnergy();
        }
        private void DisplayEnergy()
        {
            //Debug.Log("CALLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL ME");
            _lightBar.fillAmount = lightVal/100;
            _lightNum.text = lightVal.ToString();
            _darkNum.text = darkVal.ToString();
        }
    }
}
