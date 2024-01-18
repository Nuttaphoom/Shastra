using UnityEngine;
using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;

public class CloseSubtitleButton : MonoBehaviour
{
    public void CloseSubtitle()
    {
        GetComponentInParent<StandardUISubtitlePanel>().Close();
        Sequencer.Message("ClosedSubtitle");
    }

    public void HideSubtitleText()
    {
        Sequencer.Message("ClosedSubtitle");
    }


}
