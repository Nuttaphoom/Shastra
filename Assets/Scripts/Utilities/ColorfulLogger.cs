using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring 
{
    public class ColorfulLogger : MonoBehaviour
    {
        public static void LogWithColor(string message, Color color)
        {
            // Convert the color to a hexadecimal representation
            string hexColor = ColorUtility.ToHtmlStringRGB(color);

            // Format the message with the color tag
            string formattedMessage = $"<color=#{hexColor}>{message}</color>";

            // Output the formatted message to the Unity Console
            Debug.Log(formattedMessage);
        }
    }
}
