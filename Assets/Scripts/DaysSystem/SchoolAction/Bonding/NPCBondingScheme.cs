using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Vanaring.Assets.Scripts.DaysSystem.SchoolAction;

namespace Vanaring 
{
    [Serializable]
    public class NPCBondingScheme : ISchoolAction
    {
        private CharacterRelationshipDataSO _characterRelationshipDataSO;
        private BondingAnimationGO _bondingAnimationGO ; 
        public void OnPerformAcivity()
        {
            RelationshipHandler relationshipHandler = PersistentPlayerPersonalDataManager.Instance.RelationshipHandler;
            string characterName = _characterRelationshipDataSO.GetCharacterName; 
            int currentLevel = relationshipHandler.GetCurrentBondLevel(characterName);
            int currentExp = relationshipHandler.GetCurrentRelationshipEXP(characterName);
             
            _bondingAnimationGO = MonoBehaviour.Instantiate(_characterRelationshipDataSO.GetBondingAnimationGO(currentLevel, currentExp));

           _bondingAnimationGO.StartCoroutine(_bondingAnimationGO.PlayCutscene(PostPerformActivity) ) ; 
        
        }


        public void PlayingBondingScheme(CharacterRelationshipDataSO characterRelationshipDataSO)
        {
            _characterRelationshipDataSO = characterRelationshipDataSO ;
            OnPerformAcivity(); 
        }

        public void PostPerformActivity()
        {
            //Display reward displayer 
            throw new NotImplementedException();
        }
    }
}
