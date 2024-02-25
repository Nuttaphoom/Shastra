using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "LocationSO", menuName = "ScriptableObject/DaySystem/LocationSO")]
    public class LocationSO : ScriptableObject
    {
        [SerializeField]
        private LocationName _locationName;

        [SerializeField]
        private SceneDataSO _sceneData ;

        [SerializeField]
        private Sprite _locationIcon;
        public Sprite LocationIcon => _locationIcon;
        public SceneDataSO SceneData => _sceneData;
        public LocationName LocationName { get { return _locationName; } }

        public RuntimeLocation FactorizeRuntimeLocation(List<BaseLocationActionCommand> actionWithinLocation)
        {
            return new RuntimeLocation(this, actionWithinLocation); 
        }


    }

    public struct LoadLocationMenuCommandData
    {
        public LocationName LocationName; 
    }

    public class RuntimeLocation
    {
        private LocationName _locationName;
        private SceneDataSO _sceneData;
        private List<BaseLocationActionCommand> actionOnLocation;
        private Sprite _locationIcon;
        public RuntimeLocation(LocationSO data, List<BaseLocationActionCommand> actionOnLocation)
        {
            this._locationName = data.LocationName;
            this._sceneData = data.SceneData;
            this.actionOnLocation = actionOnLocation;
            this._locationIcon = data.LocationIcon;
        }

        public LocationName LocationName { get { return _locationName; } }
        public Sprite LocationIcon { get { return _locationIcon; } }
        public List<BaseLocationActionCommand> ActionOnLocation  { get { return actionOnLocation; } }

        public void LoadThisLocation()
        {
            if (_locationName == LocationName.Null)
                throw new Exception("location name is null");

            LoadLocationMenuCommandData data = new LoadLocationMenuCommandData();
            data.LocationName = LocationName; 

            //foreach (var actionCommand in actionOnLocation)
            //    data.action_on_this_location.Add(actionCommand);

            PersistentSceneLoader.Instance.LoadLocation<LoadLocationMenuCommandData>(_sceneData, data);
        }

        public bool IsSameLocation (LocationName name)
        {
            return (name == LocationName) ;
        }
    }

    public enum LocationName
    {
        Null,
        Front_Gate, 
        Library,
        Stadium,
        Cottage,
        SchoolMap,
        Dorm
    }
}
