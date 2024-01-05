using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class Arm_SceneLoaderTest : MonoBehaviour
    {
     

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log((FindObjectOfType<LoaderDataContainer>().UseDataInContainer()   ) );

                Debug.Log((FindObjectOfType<LoaderDataContainer>().UseDataInContainer() as LoaderDataUser<LoadLocationMenuCommandData>));

                Debug.Log((FindObjectOfType<LoaderDataContainer>().UseDataInContainer() as LoaderDataUser<LoadLocationMenuCommandData>).GetData());
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                (FindObjectOfType<LoaderDataContainer>().UseDataInContainer() as LoaderDataUser<LoadLocationMenuCommandData>).GetData().action_on_this_location[0].ExecuteCommand(); 

                //Debug.Log( (FindObjectOfType<LoaderDataContainer>().UseDataInContainer() as LoaderDataUser<LoadLocationMenuCommandData> ).GetData().action_on_this_location.Count);
            }
        }

    }
}
