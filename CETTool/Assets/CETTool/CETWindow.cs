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

        private static CETWindow instance = null;

        private static BehaviourGraph currentGraph;

        private static BehaviourGraph previousGraph = null;

        public readonly Vector2 stateNodeSize = new Vector2(200, 250);


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

        #region Property
        public static CETWindow Instance 
        {
            get
            {
                if (instance == null)
                    instance = EditorWindow.GetWindow<CETWindow>();

                return instance;
            }
        }

        static public BehaviourGraph CurrentGraph { get => currentGraph; set => currentGraph = value; }
        #endregion

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
            if (currentGraph != null)
                EditorUtility.SetDirty(currentGraph);
        }

        private void OnDisable()
        {
            if (currentGraph != null)
                EditorUtility.SetDirty(currentGraph);
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

            DrawGraph();

            EndWindows();
        }

        void DrawNodeWindow(int id)
        {
            windows[id].DrawWindow();
            GUI.DragWindow();
        }
        #endregion


        #region Helper Methods
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


                if(e.type == EventType.MouseDrag)
                {
                    for (int i = 0; i < windows.Count; i++)
                    {
                        if (windows[i].windowRect.Contains(e.mousePosition))
                        {
                            if (currentGraph != null)
                                currentGraph.SetNode(windows[i]);
                            break;
                        }
                    }
                }
            }
        }

        void RightClick(Event e)
        {
            selectedNode = null;
            clickedOnWindow = false;
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(e.mousePosition))
                {
                    clickedOnWindow = true;
                    selectedNode = windows[i];
                    break;
                }
            }

            if (!clickedOnWindow)
            {
                CreateNode(e);
            }
            else
            {
                ModifyNode(e);
            }
        }
        #endregion

        #region Node Method
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
                    AddStateNode(new Vector2(mousePosition.x, mousePosition.y));

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

                        currentGraph.RemoveStateNode(target);
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
                    CommentNode commentNode = CreateInstance<CommentNode>();

                    commentNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 100);
                    commentNode.windowTitle = "Comment";
                    

                    windows.Add(commentNode);
                    break;
                default:
                    break;
            }
        }
        public TransitionNode AddTransitionNode(int index, Transition transition, StateNode from)
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


        public void DrawNodeCurve(Rect start, Rect end, bool left, Color curveColor)
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

        public void ClearWindowsFormList(List<BaseNode> baseNodes)
        { 
            for(int i = 0; i < baseNodes.Count; i++)
            {
                if (windows.Contains(baseNodes[i]))
                    windows.Remove(baseNodes[i]);
            }
        }

        public StateNode AddStateNode(Vector2 pos)
        {
            if (currentGraph == null)
                return null;
            StateNode stateNode = CreateInstance<StateNode>();

            stateNode.windowRect = new Rect(pos.x, pos.y, stateNodeSize.x, stateNodeSize.y);
            stateNode.windowTitle = "State";

            windows.Add(stateNode);

            currentGraph?.SetStateNode(stateNode);

            return stateNode;
        }

        #endregion

        #region Graph Method

        private void DrawGraph()
        {
            if (currentGraph == null)
                EditorGUILayout.LabelField("Add Graph: ");

            currentGraph = (BehaviourGraph)EditorGUILayout.ObjectField(currentGraph, typeof(BehaviourGraph), false);

            if(previousGraph != currentGraph)
            {
                previousGraph = currentGraph;

                windows.Clear();

                LoadGraph();

                //if (currentGraph == null)
                //    return;

                //for (int i = 0; i < currentGraph.savedStateNodes.Count; i++)
                //{
                //    StateNode stateNode = new StateNode();


                //    windows.Add(stateNode);

                //    currentGraph.AddStateNode(stateNode, currentGraph.savedStateNodes[i]);
                //}
            }
        }

        private void LoadGraph()
        {
            if (currentGraph == null)
                return;
            currentGraph.Init();

            List<Saved_StateNode> stateNodeList = new List<Saved_StateNode>();

            stateNodeList.AddRange(currentGraph.savedStateNodes);
            currentGraph.savedStateNodes.Clear();

            for(int i = stateNodeList.Count - 1; i >= 0; i--)
            {
                StateNode stateNode = AddStateNode(stateNodeList[i].position);

                stateNode.currentState = stateNodeList[i].state;
                stateNode.collapse = stateNodeList[i].isCollaps;

                currentGraph.SetStateNode(stateNode);

                stateNode.NodeInitialize();
            }
        }

        #endregion
    }
}