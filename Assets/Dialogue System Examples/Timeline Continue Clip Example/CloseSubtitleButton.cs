using UnityEngine;
using PixelCrushers.DialogueSystem;

public class CloseSubtitleButton : MonoBehaviour
{
    public void CloseSubtitle()
    {
        Debug.Log("speed : " + TypewriterUtility.GetTypewriterSpeed(DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels[0].subtitleText)); 

        if (false)
        {
            TypewriterUtility.StopTyping(DialogueManager.standardDialogueUI.conversationUIElements.subtitlePanels[0].subtitleText);

        }
        else
        {
            Sequencer.Message("ClosedSubtitle");

        }
    }
}
