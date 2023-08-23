using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Nonthi/Test Card")]
public class TestCardSO : NSCardSO
{
    public override NSRuntimeCard CreateRuntime()
    {
        return new TestRuntimeCard();
    }
}

public class TestRuntimeCard : NSRuntimeCard
{
    public override IEnumerator ExecuteEffect()
    {
        Debug.Log("First Step");

        NSTargetSelector<Tile> tileSelector = new NSTargetSelector<Tile>();

        yield return tileSelector ;

        Debug.Log($"Second Step: {tileSelector.SelectedTarget}");
    }
}
