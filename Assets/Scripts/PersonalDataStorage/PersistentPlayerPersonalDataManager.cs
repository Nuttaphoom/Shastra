using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Vanaring
{
    public class PersistentPlayerPersonalDataManager : PersistentInstantiatedObject<PersistentPlayerPersonalDataManager>
    {
        // TEMP : Manually Load data from SO for now 
        [SerializeField]
        private PersonalityTraitSO player_personalityTraitSO;

        //[SerializeField]
        //private Trait.Trait_Type trait;

        [HideInInspector]
        public PersonalityTrait player_personalityTrait;

        [SerializeField]
        private Backpack _backpack; 
        
        private void Awake()
        {
            // TO DO : 
            player_personalityTrait = new PersonalityTrait(player_personalityTraitSO);
        }

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.E))
        //    {
        //        Debug.Log(trait + " : " + player_personalityTrait.GetStat(trait));
        //    }
        //    if (Input.GetKeyDown(KeyCode.R))
        //    {
        //        player_personalityTrait.SetStat(trait, player_personalityTrait.GetStat(trait) - 1);
        //        Debug.Log(trait + " : " + player_personalityTrait.GetStat(trait));
        //    }
        //    if (Input.GetKeyDown(KeyCode.T))
        //    {
        //        Debug.Log("Change : " + trait);
        //        trait++;
        //        if ((int)trait >= System.Enum.GetValues(typeof(Trait.Trait_Type)).Length)
        //        {
        //            trait = 0;
        //        }
        //        Debug.Log("To : " + trait);
        //    }
        //    if (Input.GetKeyDown(KeyCode.Y))
        //    {
        //        player_personalityTrait.SetStat(trait, player_personalityTrait.GetStat(trait) + 1);
        //        Debug.Log(trait + " : " + player_personalityTrait.GetStat(trait));
        //    }
        //}
    }
}
