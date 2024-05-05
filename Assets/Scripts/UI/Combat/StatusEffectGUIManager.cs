using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Vanaring
{
    public class StatusEffectGUIManager : MonoBehaviour
    {
        [SerializeField] private EffectIconGUI templateIcon;
        [SerializeField] private GameObject statusBarLayout; 
        private CombatEntity _ownerCombatEntity;
        private Dictionary<string, EffectIconGUI> statusEffectIconDict = new Dictionary<string, EffectIconGUI>();
        private void OnEnable()
        {
            if (_ownerCombatEntity == null)
                return;
            SubAllEvent();
        }

        private void OnDisable()
        {
            UnSubAllEvent();
        }

        private void SubAllEvent()
        {
            _ownerCombatEntity.SubOnStatusEffectApplied(AddNewStatusEffectIcon);
        }

        private void UnSubAllEvent()
        {
            _ownerCombatEntity.UnSubOnStatusEffectApplied(AddNewStatusEffectIcon);
        }

        public void Init(CombatEntity combatEntity)
        {
            _ownerCombatEntity = combatEntity;
            SubAllEvent();
        }

        private void AddNewStatusEffectIcon(EntityStatusEffectPair effect)
        {
            var statusRuntime = effect.StatusRuntime;
            var statusStackID = effect.StatusEffectFactory.Property.StackID();

            statusRuntime.SubOnStatusEffectBreak((bool isExpired) => { RemoveExpiredEffect(statusStackID, isExpired); });

            statusRuntime.SubOnStatusEffectExpire((bool isExpired) => { RemoveExpiredEffect(statusStackID, isExpired); });

            statusRuntime.SubOnTTLUpdate((int ttl) => { UpdateEffectTTL(statusStackID, ttl); });

            if (!statusEffectIconDict.ContainsKey(statusStackID))
            {
                CreateNewEffectIcon(effect, statusStackID);
            }
            else
            {
                Destroy(statusEffectIconDict[statusStackID].gameObject);
                statusEffectIconDict.Remove(statusStackID);

                CreateNewEffectIcon(effect, statusStackID);
            }
        }

        private void CreateNewEffectIcon(EntityStatusEffectPair statusEffData, string stackID)
        {
            EffectIconGUI newEffectIcon = Instantiate(templateIcon, statusBarLayout.transform);
            newEffectIcon.Init(statusEffData);
            newEffectIcon.gameObject.SetActive(true);
            statusEffectIconDict.Add(stackID, newEffectIcon);
        }

        private void RemoveExpiredEffect(string stackID, bool isexpire)
        {
            if (!isexpire)
                return;
            Debug.Log("Destroy effect");
            Destroy(statusEffectIconDict[stackID].gameObject);
            statusEffectIconDict.Remove(stackID);
        }

        private void UpdateEffectTTL(string effect, int currentTTL)
        {
            if (statusEffectIconDict.ContainsKey(effect))
            {
                statusEffectIconDict[effect].SetTimetoLiveText(currentTTL.ToString());
            }
            else
            {
                Debug.Log("No effect gui detect");
            }
        }
    }
}
