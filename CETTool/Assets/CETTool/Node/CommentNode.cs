using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CET
{
    public class CommentNode : BaseNode
    {
        string comment = "This is a comment";

        public override void DrawWindow()
        {
            base.DrawWindow();

            comment = GUILayout.TextArea(comment, 200);
        }
    }
}