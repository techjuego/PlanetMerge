using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomQuad))]
public class EditableQuadEditor : Editor
{
    void OnSceneGUI()
    {
        CustomQuad quad = (CustomQuad)target;

        // transform to handle local <-> world
        Transform t = quad.transform;

        EditorGUI.BeginChangeCheck();

        // Draw position handles for each corner
        Vector3 bl = Handles.PositionHandle(t.TransformPoint(quad.bottomLeft), Quaternion.identity);
        Vector3 br = Handles.PositionHandle(t.TransformPoint(quad.bottomRight), Quaternion.identity);
        Vector3 tl = Handles.PositionHandle(t.TransformPoint(quad.topLeft), Quaternion.identity);
        Vector3 tr = Handles.PositionHandle(t.TransformPoint(quad.topRight), Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(quad, "Move Quad Corner");

            quad.bottomLeft = t.InverseTransformPoint(bl);
            quad.bottomRight = t.InverseTransformPoint(br);
            quad.topLeft = t.InverseTransformPoint(tl);
            quad.topRight = t.InverseTransformPoint(tr);

            quad.BuildMesh();
        }

        // Optionally: draw lines between points
        Handles.color = Color.green;
        Handles.DrawLine(bl, br);
        Handles.DrawLine(br, tr);
        Handles.DrawLine(tr, tl);
        Handles.DrawLine(tl, bl);
    }
}
