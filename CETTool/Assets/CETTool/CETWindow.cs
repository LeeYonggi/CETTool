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
                CreateNode(e);
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

            if(selectedNode is StateNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ConTextCallback, UserActions.DeleteNode);
            }
            menu.ShowAsContext();
        }

        void ConTextCallback(object o)
        {
            UserActions action = (UserActions)o;

            switch (action)
            {
                case UserActions.AddState:
                    StateNode stateNode = new StateNode{
                        windowRect = new Rect(mousePosition.x, mousePosition.y, 150, 250),
                        windowTitle = "State"
                    };
                    windows.Add(stateNode);

                    break;
                case UserActions.AddTransitionNode:
                    break;
                case UserActions.DeleteNode:
                    if(selectedNode != null)
                    {
                        windows.Remove(selectedNode);
                    }

                    break;
                case UserActions.CommentNode:
                    CommentNode commentNode = new CommentNode
                    {
                        windowRect = new Rect(mousePosition.x, mousePosition.y, 150, 100),
                        windowTitle = "State"
                    };

                    windows.Add(commentNode);
                    break;
                default:
                    break;
            }
        }

        #region sample
        void Sample()
        {
            //GenericMenu menu = new GenericMenu();

            //menu.AddItem(new GUIContent("노드추가"), false, CreateNode, "Node");

            //node1 = GUI.Window(1, node1, WindowCallBack, "node1", "flow node 1");
            //node2 = GUI.Window(2, node2, WindowCallBack2, "node2", "flow node 3");

            //var start = new Vector3(node1.x + node1.width, node1.y + node1.height / 2.0f, 0.0f);
            //var startTan = start + new Vector3(100.0f, 0.0f, 0.0f);

            //var end = new Vector3(node2.x, node2.y + node2.height / 2.0f, 0.0f);
            //var endTan = end + new Vector3(-100.0f, 0.0f, 0.0f);

            //Handles.DrawBezier(start, end, startTan, endTan, Color.red, null, 3.0f);
        }
        void WindowCallBack(int id)
        {
            GUI.DragWindow();
        }

        void WindowCallBack2(int id)
        {
            GUI.DragWindow();
        }
        #endregion
    }
}