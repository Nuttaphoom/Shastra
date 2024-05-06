using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class RandomUIPositionInCircle : MonoBehaviour
    {
        public RectTransform[] uiElements; // Reference to the UI elements you want to position
        public Vector2 centerPosition; // Center position of the circular area
        public float radius = 100f; // Radius of the circular area

        void Start()
        {
            PlaceUIElements();
        }

        void PlaceUIElements()
        {
            foreach (RectTransform uiElement in uiElements)
            {
                // Generate a random point within the unit circle
                Vector2 randomPoint = Random.insideUnitCircle;

                // Scale and translate the random point to fit within the circular area
                Vector2 newPosition = centerPosition + randomPoint * radius;

                // Set the position of the UI element
                uiElement.anchoredPosition = newPosition;
            }
        }
    }
}
