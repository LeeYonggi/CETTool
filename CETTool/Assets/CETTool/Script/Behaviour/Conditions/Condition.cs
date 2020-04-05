using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CET
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool CheckCondition(StateManager state);

    }
}