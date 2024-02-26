using System;
using System.Collections;
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
        private RelationshipHandler _relationshipHandler;

        public void PlayBondingScheme(CharacterRelationshipDataSO characterRelationshipDataSO)
        {
            _characterRelationshipDataSO = characterRelationshipDataSO;
            OnPerformAcivity();
        }

        public void OnPerformAcivity()
        {
            _relationshipHandler = PersistentPlayerPersonalDataManager.Instance.RelationshipHandler;
            string characterName = _characterRelationshipDataSO.GetCharacterName; 
            int currentLevel = _relationshipHandler.GetCurrentBondLevel(characterName);
            int currentExp = _relationshipHandler.GetCurrentRelationshipEXP(characterName);
             
            _bondingAnimationGO = MonoBehaviour.Instantiate(_characterRelationshipDataSO.GetBondingAnimationGO(currentLevel, currentExp));

           _bondingAnimationGO.StartCoroutine(_bondingAnimationGO.PlayCutscene(this) ) ;

        }

        public IEnumerator PostPerformActivity()
        {
            //Display reward displayer 
            //SubmitActionReward() ;
            yield return new WaitForSeconds(1);
            PersistentActiveDayDatabase.Instance.OnPostPerformSchoolAction();
            yield return null; 
            //yield return null; 
            //Display action reward 
            //throw new NotImplementedException();
        }

        public void SubmitActionReward()
        {
            _relationshipHandler.ProgressRelationship(_characterRelationshipDataSO.GetCharacterName,1) ;
        }
    }
}
