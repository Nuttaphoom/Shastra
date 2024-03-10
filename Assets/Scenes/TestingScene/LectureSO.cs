using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "LectureSO", menuName = "ScriptableObject/LectureSO")]
    public class LectureSO : ScriptableObject
    {
        [SerializeField]
        private LectureParticipationScheme _participationScheme ;
        [SerializeField]
        public int maxPoint = 1000;
        [SerializeField]
        public List<LectureChechpoint> Checkpoint = new List<LectureChechpoint>();
        [SerializeField]
        public List<LectureRequieBoost> lectureRequieBoosts = new List<LectureRequieBoost>();

        [SerializeField] 
        private DescriptionBaseField description ;

        public Sprite GetLectureLogoIcon => description.FieldImage;
        public string GetLectureName => description.FieldName;
        public string GetLectureDestcription => description.FieldDescription;
        public List<LectureRequieBoost> GetBooster => lectureRequieBoosts;

        public LectureParticipationScheme GetParticipationSchemeTemplateObj => _participationScheme; 

    }

    [Serializable]
    public class LectureRequieBoost
    {
        [SerializeField]
        private Trait.Trait_Type trait;
        [SerializeField]
        private int requirelevel;

        public Trait.Trait_Type GetTrait => trait;
        public int RequireLevel => requirelevel;
    
    }
}
