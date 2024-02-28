using UnityEngine;
using PixelCrushers.DialogueSystem;

public class CloseSubtitleButton : MonoBehaviour
{
    public void CloseSubtitle()
    {
        GetComponentInParent<StandardUISubtitlePanel>().Close();
        Sequencer.Message("ClosedSubtitle");
    }
}
