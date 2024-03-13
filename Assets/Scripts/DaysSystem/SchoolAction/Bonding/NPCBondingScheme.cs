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
    public struct BondingRewardData
    {
        public int bondLevel;
        public int curExp;
        public int capExp;
        public string characterName;
        public Sprite characterSprite;
    }

    [Serializable]
    public class NPCBondingScheme : ISchoolAction
    {
        private CharacterRelationshipDataSO _characterRelationshipDataSO;
        private BondingAnimationGO _bondingAnimationGO ;
        private RelationshipHandler _relationshipHandler;

        [SerializeField] private GameObject bondingRewardDisplayerPanel;

        public void PlayBondingScheme(CharacterRelationshipDataSO characterRelationshipDataSO)
        {
            _characterRelationshipDataSO = characterRelationshipDataSO;
            OnPerformAcivity();
        }

        public void OnPerformAcivity()
        {
            Debug.Log("on perform activity"); 
            _relationshipHandler = PersistentPlayerPersonalDataManager.Instance.RelationshipHandler;
            string characterName = _characterRelationshipDataSO.GetCharacterName; 
            int currentLevel = _relationshipHandler.GetCurrentBondLevel(characterName);
            int currentExp = _relationshipHandler.GetCurrentRelationshipEXP(characterName);

            
            _bondingAnimationGO = MonoBehaviour.Instantiate(_characterRelationshipDataSO.GetBondingAnimationGO(currentLevel, currentExp));

           _bondingAnimationGO.StartCoroutine(_bondingAnimationGO.PlayCutscene(this) ) ;

        }

        public IEnumerator PostPerformActivity()
        {
            Debug.Log("Post perform");

            //Display reward displayer 
            SubmitActionReward();

            yield return new WaitForSeconds(1);
            bondingRewardDisplayerPanel = MonoBehaviour.Instantiate(PersistentAddressableResourceLoader.Instance.LoadResourceOperation<GameObject>("BondingRewardDisplayerPanel"));
            bondingRewardDisplayerPanel.GetComponent<BondingRewardDisplayer>().SetReceivedReward(new BondingRewardData()
            {
                bondLevel = PersistentPlayerPersonalDataManager.Instance.RelationshipHandler.GetCurrentBondLevel(_characterRelationshipDataSO.GetCharacterName),
                curExp = PersistentPlayerPersonalDataManager.Instance.RelationshipHandler.GetCurrentRelationshipEXP(_characterRelationshipDataSO.GetCharacterName),
                capExp = PersistentPlayerPersonalDataManager.Instance.RelationshipHandler.GetRelationshipCapEXP(_characterRelationshipDataSO.GetCharacterName),
                characterName = _characterRelationshipDataSO.GetCharacterName,
                characterSprite = _characterRelationshipDataSO.GetCharacterSprite
            });
            yield return bondingRewardDisplayerPanel.GetComponent<BondingRewardDisplayer>().SettingUpNumber();
            PersistentActiveDayDatabase.Instance.OnPostPerformSchoolAction();
            yield return null; 
        }

        public void SubmitActionReward()
        {
            _relationshipHandler.ProgressRelationship(_characterRelationshipDataSO.GetCharacterName,1) ;
        }
    }
}
