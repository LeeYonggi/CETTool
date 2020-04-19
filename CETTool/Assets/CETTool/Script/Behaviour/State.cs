﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CET
{
    [CreateAssetMenu]
    public class State : ScriptableObject
    {
        public StateActions onState;

        public List<Transition> transitions = new List<Transition>();


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
                    }
                    return;
                }

            }
        }

        public void ExecuteActions(StateManager states, StateActions actions)
        {
            actions.Execute(states);
        }

        public Transition AddTransition()
        {
            Transition retVal = new Transition();

            transitions.Add(retVal);
            return retVal;
        }
    }
}