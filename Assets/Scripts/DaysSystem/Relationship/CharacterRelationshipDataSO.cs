using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
 

namespace Vanaring 
{
    [CreateAssetMenu(fileName = "CharacterRelationshipDataSO", menuName = "ScriptableObject/Relationship/CharacterRelationshipDataSO")]
    public class CharacterRelationshipDataSO : ScriptableObject
    {
        [SerializeField] 
        private CharacterSheetSO _characterSheetSO ;

        [Serializable]
        struct BondingDataStruct
        {
            public int CorrectLevel ;

            public List<BondingAnimationGO> BondingAnimationGOs;
}

        [SerializeField]
        private List<BondingDataStruct> _bondsData ; 

        public BondingAnimationGO GetBondingAnimationGO (int bondLevel, int exp)
        {
            //Because Relationship exp start with 1
            exp = exp - 1;

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

        public string GetCharacterName
        {
            get
            {
                if (_characterSheetSO == null)
                    throw new Exception("_characterSheetSO hasn't never been assigned"); 

                return _characterSheetSO.CharacterName; 
            }
        }


    }
}
