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
        private string LectureName = "GAM300";
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
    
    }

    [Serializable]
    public class LectureRequieBoost
    {
        [SerializeField]
        private Trait.Trait_Type trait;
        [SerializeField]
        private int requirelevel;
    }
}
