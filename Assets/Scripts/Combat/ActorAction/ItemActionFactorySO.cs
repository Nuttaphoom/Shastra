using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring 
{ 
    [CreateAssetMenu(fileName = "Item Ability", menuName = "ScriptableObject/Combat/ItemAbility")]
    public class ItemActionFactorySO : ActorActionFactory
    {
        public Sprite ItemSprite  => this.AbilityImage;

        public ItemAbilityRuntime FactorizeRuntimeItem(ItemUserHandler itemuserHandler)
        {
            return new ItemAbilityRuntime(EffectFactory,this, itemuserHandler, _targetSelector) ;
        }
    }

    public class ItemAbilityRuntime  : IActorAction 
    {
        private string _itemName;
        private string _description;

        private ItemActionFactorySO _factory; 
        private RuntimeEffectFactorySO _effectFactory ;
        private ItemUserHandler _itemUserHandler;
        private List<CombatEntity> _targets;
        private TargetSelector _targetSelector; 
        public ItemAbilityRuntime(RuntimeEffectFactorySO effect, ItemActionFactorySO factory, ItemUserHandler handler, TargetSelector targetselector)
        {
            _itemUserHandler = handler; 
            _effectFactory = effect;
             _factory = factory;
            _targetSelector = targetselector;   
        }

        public string ItemName => _factory.AbilityName;
        public string ItemDescrption => _factory.Desscription;

        public Sprite ItemSprite => _factory.ItemSprite;

        public RuntimeEffectFactorySO GetEffectFactory()
        {
            return _effectFactory ;  
        }

        #region INTERFACE

        public RuntimeEffect GetRuntimeEffect()
        {
            return _effectFactory.Factorize(_targets);
        }

        public TargetSelector GetTargetSelector()
        {
            return _targetSelector  ; 
        }

        public IEnumerator PreActionPerform( )
        {
            _itemUserHandler.RemoveItem(this) ;
            yield return null ; 
        }

        public void SetActionTarget(List<CombatEntity> targets)
        {
            _targets = new List<CombatEntity>(); 
            foreach (CombatEntity entity in targets)
            {
                _targets.Add(entity) ;
            }
        }
        #endregion
    }

}
