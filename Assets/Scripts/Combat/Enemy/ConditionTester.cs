using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vanaring
{
    public class ConditionTester : MonoBehaviour
    {
        [SerializeField]
        private CombatEntity tester;

        [Serializable]
        public struct ConditionData
        {
            [SerializeField]
            public BaseConditionSO Condition;
            [SerializeField]
            public float conditionAmount;
        }

        public List<ConditionData> conditionlist;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log("ModifyHPStat -100");
                tester.StatsAccumulator.ModifyHPStat(-100.0f);
            }
        }
    }

    ////[Serializable]
    //public class ConditionTest
    //{
    //    public ConditionTest(BaseConditionSO conditionSO)
    //    {
    //        this.handler = conditionSO.ConditionsMet();
    //    }

    //    public string title;
    //    public string description;
    //    public Predicate<object> trigger;

    //    private delegate bool Callback(BaseConditionSO conditionSO);

    //    private Callback handler;

    //    // Call the delegate.
    //    handler("Hello World");

    //    [NonSerialized]
    //    public bool istriggered = false;

    //    public void UpdateTutorialTrigger()
    //    {
    //        if (istriggered)
    //            return;

    //        if (TriggerActive())
    //        {
    //            Debug.Log("TriggerActive : " + title);
    //            istriggered = true;
    //        }
    //    }

    //    public bool TriggerActive()
    //    {
    //        return trigger.Invoke(null);
    //    }
    //}

    //public class ConditionTester : MonoBehaviour
    //{
    //    public List<TestCondition<int>> tester = new List<TestCondition<int>>();
    //    public int integer;

    //    //Predicate<int> functiontest = (int o) => o >= 100;
    //    private void Start()
    //    {
    //        InitializeTutorial();
    //    }
    //    private void InitializeTutorial()
    //    {
    //        if (tester != null)
    //            return;

    //        //tester = new List<TestCondition<int>>();
    //        tester.Add(new TestCondition<int>((int o) => o >= 100));
    //        tester.Add(new TestCondition<int>((int o) => o >= 100));
    //        tester.Add(new TestCondition<int>((int o) => o >= 100));
    //    }

    //    private void Update()
    //    {
    //        CheckIsTrigger();
    //    }

    //    private void CheckIsTrigger()
    //    {
    //        if (tester == null)
    //            return;

    //        foreach (TestCondition<int> t in tester)
    //        {
    //            t.UpdateTrigger(integer);
    //        }
    //    }
    //}

    //[Serializable]
    //public class TestCondition<T>
    //{
    //    public TestCondition(Predicate<T> trigger)
    //    {
    //        this.trigger = trigger;
    //    }

    //    public string name = ":D";

    //    public Predicate<T> trigger;
    //    [NonSerialized]
    //    public bool istriggered = false;

    //    public void UpdateTrigger(T input)
    //    {
    //        if (istriggered)
    //            return;

    //        if (TriggerActive(input))
    //        {
    //            Debug.Log("TriggerActive : " + name);
    //            istriggered = true;
    //        }
    //    }

    //    public bool TriggerActive(T input)
    //    {
    //        return trigger.Invoke(input);
    //    }
    //}
}
