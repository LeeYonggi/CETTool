using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CET
{
    public class ActionNode : BaseNode
    {
        public Action targetAction;
        public TransitionNode transitionNode;

        public void Init(Action action, TransitionNode transitionNode)
        {
            this.targetAction = action;
            this.transitionNode = transitionNode;
        }

        public override void DrawWindow()
        {
            base.DrawWindow();

            EditorGUILayout.LabelField("");
            targetAction.stateAction = (StateActions)EditorGUILayout.ObjectField(targetAction.stateAction, typeof(StateActions), false);

            if(targetAction.stateAction == null)
            {
                EditorGUILayout.LabelField("No Action!");
            }
            else
            {

            }
        }

        public override void DrawCurve()
        {
            base.DrawCurve();

            if(transitionNode)
            {
                Rect rect = windowRect;

                rect.y += windowRect.height * 0.5f;
                rect.width = 1;
                rect.height = 1;

                CETWindow.Instance.DrawNodeCurve(transitionNode.windowRect, rect, true, Color.white);
            }
        }
    }
}
