using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CET
{
    [CreateAssetMenu(menuName = "Conditions/IsHealthUp")]
    public class IsHealthUp : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            return (state.health > 10.0f);
        }
    }
}