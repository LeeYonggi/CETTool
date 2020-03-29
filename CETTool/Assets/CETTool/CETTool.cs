using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class CETTool : MonoBehaviour
{
    static CETWindow cetWindow = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [MenuItem("CETTool/Open Node Window")]
    static void OpenNodeWindow()
    {
        cetWindow = EditorWindow.GetWindow<CETWindow>();

        cetWindow.maxSize = new Vector2(1280, 720);
        cetWindow.minSize = new Vector2(1280, 720);

        cetWindow.Show();
    }
}
