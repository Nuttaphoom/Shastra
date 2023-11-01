using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Vanaring
{
    public enum SignalType
    {
        None,
        SignalA,
        SignalB,
        SignalC,
        SignalD
    }
    public class TimelineSignalBroadcaster : MonoBehaviour
    {
        [SerializeField] DirectorManager directorManager;
        public void RecieveSignal(string signal)
        {
            SignalType broadcastSignal = SignalType.None  ;
            switch (signal)
            {
                case "A":
                    broadcastSignal = SignalType.SignalA ;
                    break;
                case "B":
                    broadcastSignal = SignalType.SignalB ;
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

            if (broadcastSignal == SignalType.None)
            {
                throw new System.Exception("Received signal is invalid");
            }
            directorManager.TransmitSignal(broadcastSignal);

        }
    }
}
