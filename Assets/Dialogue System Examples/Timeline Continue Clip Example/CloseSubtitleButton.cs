using UnityEngine;
using PixelCrushers.DialogueSystem;

public class CloseSubtitleButton : MonoBehaviour
{
    public void CloseSubtitle()
    {

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
