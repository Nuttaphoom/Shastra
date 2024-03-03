using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "TuitorialDatabaseSO", menuName = "ScriptableObject/Database/TuitorialDatabaseSO")]

    public class TuitorialDatabaseSO  : ScriptableObject
    {
        [Serializable]
        public class TuitorialInstanceData
        {
            public CutsceneDirector TuitorialCutscene;
            public string TutorialNotifyKey;
        }

        [SerializeField]
        private List<TuitorialInstanceData> _tuitorialInstanceDatas = new List<TuitorialInstanceData>(); 

        public TuitorialInstanceData GetTuitorialInstanceData(string tutorialKey)
        {
            foreach (var data in _tuitorialInstanceDatas)
            {
                if (data.TutorialNotifyKey == tutorialKey)
                    return data; 
            }

            return null; 
        }
    }
}
