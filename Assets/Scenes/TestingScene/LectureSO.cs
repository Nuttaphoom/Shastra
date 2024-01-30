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
        public string LectureName = "GAM300";
        [SerializeField]
        public int maxPoint = 1000;
        [SerializeField]
        public List<LectureChechpoint> Checkpoint = new List<LectureChechpoint>();
        [SerializeField]
        public List<LectureRequieBoost> lectureRequieBoosts = new List<LectureRequieBoost>();

 
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
