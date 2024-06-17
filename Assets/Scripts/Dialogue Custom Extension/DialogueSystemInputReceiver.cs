using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring
{
    public class DialogueSystemInputReceiver : MonoBehaviour, IInputReceiver
    {

        private const float maxInputDiffTime = 0.05f;
        //TODO : Remove temp 
        private float preventInputSpam = .05f;
        private void Awake()
        {
            DialogueSystemController dialogueSystemController = GetComponent<DialogueSystemController>();    
            if (dialogueSystemController == null)
                throw new System.Exception("DialogueSystemController can not be found within " + gameObject.name);

            dialogueSystemController.conversationEnded += OnEndConversation;
            dialogueSystemController.conversationStarted  += OnStartConversation;
            preventInputSpam = maxInputDiffTime ;

        }
        private void Update()
        {
            if (preventInputSpam > 0.0f)
            {
                preventInputSpam -= 1.0f * Time.deltaTime;
            }
        }

        private void OnDisable()
        {
            DialogueSystemController dialogueSystemController = GetComponent<DialogueSystemController>();

            if (dialogueSystemController == null)
                throw new System.Exception("DialogueSystemController can not be found within " + gameObject.name);

            dialogueSystemController.conversationEnded += OnEndConversation;
            dialogueSystemController.conversationStarted  += OnStartConversation;

        }
        public void OnStartConversation(Transform actor )
        {
            CentralInputReceiver.Instance.AddInputReceiverIntoStack(this);
        }

        public void OnEndConversation(Transform actor)
        {
            CentralInputReceiver.Instance.RemoveInputReceiverIntoStack(this); 
        }

        public void ReceiveKeys(InputCode key)
        {
            if (preventInputSpam > 0)
                return;

            preventInputSpam = maxInputDiffTime;

            if (key == InputCode.Select)
            {
                var subtitlePanels  = DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels;
                bool isType = false;
                int subtitleIndex = -1 ;

                for (int i = 0; i < subtitlePanels.Length; i++)
                {
                    if (subtitlePanels[i].gameObject.activeSelf)
                    {
                        subtitleIndex = i;
                       isType = TypewriterUtility.GetTypewriter(DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels[i].subtitleText).isPlaying;

                    }
                }

                if (subtitleIndex == -1)
                {
                    Debug.LogWarning("Subtitle index is -1, no subtitlePanels is active");
                    return;
                }

                if (isType)
                {
                    TypewriterUtility.StopTyping(DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels[subtitleIndex].subtitleText);

                }
                else
                {

                    Debug.Log("closed subtitle");
                    Sequencer.Message("ClosedSubtitle");

                    

                }
            }
        }

         
    }
}
