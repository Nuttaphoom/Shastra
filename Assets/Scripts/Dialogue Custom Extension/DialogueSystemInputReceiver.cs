using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring
{
    public class DialogueSystemInputReceiver : MonoBehaviour, IInputReceiver
    {
        private void Awake()
        {
            DialogueSystemController dialogueSystemController = GetComponent<DialogueSystemController>();    
            if (dialogueSystemController == null)
                throw new System.Exception("DialogueSystemController can not be found within " + gameObject.name);

            dialogueSystemController.conversationEnded += OnEndConversation;
            dialogueSystemController.conversationStarted  += OnStartConversation;

        }
        public void OnStartConversation(Transform actor )
        {
            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);
        }

        public void OnEndConversation(Transform actor)
        {
            CentralInputReceiver.Instance().RemoveInputReceiverIntoStack(this); 
        }

        public void ReceiveKeys(KeyCode key)
        {
            bool isType = TypewriterUtility.GetTypewriter(DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels[0].subtitleText).isPlaying; 

            if (isType)
            {
                TypewriterUtility.StopTyping(DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels[0].subtitleText);

            }else
            {
                Sequencer.Message("ClosedSubtitle");

            }
        }

         
    }
}
