using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Vanaring
{
    [Serializable]
    public class LectureChechpoint
    {
        [SerializeField]
        private int cp_requirepoint;
        [SerializeField]
        private string cp_reward;

        [NonSerialized]
        private bool cp_received;

        public void SetReceived(bool fact) { cp_received = fact; }

        public int RequirePoint => cp_requirepoint;
        public string Reward => cp_reward;
        public bool Received => cp_received;
    }
}
