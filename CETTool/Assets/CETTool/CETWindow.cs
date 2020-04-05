using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CET
{
    public class CETWindow : EditorWindow
    {
        #region Variables
        static List<BaseNode> windows = new List<BaseNode>();
        bool makeTransition = false;
        bool clickedOnWindow;
        BaseNode selectedNode;

        public enum UserActions
        {
            AddState,
            AddTransitionNode,
            DeleteNode,
            CommentNode
        }

        #endregion

        Rect node1 = new Rect(10, 10, 100, 100);
        Rect node2 = new Rect(210, 10, 100, 100);
        Vector3 mousePosition;

        int index = 0;


        #region Draw Window
        private void OnGUI()
        {
            Event e = Event.current;

            mousePosition = e.mousePosition;
            UserInput(e);

            DrawWindows();
        }

        private void OnEnable()
        {
            windows.Clear();
        }

        void DrawWindows()
        {
            BeginWindows();

            foreach(var node in windows)
            {
                node.DrawCurve();
            }

            for(int i = 0; i < windows.Count; i++)
            {
                windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
            }

            EndWindows();
        }

        void DrawNodeWindow(int id)
        {
            windows[id].DrawWindow();
            GUI.DragWindow();
        }
        #endregion

        void UserInput(Event e)
        {
            if(e.button == 1 && !makeTransition)
            {
                if(e.type == EventType.MouseDown)
                {
                    RightClick(e);
                }
            }

            if(e.button== 0 && !makeTransition)
            {
                if(e.type == EventType.MouseDown)
                {

                }
            }

        }

        void RightClick(Event e)
        {
            selectedNode = null;
            clickedOnWindow = false;

            for (int i = 0; i < windows.Count; i++)
            {
                if(windows[i].windowRect.Contains(e.mousePosition))
                {
                    clickedOnWindow = true;
                    selectedNode = windows[i];
                    break;
                }
            }

            if(!clickedOnWindow)
            {
                CreateNode(e);
            }
            else
            {
                ModifyNode(e);
            }
        }

        private void CreateNode(Event e)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Add State"), false, ConTextCallback, UserActions.AddState);
            menu.AddItem(new GUIContent("Add Comment"), false, ConTextCallback, UserActions.CommentNode);
            
            menu.ShowAsContext();

            e.Use();
        }

        void ModifyNode(Event e)
        {
            GenericMenu menu = new GenericMenu();

            if (selectedNode is StateNode)
            {
                StateNode stateNode = selectedNode as StateNode;
                if (stateNode.currentState != null)
                {
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Add Transition"), false, ConTextCallback, UserActions.AddTransitionNode);
                }
                else
                {
                    menu.AddSeparator("");
                    menu.AddDisabledItem(new GUIContent("Add Transition"));
                }
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ConTextCallback, UserActions.DeleteNode);
            }

            if (selectedNode is TransitionNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ConTextCallback, UserActions.DeleteNode);
            }

            if(selectedNode is CommentNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ConTextCallback, UserActions.DeleteNode);
            }
            menu.ShowAsContext();
            e.Use();
        }

        void ConTextCallback(object o)
        {
            UserActions action = (UserActions)o;

            switch (action)
            {
                case UserActions.AddState:
                    StateNode stateNode = new StateNode{
                        windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 250),
                        windowTitle = "State"
                    };
                    windows.Add(stateNode);

                    break;
                case UserActions.AddTransitionNode:
                    if(selectedNode is StateNode)
                    {
                        StateNode from = (StateNode)selectedNode;
                        from.AddTransition();
                    }

                    break;
                case UserActions.DeleteNode:
                    if(selectedNode is StateNode)
                    {
                        StateNode target = (StateNode)selectedNode;

                        target.ClearReferences();
                        windows.Remove(target);
                    }

                    if(selectedNode is TransitionNode)
                    {
                        TransitionNode target = (TransitionNode)selectedNode;

                        windows.Remove(target);

                        if (target.enterState.currentState.transitions.Contains(target.targetTransition))
                            target.enterState.currentState.transitions.Remove(target.targetTransition);
                    }

                    if(selectedNode is CommentNode)
                    {
                        windows.Remove(selectedNode);
                    }

                    break;
                case UserActions.CommentNode:
                    CommentNode commentNode = new CommentNode
                    {
                        windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 100),
                        windowTitle = "Comment"
                    };

                    windows.Add(commentNode);
                    break;
                default:
                    break;
            }
        }

        #region Helper Methods
        public static TransitionNode AddTransitionNode(int index, Transition transition, StateNode from)
        {
            Rect fromRect = from.windowRect;

            fromRect.x += 50;

            float targetY = fromRect.y - fromRect.height;

            if(from.currentState != null)
            {
                targetY += (index * 100);
            }

            fromRect.y = targetY;

            TransitionNode transitionNode = CreateInstance<TransitionNode>();
            transitionNode.Init(from, transition);
            transitionNode.windowRect = new Rect(fromRect.x + 200 + 100, fromRect.y + (fromRect.height * 0.7f), 200, 80);
            transitionNode.windowTitle = "Condition Check";

            windows.Add(transitionNode);

            return transitionNode;
        }

        #endregion

        public static void DrawNodeCurve(Rect start, Rect end, bool left, Color curveColor)
        {
            Vector3 startPos = new Vector3(
                (left) ? start.x + start.width : start.x,
                start.y + (start.height * 0.5f),
                0);

            Vector3 endPos = new Vector3(end.x + (end.width * 0.5f), end.y + (end.height * 0.5f), 0);
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;

            Color shadow = new Color(0, 0, 0, 0.06f);

            for(int i = 0; i < 3; i++)
            {
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadow, null, (i + 1) * 0.5f);
            }

            Handles.DrawBezier(startPos, endPos, startTan, endTan, curveColor, null, 1);
        }

        public static void ClearWindowsFormList(List<BaseNode> baseNodes)
        { 
            for(int i = 0; i < baseNodes.Count; i++)
            {
                if (windows.Contains(baseNodes[i]))
                    windows.Remove(baseNodes[i]);
            }
        }
    }
}