using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CETWindow : EditorWindow
{
    List<Rect> nodeList = new List<Rect>();
    Rect node1 = new Rect(10, 10, 100, 100);
    Rect node2 = new Rect(210, 10, 100, 100);

    int index = 0;
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        BeginWindows();

        GenericMenu menu = new GenericMenu();

        menu.AddItem(new GUIContent("노드추가"), false, CreateNode, "Node");

        node1 = GUI.Window(1, node1, WindowCallBack, "node1", "flow node 1");
        node2 = GUI.Window(2, node2, WindowCallBack2, "node2", "flow node 3");

        var start = new Vector3(node1.x + node1.width, node1.y + node1.height / 2.0f, 0.0f);
        var startTan = start + new Vector3(100.0f, 0.0f, 0.0f);

        var end = new Vector3(node2.x, node2.y + node2.height / 2.0f, 0.0f);
        var endTan = end + new Vector3(-100.0f, 0.0f, 0.0f);

        Handles.DrawBezier(start, end, startTan, endTan, Color.red, null, 3.0f);

        EndWindows();
    }

    private void CreateNode(object obj)
    {

    }

    void WindowCallBack(int id)
    {
        GUI.DragWindow();
    }

    void WindowCallBack2(int id)
    {
        GUI.DragWindow();
    }
}
