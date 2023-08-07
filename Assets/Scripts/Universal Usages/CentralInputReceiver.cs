using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Vanaring_DepaDemo
{
    public class CentralInputReceiver 
    {
        private readonly Dictionary<char, KeyCode> _keycodeCache = new Dictionary<char, KeyCode>();

        private static CentralInputReceiver instance;

        private static Stack<IInputReceiver> _receiverStack ;
        private static IInputReceiver _currentReceiver;

        public void TickUpdate()
        {
            InputSystem.onAnyButtonPress
            .CallOnce(ctrl => Debug.Log(ctrl.name[0]) );
        }
        private KeyCode GetKeyCode(char character)
        {
            // Get from cache if it was taken before to prevent unnecessary enum parse
            KeyCode code;
            if (_keycodeCache.TryGetValue(character, out code)) return code;

            // Cast to it's integer value
            int alphaValue = character;
            code = (KeyCode)Enum.Parse(typeof(KeyCode), alphaValue.ToString());
            _keycodeCache.Add(character, code);

           
            return code;
        }
        public static CentralInputReceiver Instance()
        {
            if (instance == null) { 
                instance = new CentralInputReceiver(); 
            }

            return instance ; 
        }
 

        public void AddInputReceiverIntoStack(IInputReceiver receiver)
        {
            if (_receiverStack.Contains(receiver))
            {
                _receiverStack.Push(receiver); 
            }
        }

        public void RemoveInputReceiverIntoStack()
        {
            _receiverStack.Pop(); 
        }

        public void ClearStack()
        {
            _receiverStack.Clear(); 
        } 
    }


    public interface IInputReceiver
    {
        public void ReceiveKeys(string key);
    }
}
