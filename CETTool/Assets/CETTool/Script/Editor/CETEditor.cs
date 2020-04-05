using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CET
{
    [InitializeOnLoad]
    public class CETEditor : MonoBehaviour
    {
        static CETWindow cetWindow = null;

        #region Init
        [MenuItem("CETTool/Open Node Window")]
        static void OpenNodeWindow()
        {
            cetWindow = EditorWindow.GetWindow<CETWindow>();

            cetWindow.maxSize = new Vector2(1280, 720);
            cetWindow.minSize = new Vector2(1280, 720);

            cetWindow.Show();
        }
        #endregion

    }
}