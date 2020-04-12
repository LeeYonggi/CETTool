using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CET
{
    public class StateManager : MonoBehaviour
    {
        public float health;

        public BehaviourGraph currentGraph;

        private State currentState;

        [HideInInspector]
        public float delta;
        [HideInInspector]
        public Transform mTransform;

        public State CurrentState { get => currentState; set => currentState = value; }

        private void Start()
        {
            mTransform = this.transform;

            currentState = currentGraph.savedStateNodes[0].state;

            currentState.OnEnter(this);
        }

        private void Update()
        {
            if(currentState != null)
            {
                currentState.Tick(this);
            }
        }

        public void DebugLog(string obj)
        {
            Debug.Log(obj);
        }
    }
}
