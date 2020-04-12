using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CET
{
    [CreateAssetMenu]
    public class State : ScriptableObject
    {
        public StateActions[] onState;
        public StateActions[] onEnter;
        public StateActions[] onExit;

        public List<Transition> transitions = new List<Transition>();


        public void OnEnter(StateManager states)
        {
            ExecuteActions(states, onEnter);
        }

        public void OnExit(StateManager states)
        {
            ExecuteActions(states, onExit);
        }

        public void Tick(StateManager states)
        {
            ExecuteActions(states, onState);
            CheckTransitions(states);
        }

        public void CheckTransitions(StateManager states)
        {
            for(int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].disable)
                    continue;

                if (transitions[i].condition.CheckCondition(states))
                {
                    if (transitions[i].targetState != null)
                    {
                        states.CurrentState = transitions[i].targetState;
                        OnExit(states);
                        states.CurrentState.OnEnter(states);
                    }
                    return;
                }

            }
        }

        public void ExecuteActions(StateManager states, StateActions[] actions)
        {
            for(int i = 0; i < actions.Length; i++)
            {
                if (actions[i] != null)
                    actions[i].Execute(states);
            }
        }

        public Transition AddTransition()
        {
            Transition retVal = new Transition();

            transitions.Add(retVal);
            return retVal;
        }
    }
}