using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CET
{ 
    [System.Serializable]
    public class Transition
    {
        public Condition condition;
        public State targetState;
        public GameObject targetObject;
        public bool disable;

        public List<Action> targetActions = new List<Action>();

        public Action AddAction()
        {
            Action action = new Action();

            targetActions.Add(action);
            return action;
        }
    }
}
