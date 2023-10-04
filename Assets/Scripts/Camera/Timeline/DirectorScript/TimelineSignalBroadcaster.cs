using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Vanaring
{
    public enum SignalType
    {
        SignalA,
        SignalB,
        SignalC,
        SignalD
    }
    public class TimelineSignalBroadcaster : MonoBehaviour
    {
        
        public void RecieveSignal(string signal)
        {
            switch (signal)
            {
                case "A":
                    Debug.Log(SignalType.SignalA);
                    break;
                //case SignalType.SignalB:
                //    Debug.Log("Signal B");
                //    break;
                //case SignalType.SignalC:
                //    Debug.Log("Signal C");
                //    break;
                //case SignalType.SignalD:
                //    Debug.Log("Signal D");
                //    break;
            }
        }
    }
}
