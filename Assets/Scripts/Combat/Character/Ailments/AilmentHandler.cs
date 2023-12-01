using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Vanaring
{
    public class AilmentHandler   
    {
        private AilmentResistantHandler _ailmentResistantHandler; 
        private CombatEntity _user;
        private Ailment _currentAilment;

        #region EventBroadcaster 
        private EventBroadcaster _eventBroadcaster;

        public EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();
                _eventBroadcaster.OpenChannel<EntityAilmentEffectPair>("OnAilmentControl");
                _eventBroadcaster.OpenChannel<EntityAilmentEffectPair>("OnAilmentRecover");
                _eventBroadcaster.OpenChannel<EntityAilmentApplierEffect>("OnAilmentApplied");
            }

            return _eventBroadcaster;
        }

        public void SubOnAilmentControlEventChannel(UnityAction<EntityAilmentEffectPair> func)
        {
            GetEventBroadcaster().SubEvent<EntityAilmentEffectPair>(func, "OnAilmentControl"); 
        }

        public void UnSubOnAilmentControlEventChannel(UnityAction<EntityAilmentEffectPair> func)
        {
            GetEventBroadcaster().UnSubEvent<EntityAilmentEffectPair>(func, "OnAilmentControl");
        }
        public void SubOnAilmentRecoverEventChannel(UnityAction<EntityAilmentEffectPair> func)
        {
            GetEventBroadcaster().SubEvent<EntityAilmentEffectPair>(func, "OnAilmentRecover");
        }

        public void UnSubOnAilmentRecoverEventChannel(UnityAction<EntityAilmentEffectPair> func)
        {
            GetEventBroadcaster().UnSubEvent<EntityAilmentEffectPair>(func, "OnAilmentRecover");
        }
 

        #endregion 

        public AilmentHandler(CombatEntity user)
        {
            _user = user;
            _ailmentResistantHandler = new AilmentResistantHandler(user.CharacterSheet.ResistantData); 
        }

        public IEnumerator LogicApplyAilment(Ailment newAilment)
        {
            if (_currentAilment != null)
            {
                if (! newAilment.ShouldOverwritedOthers())
                    goto End;

                if (_currentAilment.ResistOverwrited())
                    goto End;

                yield return _currentAilment.AilmentRecover();
            }

            _currentAilment = newAilment ;
            _currentAilment.OnApplyAilment();

            End:
            yield return null;

        }

        /// <summary>
        /// Call ProgressAlimentTTL when turn enter  
        /// Removed ailment return true ;
        /// </summary>
        public void ProgressAlimentTTL()
        {
            if (_currentAilment == null)
                return;

            _currentAilment.UpdateTTL();

        }

        public IEnumerator CheckForExpiration()
        {
            if (_currentAilment != null)
            {
                if (_currentAilment.AlimentExpired())
                {
                    Debug.Log("check for expiration");
                    yield return _currentAilment.AilmentRecover();
                    Debug.Log("current ailment type is " + _currentAilment.GetType()) ;
                    _currentAilment = null;
                }
            }

            yield return null; 
        }
        /// <summary>
        /// If Ailment is expired, remove its effect and return false to give control back to the Entity 
        /// </summary>
        /// <returns></returns>
        public bool DoesAilmentOccur()
        {
            if (_currentAilment == null)
            {
                return false;
            }

           

            return (_currentAilment != null);
        }

        public IEnumerator AlimentControlGetAction()
        {
            if (_currentAilment == null)
                throw new Exception("_currentAilment is null when it shouldn't be");

            yield return _currentAilment.SetEntityAction();

            _eventBroadcaster.InvokeEvent<EntityAilmentEffectPair>(new EntityAilmentEffectPair() { Ailment = _currentAilment, Actor = _user }, "OnAilmentControl");

        }


    }

    
}
