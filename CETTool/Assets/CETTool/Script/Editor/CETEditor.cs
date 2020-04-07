using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CET
{
    [InitializeOnLoad]
    public class CETEditor : MonoBehaviour
    {
        //public static CETWindow cetWindow = null;
        #region Init
        [MenuItem("CETTool/Open Node Window")]
        static void OpenNodeWindow()
        {
            CETWindow.Instance.maxSize = new Vector2(1280, 720);
            CETWindow.Instance.minSize = new Vector2(1280, 720);

            CETWindow.Instance.Show();
        }
        #endregion

    }
}