using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CET
{
    [CreateAssetMenu(menuName = "Actions/Test/Add Health")]
    public class AddHealth : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.health += 0.1f;

            states.DebugLog($"{states.health}");
        }
    }
}