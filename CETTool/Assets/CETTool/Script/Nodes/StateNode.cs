using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CET
{
    public class StateNode : BaseNode
    {
        public bool collapse;
        public State currentState;
        State previousState;

        SerializedObject serializedState;
        StateActions onState;

        List<BaseNode> dependencies = new List<BaseNode>();

        public override void DrawCurve()
        {
            base.DrawCurve();
        }

        public override void DrawWindow()
        {
            base.DrawWindow();

            if(currentState == null)
            {
                EditorGUILayout.LabelField("Add state to modify:");
            }
            else
            {
                if(!collapse)
                {
                    windowRect.height = 300;
                }
                else
                {
                    windowRect.height = 100;
                }

                bool prevCollapse = collapse;

                collapse = EditorGUILayout.Toggle("Window Reduction", collapse);

                if (collapse != prevCollapse)
                    CETWindow.CurrentGraph.SetStateNode(this);
            }

            currentState = (State)EditorGUILayout.ObjectField(currentState, typeof(State), false);

            if(previousState != currentState)
            {
                NodeInitialize();

                //CETWindow.CurrentGraph.SetStateNode(this);
            }

            if(currentState != null)
            {
                if(serializedState == null)
                {
                    serializedState = new SerializedObject(currentState);
                    onState = (StateActions)EditorGUILayout.ObjectField(onState, typeof(StateActions), false);
                }

                if(!collapse)
                {
                    serializedState.Update();

                    serializedState.ApplyModifiedProperties();

                    float standard = 300;

                    standard += 1 * 20;
                    windowRect.height = standard;
                }

                CETWindow.CurrentGraph.SetStateNode(this);
            }
        }

        public void NodeInitialize()
        {
            serializedState = null;

            previousState = currentState;

            ClearReferences();

            for (int i = 0; i < currentState.transitions.Count; i++)
            {
                dependencies.Add(CETWindow.Instance.AddTransitionNode(i, currentState.transitions[i], this));
            }
        }

        void HandleReordableList(ReorderableList list, string targetName)
        {
            list.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, targetName);
            };

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
            };
        }

        public Transition AddTransition()
        {
            Transition transition = currentState.AddTransition();
            
            dependencies.Add(CETWindow.Instance.AddTransitionNode(currentState.transitions.Count, transition, this));

            return transition;
        }

        public void ClearReferences()
        {
            for (int i = 0; i < dependencies.Count; i++)
            {
                TransitionNode node = dependencies[i] as TransitionNode;

                node.ClearReference();
            }
            CETWindow.Instance.ClearWindowsFormList(dependencies);
            dependencies.Clear();
        }

    }
}