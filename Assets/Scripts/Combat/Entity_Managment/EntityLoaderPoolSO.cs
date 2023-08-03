

using System.Collections.Generic;
using UnityEngine;
using Vanaring_DepaDemo;

[CreateAssetMenu(fileName = "EntityPool" , menuName = "ScriptableObject/EntityManagement/EntityLoaderPoolSO")]
public class EntityLoaderPoolSO : ScriptableObject
{
    [SerializeField]
    private List<CombatEntity> _entitiesPrefabPool ;

    [SerializeField]
    private ECompetatorSide _competatorSide ;

    public List<CombatEntity> Entities => _entitiesPrefabPool ; 


}