using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CET
{
    [CreateAssetMenu(menuName = "Actions/Test/GameStart")]
    public class GameStart : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.DebugLog("Start!");
        }
    }
}