using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class TutorialManager : MonoBehaviour
    {
        public List<Tutorial> tutorials;

        public int integer;

        //public bool TutorialActivate(string tutorialname)
        //{
        //    bool result = false;
        //    if (tutorials == null)
        //        return false;

        //    Tutorial[] tutorialArray = tutorials.ToArray();
        //    Tutorial t = Array.Find(tutorialArray, temp => tutorialname == temp.title);

        //    if (t == null)
        //        return false;

        //    result = t.istriggered;

        //    return result;
        //}

        private void Start()
        {
            InitializeTutorial();
        }
        private void InitializeTutorial()
        {
            if (tutorials != null)
                return;

            tutorials = new List<Tutorial>();
            tutorials.Add(new Tutorial("INT", "OMG", (object o) => integer >= 100));
        }

        private void Update()
        {
            CheckIsTutorialTrigger();
        }

        private void CheckIsTutorialTrigger()
        {
            if (tutorials == null)
                return;

            foreach (Tutorial t in tutorials)
            {
                t.UpdateTutorialTrigger();
            }
        }
    }

    [Serializable]
    public class Tutorial
    {
        public Tutorial(string title, string description, Predicate<object> trigger)
        {
            this.title = title;
            this.description = description;
            this.trigger = trigger;

        }

        public string title;
        public string description;
        public Predicate<object> trigger;
        [NonSerialized]
        public bool istriggered = false;

        public void UpdateTutorialTrigger()
        {
            if (istriggered)
                return;

            if (TriggerActive())
            {
                Debug.Log("TriggerActive : " + title);
                istriggered = true;
            }
        }

        public bool TriggerActive()
        {
            return trigger.Invoke(null);
        }
    }
}
