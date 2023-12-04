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

        public void SubOnOnAilmentAppliedEventChannel(UnityAction<EntityAilmentApplierEffect> func)
        {
            GetEventBroadcaster().SubEvent<EntityAilmentApplierEffect>(func, "OnAilmentApplied");

        }
        public void UnSubOnOnAilmentAppliedEventChannel(UnityAction<EntityAilmentApplierEffect> func)
        {
            GetEventBroadcaster().UnSubEvent<EntityAilmentApplierEffect>(func, "OnAilmentApplied");

        }



        #endregion

        public AilmentHandler(CombatEntity user)
        {
            _user = user;
            _ailmentResistantHandler = new AilmentResistantHandler(user.CharacterSheet.ResistantData); 
        }

        public IEnumerator LogicApplyAilment(Ailment newAilment)
        {
            EntityAilmentApplierEffect applierEventData = new EntityAilmentApplierEffect() {
                AilmentEffectPair = new EntityAilmentEffectPair() {   Actor = _user, Ailment = newAilment }, 
                ResistantBlocked = false , 
                SucessfullyAttach = false  };
 

            if (_ailmentResistantHandler.ResistantToAilment(newAilment.GetAilmentType()))
            {
                applierEventData.ResistantBlocked = true ;
                goto End;
            }

            if (_currentAilment != null)
            {
                if (!newAilment.ShouldOverwrittenOthers() || _currentAilment.ResistOverwritten())
                {
                    goto End; 
                }

                yield return _currentAilment.OnAilmentOverwritten();
            }

            _currentAilment = newAilment ;
            applierEventData.SucessfullyAttach = true;
            _currentAilment.OnApplyAilment();

        End:

            GetEventBroadcaster().InvokeEvent(applierEventData, "OnAilmentApplied");

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
                    yield return _currentAilment.AilmentRecover();

                    _eventBroadcaster.OpenChannel<EntityAilmentEffectPair>("OnAilmentRecover");
                    _eventBroadcaster.InvokeEvent(new EntityAilmentEffectPair() { Actor = _user, Ailment = _currentAilment} , "OnAilmentRecover");
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
