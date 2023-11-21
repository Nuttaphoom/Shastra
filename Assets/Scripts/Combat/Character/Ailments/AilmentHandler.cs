using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring
{
    public class AilmentHandler   
    {
        private CombatEntity _user;
        private Ailment _currentAilment;
        public AilmentHandler(CombatEntity user)
        {
            _user = user;
        }

        public void LogicApplyAilment(Ailment newAilment)
        {
            if (_currentAilment != null)
                return;

            _currentAilment = newAilment; 

        }

        /// <summary>
        /// Call ProgressAlimentTTL when turn enter 
        /// </summary>
        public void ProgressAlimentTTL()
        {
            if (_currentAilment == null)
                return;

            _currentAilment.UpdateTTL();
        }

        /// <summary>
        /// If Ailment is expired, remove its effect and return false to give control back to the Entity 
        /// </summary>
        /// <returns></returns>
        public bool DoesAilmentOccur()
        {
            if (_currentAilment.AlimentExpired())
            {
                _currentAilment = null;
            }

            return (_currentAilment != null);
        }

        public IEnumerator AlimentControl()
        {
            if (_currentAilment == null)
                throw new Exception("_currentAilment is null when it shouldn't be");

            yield return _currentAilment.TakeControlEntity();
        }


    }

    
}
