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
            return new ItemAbilityRuntime(this, itemuserHandler, _targetSelector, _actionSignal) ;
        }
    }

    public class ItemAbilityRuntime  : IActorAction 
    {
        private ItemActionFactorySO _factory; 
        private RuntimeEffectFactorySO _effectFactory ;
        private ItemUserHandler _itemUserHandler;
        private List<CombatEntity> _targets;
        private TargetSelector _targetSelector;
        private ActionSignal _actionSignal; 
        public ItemAbilityRuntime(ItemActionFactorySO factory, ItemUserHandler handler, TargetSelector targetselector, ActionSignal actionSignal)
        {
            _itemUserHandler = handler; 
             _factory = factory;
            _targetSelector = targetselector;
            _actionSignal = new ActionSignal(actionSignal) ; 
            
        }

        public string ItemName => _factory.AbilityName;
        public string ItemDescrption => _factory.Desscription;

        public Sprite ItemSprite => _factory.ItemSprite;

     

        #region INTERFACE

        public RuntimeEffect GetRuntimeEffect()
        {
            return _effectFactory.Factorize(_targets);
        }

        public TargetSelector GetTargetSelector()
        {
            return _targetSelector  ; 
        }

        public IEnumerator PerformAction()
        {
            throw new NotImplementedException();
        }

        public IEnumerator PostActionPerform()
        {
            yield return null; 
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


        public IEnumerator Simulate(CombatEntity target)
        {
            throw new NotImplementedException();
        }

        TargetSelector IActorAction.GetTargetSelector()
        {
            throw new NotImplementedException();
        }

        IEnumerator IActorAction.PostActionPerform()
        {
            throw new NotImplementedException();
        }

        IEnumerator IActorAction.PreActionPerform()
        {
            throw new NotImplementedException();
        }

        void IActorAction.SetActionTarget(List<CombatEntity> targets)
        {
            throw new NotImplementedException();
        }

     

        IEnumerator IActorAction.Simulate(CombatEntity target)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

}
