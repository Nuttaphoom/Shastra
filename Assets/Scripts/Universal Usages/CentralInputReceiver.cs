using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
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

        InspectionOpen = 7,
        Escape = 8,
        C = 9,
        DeSelect = 10,
    }

    public enum ControlScheme
    {
        ps5,
        keyboard,
        xbox
    }

    public class CentralInputReceiver : PersistentInstantiatedObject<CentralInputReceiver>
    {
        private  Dictionary<char, KeyCode> _keycodeCache = new Dictionary<char, KeyCode>();

        private static Stack<IInputReceiver> _receiverStack = new Stack<IInputReceiver>();

        public PlayerInput playerInput_;

        [SerializeField]
        private ControlScheme currentScheme;

        [Serializable]
        struct ControlSchemeUI
        {
            [SerializeField]
            public ControlScheme _scheme;

            [SerializeField]
            public UIInputSetSO _uiInputSetSO;
        }

        [SerializeField]
        private List<ControlSchemeUI> controlSchemeSets;

        #region EventBroadcasterS
        private EventBroadcaster _eventBroadcaster;

        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();
                _eventBroadcaster.OpenChannel<ControlScheme>("OnControllerSchemeChange");
            }

            return _eventBroadcaster;
        }

        public void SubOnControllerSchemeChange(UnityAction<ControlScheme> argc)
        {
            GetEventBroadcaster().SubEvent<ControlScheme>(argc, "OnControllerSchemeChange");
        }

        public void UnSubOnControllerSchemeChange(UnityAction<ControlScheme> argc)
        {
            GetEventBroadcaster().UnSubEvent<ControlScheme>(argc, "OnControllerSchemeChange");
        }

        #endregion

        public Sprite GetUISprite(InputCode code)
        {
            for (int i = 0; i < controlSchemeSets.Count; i++)
            {
                if (controlSchemeSets[i]._scheme == currentScheme)
                {
                    return controlSchemeSets[i]._uiInputSetSO.GetSprite(code);
                }
            }

            Debug.LogError("NULL");
            return null;
        }

        void Awake()
        {
            if (playerInput_ == null)
            {
                playerInput_ = GetComponent<PlayerInput>();
            }
        }

        void Check()
        {
            // Check if Input is Playstation 4
            //Debug.Log("Device count: " + playerInput_.devices.Count);
            if (playerInput_.devices.Count > 0)
            {
                if (playerInput_.devices[0].description.deviceClass == "Keyboard")
                {
                    currentScheme = ControlScheme.keyboard;
                }
                else
                {
                    currentScheme = ControlScheme.ps5;
                }

                GetEventBroadcaster().InvokeEvent(currentScheme, "OnControllerSchemeChange");
            }
           
        }

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
            //Debug.Log("TransmitInput");
            SelectButtonCheck();
            if (_receiverStack.Count > 0) {
                
                _receiverStack.Peek().ReceiveKeys(key);
            } 
        }

        //if button is not select and not in tutorial select it.
        private void SelectButtonCheck()
        {

            if (!PersistentTutorialManager.Instance.IsShowingTutorial)
            {
                //Debug.Log("Selected");
                PersistentButtonSelector.Instance.SelectInitialButtons();
            }
        }


        private void OnNavigate(InputValue value)
        {
            
            Check();

            Vector2 inputValue = value.Get<Vector2>();
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
        private void OnInspectionOpen()
        {
            Check();
            TransmitInput(InputCode.InspectionOpen); 
        }


        private void OnSelect()
        {
            Debug.Log("onselect called"); 
            Check();
            TransmitInput(InputCode.Select);
        }

        private void OnSkill()
        {
            Check();
            TransmitInput(InputCode.Skill);
        }

        private void OnItem()
        {
            Check();
            TransmitInput(InputCode.Item);
        }

        private void OnDeSelect()
        {
            Check();
            TransmitInput(InputCode.DeSelect);
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
            Debug.Log("Add Input from: " + receiver);
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
            //PersistentButtonSelector.Instance.SelectInitialButtons();
            //Debug.Log("RemoveInputReceiverIntoStack");
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
