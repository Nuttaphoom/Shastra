using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Vanarin;


namespace Vanaring 
{
    [CreateAssetMenu(fileName = "CharacterRelationshipDataSO", menuName = "ScriptableObject/Relationship/CharacterRelationshipDataSO")]
    public class CharacterRelationshipDataSO : ScriptableObject
    {
        [SerializeField] 
        private CharacterSheetSO _characterSheetSO ;
        [SerializeField]
        private Sprite _characterSprite;

 
        public Sprite GetCharacterSprite => _characterSprite;


        [Serializable]
        struct BondingDataStruct
        {
            public int CorrectLevel ;

            public List<BondingAnimationGO> BondingAnimationGOs;
        }

        [Serializable]
        struct HangoutDataStruct
        {
            public int CorrectLevel;
            public List<HangoutCutsceneSceneDataSO> HangoutCutsceneData ;
        }

        [SerializeField]
        private List<BondingDataStruct> _bondsData ;

        [SerializeField]
        private List<HangoutDataStruct> _hangoutData;  

        public BondingAnimationGO GetBondingAnimationGO (int bondLevel, int exp)
        {
            if (bondLevel > _bondsData.Count )
                throw new Exception("bondLevel : " + bondLevel + " excess the size of _bondsData which is " + _bondsData.Count);
            if (bondLevel > _bondsData.Count)
                throw new Exception("bondLevel : " + bondLevel + " excess the size of _bondsData which is " + _bondsData.Count);

            foreach (var bond in _bondsData)
            {
                if (bond.CorrectLevel == bondLevel)
                {
                    if (bond.BondingAnimationGOs.Count <= exp )
                        throw new Exception("BondingAnimationGOs.count (" + bond.BondingAnimationGOs.Count + " ) is less than given exp ( " + exp + " )" );

                    return bond.BondingAnimationGOs[exp ];
                }
            }

            throw new Exception();
        }

        /// <summary>
        /// Function to reurn correct scene to load for hanging out 
        /// current bond level start at 1
        /// </summary>
        /// <param name="currentBondLevel" ></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        //public BondingAnimationGO GetHangoutCutsceneData(int currentBondLevel)
        //{
        //    if (currentBondLevel > _hangoutData.Count)
        //        throw new Exception("bondLevel : " + currentBondLevel + " excess the size of _hangoutData which is " + _hangoutData.Count);
        //    if (currentBondLevel > _hangoutData.Count)
        //        throw new Exception("bondLevel : " + currentBondLevel + " excess the size of _hangoutData which is " + _hangoutData.Count);

        //    foreach (var bond in _hangoutData)
        //    {
        //        if (bond.CorrectLevel == bondLevel)
        //        {
        //            if (bond.BondingAnimationGOs.Count <= exp)
        //                throw new Exception("BondingAnimationGOs.count (" + bond.BondingAnimationGOs.Count + " ) is less than given exp ( " + exp + " )");

        //            return bond.BondingAnimationGOs[exp];
        //        }
        //    }

        //    throw new Exception();
        //}

        public string GetCharacterName
        {
            get
            {
                if (_characterSheetSO == null)
                    throw new Exception("_characterSheetSO hasn't never been assigned"); 

                return _characterSheetSO.CharacterName; 
            }
        }

        //public string GetCharacterSprite
        //{
        //    get
        //    {
        //        if (_characterSheetSO == null)
        //            throw new Exception("_characterSheetSO hasn't never been assigned");

        //        return _characterSheetSO.Char;
        //    }
        //}


    }
}
