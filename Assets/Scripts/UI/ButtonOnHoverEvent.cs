using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Vanaring
{
    public class ButtonOnHoverEvent : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public void OnSelect(BaseEventData eventData)
        {
            transform.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            transform.transform.localScale = Vector3.one;
        }
    }
}
