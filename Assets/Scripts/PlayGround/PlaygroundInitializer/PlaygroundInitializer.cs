using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundInitializer : MonoBehaviour
{
    public ThemeSO theme;
    //public GameObject[] particle;

    void Start()
    {
        Debug.Log("start");
        VisualInit(theme);
        //StartCoroutine(PlayVFX());
    }

    void Update()
    {

    }

    private void VisualInit(ThemeSO theme)
    {
        Debug.Log(theme.name); 
        for (int i = 0; i < theme.model.Length; i++)
        {
            Instantiate(theme.model[i], theme.position[i], Quaternion.identity);
        }
        Instantiate(theme.tabs, new Vector3(-0.10573981f, -2.4601419f, 2.65045857f), Quaternion.identity);
        Instantiate(theme.castleModel_E, new Vector3(0, 4, 23), Quaternion.identity);
        Instantiate(theme.castleModel_P, new Vector3(0, 4, -23), Quaternion.identity);
    }

    //IEnumerator PlayVFX()
    //{
    //    //Instantiate(particle[1], new Vector3(0, 0, 0), Quaternion.identity);
    //    yield return new WaitForSeconds(1.0f);
    //}
}
