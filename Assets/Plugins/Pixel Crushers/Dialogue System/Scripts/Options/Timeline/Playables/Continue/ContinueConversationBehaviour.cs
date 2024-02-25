// Recompile at 10/1/2567 19:46:46
#if USE_TIMELINE
#if UNITY_2017_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEngine.Playables;
using System;

namespace PixelCrushers.DialogueSystem
{

    [Serializable]
    public class ContinueConversationBehaviour : PlayableBehaviour
    {
        public enum Operation { Continue, ClearSubtitleText }

        [Tooltip("Continue past current subtitle or just clear text in subtitle panels.")]
        public Operation operation = Operation.Continue;

        [Tooltip("If Operation is Clear Subtitle Text, clear these panel(s).")]
        public int clearPanelNumber = 0;

        [Tooltip("Arm : disable dialogue box after clip completion")]
        public bool HideDialogueBoxAfterCompletion; 

        public bool clearAllPanels = false;
    }
}
#endif
#endif
