using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.TextCore.Text;

namespace Vanaring 
{
    public enum InputCode 
    { 
        Up = 0, 
        Down = 1, 
        Left = 2, 
        Right = 3, 
        Select = 4, 
        Item = 5, 
        Skill = 6,

        T = 7,
        Escape = 8,
        C = 9,
        DeSelect = 10,
    }

    public class CentralInputReceiver : PersistentInstantiatedObject<CentralInputReceiver>
    {
        private  Dictionary<char, KeyCode> _keycodeCache = new Dictionary<char, KeyCode>();

        private static Stack<IInputReceiver> _receiverStack = new Stack<IInputReceiver>();

        public CentralInputReceiver()
        {
            _keycodeCache = new Dictionary<char, KeyCode>();
            _receiverStack = new Stack<IInputReceiver>(); 
        } 
         ~CentralInputReceiver()
        {
            _receiverStack.Clear(); 
        } 

        private void TransmitInput(InputCode key)
        {
            if (_receiverStack.Count > 0) {
                _receiverStack.Peek().ReceiveKeys(key);
            } 
        }

        private void OnNavigate(InputValue value)
        {
            Vector2 inputValue = value.Get<Vector2>();
            Debug.Log(inputValue);

            if (inputValue.Equals(new Vector2(1.0f,0.0f)))
            {
                TransmitInput(InputCode.Right);
            }
            else if (inputValue.Equals(new Vector2(0.0f, 1.0f)))
            {
                TransmitInput(InputCode.Up);
            }
            else if (inputValue.Equals(new Vector2(-1.0f, 0.0f)))
            {
                TransmitInput(InputCode.Left);
            }
            else if (inputValue.Equals(new Vector2(0.0f, -1.0f)))
            {
                TransmitInput(InputCode.Down);
            }

        }

        private void OnSelect()
        {
            Debug.Log("Select");

            TransmitInput(InputCode.Select);
        }

        private void OnSkill()
        {
            Debug.Log("OnSkill");

            TransmitInput(InputCode.Skill);
        }

        private void OnItem()
        {
            Debug.Log("OnItem");

            TransmitInput(InputCode.Item);
        }

        //private KeyCode GetKeyCode(string key)
        //{
        //    if (key.Count() > 1)
        //    {
        //        if (key == "space")
        //            return KeyCode.Space;
        //        else if (key == "escape")
        //            return KeyCode.Escape;
        //        else if (key == "rightArrow")
        //            return KeyCode.RightArrow;
        //        else if (key == "leftArrow")
        //            return KeyCode.LeftArrow;

        //    }

        //    char character = key[0];
        //    // Get from cache if it was taken before to prevent unnecessary enum parse
        //    KeyCode code;
        //    if (_keycodeCache.TryGetValue(character, out code)) return code;

        //    // Cast to it's integer value
        //    int alphaValue = character;
        //    code = (KeyCode)Enum.Parse(typeof(KeyCode), alphaValue.ToString());
        //    _keycodeCache.Add(character, code);

        //    return code;
        //}

        public void AddInputReceiverIntoStack(IInputReceiver receiver)
        {
            if (! _receiverStack.Contains(receiver))
            {
                _receiverStack.Push(receiver); 
            }
        }

        public void RemoveInputReceiverIntoStack(IInputReceiver receiver)
        {
            if (receiver != null)
            {
                Stack<IInputReceiver> tempStack = new Stack<IInputReceiver>();

                while (_receiverStack.Count > 0)
                {
                    IInputReceiver element = _receiverStack.Pop();
                    if (element != receiver)
                    {
                        tempStack.Push(element);
                    }
                }

                while (tempStack.Count > 0)
                {
                    _receiverStack.Push(tempStack.Pop());
                }
            }
            else if (_receiverStack.Count > 0)
            {
                _receiverStack.Pop();
            }
        }

        public void ClearStack()
        {
            _receiverStack.Clear(); 
        } 
    }


    public interface IInputReceiver
    {
        public void ReceiveKeys(InputCode key);
    }
}
