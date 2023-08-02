

using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring_DepaDemo {

    [CreateAssetMenu(fileName = "CombatEntityEventChannel" , menuName = "ScriptableObject/Utilities/EventChannel/CombatEntityEventChannel")]
    public class CombatEntityEventChannel : EventChannel<CombatEntity>
    {

    }
}