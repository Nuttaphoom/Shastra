using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vanaring.Assets.Scripts.Utilities;

namespace Vanaring
{
    public class PersistentActiveDayDatabase : PersistentInstantiatedObject<PersistentActiveDayDatabase>
    {
        [SerializeReference]
        private RuntimeDayData runtimeDayData = null;



    }
}
