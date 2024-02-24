using UnityEngine;
using PixelCrushers.DialogueSystem;

public class CloseSubtitleButton : MonoBehaviour
{
    public void CloseSubtitle()
    {
        Debug.Log("close subtitle");

        GetComponentInParent<StandardUISubtitlePanel>().Close();
        Sequencer.Message("ClosedSubtitle");
    }
}
