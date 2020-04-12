using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CET
{
    public class TransitionNode : BaseNode
    {
        public Transition targetTransition;
        public StateNode enterState;
        public BaseNode targetNode;

        List<BaseNode> dependencies = new List<BaseNode>();

        public void Init(StateNode enterState, Transition transition)
        {
            this.enterState = enterState;
            this.targetTransition = transition;
        }

        public override void DrawWindow()
        {
            if (targetTransition == null)
                return;

            EditorGUILayout.LabelField("");
            targetTransition.condition = (Condition)EditorGUILayout.ObjectField(targetTransition.condition, typeof(Condition), false);

            if(targetTransition.condition == null)
            {
                EditorGUILayout.LabelField("No Condition!");
            }
            else
            {
                targetTransition.disable = EditorGUILayout.Toggle("Disable", targetTransition.disable);
            }

            if(targetNode == null && targetTransition.targetState != null)
            {
                targetNode = CETWindow.CurrentGraph.GetStateNode(targetTransition.targetState);
            }
        }

        public override void DrawCurve()
        {
            if(enterState)
            {
                Rect rect = windowRect;

                rect.y += windowRect.height * 0.5f;
                rect.width = 1;
                rect.height = 1;

                CETWindow.Instance.DrawNodeCurve(enterState.windowRect, rect, true, Color.black);
            }

            if(targetNode)
            {
                Rect rect = targetNode.windowRect;

                rect.y += windowRect.height * 0.5f;
                rect.width = 1;
                rect.height = rect.height * 0.5f;

                CETWindow.Instance.DrawNodeCurve(windowRect, rect, true, Color.red);
            }
        }

        #region Action Method
        public Action AddActionNode()
        {
            Action action = targetTransition.AddAction();

            dependencies.Add(CETWindow.Instance.AddActionNode(dependencies.Count, action, this));

            return action;
        }

        public void NodeInitialize()
        {
            ClearReference();

            for(int i = 0; i < targetTransition.targetActions.Count; i++)
            {
                dependencies.Add(CETWindow.Instance.AddActionNode(i, targetTransition.targetActions[i], this));
            }
        }

        public void ClearReference()
        {
            CETWindow.Instance.ClearWindowsFormList(dependencies);
            dependencies.Clear();
        }

        public void RemoveAction(Action action)
        {
            if(targetTransition.targetActions.Contains(action))
                targetTransition.targetActions.Remove(action);
        }

        #endregion

        public void ConnectStateNode(StateNode stateNode)
        {
            targetTransition.targetState = stateNode.currentState;
            targetNode = stateNode;
        }
    }
}
