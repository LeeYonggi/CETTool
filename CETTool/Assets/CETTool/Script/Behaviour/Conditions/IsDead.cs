using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CET
{
    [CreateAssetMenu(menuName = "Conditions/Is Dead")]
    public class IsDead : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            return state.health <= 0;
        }
    }
}