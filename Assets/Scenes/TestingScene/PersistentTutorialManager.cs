using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Vanaring
{
    public class PersistentTutorialManager : PersistentInstantiatedObject<PersistentTutorialManager>
    {
        [SerializeField]
        private GameObject tutorialDemo;

        private List<GameObject> tutorials = new List<GameObject>();

        public void PlayTutorial(string tutorialName)
        {
            Debug.Log("PlayTutorial : " + tutorialName);
            GameObject tutorial = Instantiate(tutorialDemo, new Vector3(0, 0, 0), Quaternion.identity);
            tutorial.name = tutorialName;
            tutorial.transform.parent = gameObject.transform;
            CutsceneDirector cd = tutorial.AddComponent<CutsceneDirector>();
            cd.PlayCutscene();
            tutorials.Add(tutorial);
        }

        public void DonePlayTutorial(string tutorialName)
        {
            foreach (GameObject obj in tutorials)
            {
                if (obj.name == tutorialName)
                {
                    Debug.Log("Destroy Tutorial : " + tutorialName);
                    Destroy(obj);
                    return;
                }
            }
            Debug.LogError("invalid name :" + tutorialName);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                PlayTutorial("Test");
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                DonePlayTutorial("Test");
            }
        }
    }
}
