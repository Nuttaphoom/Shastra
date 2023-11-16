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

        public override ActorAction FactorizeRuntimeAction(CombatEntity user)
        {
            
            return new ItemAbilityRuntime(user,this, user.ItemUser  ) ;
        }
    }

    public class ItemAbilityRuntime  : ActorAction 
    {
        private ItemActionFactorySO _factory; 
        private ItemUserHandler _itemUserHandler;

        public string ItemName => _factory.AbilityName;
        public string ItemDescrption => _factory.Desscription;

        public Sprite ItemSprite => _factory.ItemSprite;
        public ItemAbilityRuntime(CombatEntity caster, ItemActionFactorySO factory, ItemUserHandler handler   ) : base(factory, caster)
        {
            _itemUserHandler = handler; 
             _factory = factory;
            
        }



     

        #region INTERFACE
 

        public override IEnumerator PostActionPerform()
        {
            yield return null; 
        }

        public override IEnumerator PreActionPerform( )
        {
            _itemUserHandler.RemoveItem(this) ;
            yield return null ; 
        }

        public override IEnumerator Simulate(CombatEntity target)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

}
