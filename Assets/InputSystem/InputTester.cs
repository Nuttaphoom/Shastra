using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Vanaring
{
    public class InputTester : MonoBehaviour, IInputReceiver
    {
        //public GameObject _inputSystem;
        //public Button _button;
        //public Button _dummyButton;

        //[ContextMenu("Active")]
        //private void Active()
        //{
        //    if (_inputSystem != null) {
        //        _inputSystem.SetActive(true);
        //    }

        //}

        //[ContextMenu("Deactive")]
        //private void Deactive()
        //{
        //    if ( _inputSystem != null )
        //    {
        //        _inputSystem.SetActive(false);
        //    }

        //}

        //[ContextMenu("ButtonSelect")]
        //private void ButtonSelect()
        //{
        //    if (_button != null) {
        //        _button.Select();
        //    }
            
        //}

        //private void Update() 
        //{ 
        //    if (Input.GetKeyDown(KeyCode.O))
        //    {
        //        Active();
        //    }
        //    if (Input.GetKeyDown(KeyCode.P))
        //    {
        //        Deactive();
        //    }
        //    if (Input.GetKeyDown(KeyCode.I))
        //    {
        //        ButtonSelect();
        //    }
        //    if (Input.GetKeyDown(KeyCode.K))
        //    {
        //        if (_dummyButton != null)
        //        {
        //            _dummyButton.Select();
        //        }
        //    }
        //}

        public void ReceiveKeys(InputCode key)
        {
            
        }
    }
}
