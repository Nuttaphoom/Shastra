using UnityEngine;
using PixelCrushers.DialogueSystem;

public class CloseSubtitleButton : MonoBehaviour
{
    public void CloseSubtitle()
    {
        GetComponentInParent<StandardUISubtitlePanel>().HideContinueButton();
        Sequencer.Message("ClosedSubtitle");
    }
}
