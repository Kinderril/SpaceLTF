
using UnityEngine;

public static class GizmoUtils
{
    public static void DrawCube(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        var oldGizmosMatrix = Gizmos.matrix;

        Gizmos.matrix *= Matrix4x4.TRS(position, rotation, scale);
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
        Gizmos.matrix = oldGizmosMatrix;
    }

    public static void DrawWireCube(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        var oldGizmosMatrix = Gizmos.matrix;

        Gizmos.matrix *= Matrix4x4.TRS(position, rotation, scale);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.matrix = oldGizmosMatrix;
    }

    private static readonly Quaternion _YAW90_DEGREES = Quaternion.Euler(0, 90, 0);

    public static void DrawArrow(Vector3 origin, Vector3 direction)
    {
        var destination = origin + direction;
        var left = _YAW90_DEGREES * destination * 0.1f;
        left = Vector3.ClampMagnitude(left, 0.03f);
        Gizmos.DrawLine(origin, destination);
        Gizmos.DrawLine(destination, origin + left + direction * 0.7f);
        Gizmos.DrawLine(destination, origin - left + direction * 0.7f);
    }

    public class Styles
    {
        public readonly GUIStyle enabledStateName;

        public Styles()
        {
            enabledStateName = new GUIStyle("MeTransitionSelect");
            enabledStateName.alignment = TextAnchor.UpperLeft;
            enabledStateName.padding = new RectOffset(2, 2, 1, 1);
            enabledStateName.fontStyle = FontStyle.Bold;
            enabledStateName.normal.textColor = Color.white;
        }
    }
}
