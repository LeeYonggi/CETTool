
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CET
{
    public abstract class StateActions : ScriptableObject
    {
        public abstract void Execute(StateManager states);
    }
}
