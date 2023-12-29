using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "LocationSO", menuName = "ScriptableObject/DaySystem/LocationSO")]
    public class LocationSO : ScriptableObject
    {
        [SerializeField]
        private LocationName _locationName;

        [SerializeField]
        private SceneDataSO _sceneData ;

        public SceneDataSO SceneData => _sceneData;
        public LocationName LocationName { get { return _locationName; } }

        public RuntimeLocation FactorizeRuntimeLocation(List<BaseLocationSelectionCommand> actionWithinLocation)
        {
            return new RuntimeLocation(this, actionWithinLocation); 
        }


    }
    public struct LoadLocationMenuCommandData
    {
        public List<BaseLocationSelectionCommand> action_on_this_location;
    }

    public class RuntimeLocation
    {
        private LocationName _locationName;
        private SceneDataSO _sceneData;
        private List<BaseLocationSelectionCommand> actionOnLocation;
        public RuntimeLocation(LocationSO data, List<BaseLocationSelectionCommand> actionOnLocation)
        {
            this._locationName = data.LocationName;
            this._sceneData = data.SceneData;
            this.actionOnLocation = actionOnLocation;
        }
        public void LoadThisLocation()
        {
            if (_locationName == LocationName.Null)
                throw new Exception("location name is null");

            LoadLocationMenuCommandData data = new LoadLocationMenuCommandData();
            data.action_on_this_location = new List<BaseLocationSelectionCommand>();

            foreach (var actionCommand in actionOnLocation)
                data.action_on_this_location.Add(actionCommand);

            PersistentSceneLoader.Instance.LoadLocation<LoadLocationMenuCommandData>(_sceneData, data);
        }
    }

    public enum LocationName
    {
        Null,
        Front_Gate, 
        Library
    }
}
