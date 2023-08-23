using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSCardFlowController : MonoBehaviour
{
    [SerializeField] private NSCardSO _testCard;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RunTestCoroutine());
    }

    IEnumerator RunTestCoroutine()
    {
        NSRuntimeCard runtimeCard = _testCard.CreateRuntime();
        IEnumerator sequences = runtimeCard.ExecuteEffect();

        while (sequences.MoveNext())
        {
            if (sequences.Current != null && sequences.Current.GetType().IsSubclassOf(typeof(NSTargetSelector)))
            {
                yield return StartCoroutine(FillTargetCoroutine(sequences.Current as NSTargetSelector));
            }
        }
    }

    IEnumerator FillTargetCoroutine(NSTargetSelector selector)
    {
        while (!Input.GetKeyDown(KeyCode.E))
        {
            yield return null;
        }
        selector.SetSelectedTarget(null);
        yield break;
    }
}
