using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class DrawUtils
{

    public static void GizmosArrow(Vector3 position, Vector3 direction, Color color)
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = color;

        Gizmos.DrawRay(position, direction);
        DrawCone(position + direction, -direction * 0.333f, color, 15);

        Gizmos.color = oldColor;
    }

    public static void DebugCylinder(Vector3 start, Vector3 end, Color color, float radius = 1, float duration = 0, bool depthTest = true)
    {
        Vector3 up = (end - start).normalized * radius;
        Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
        Vector3 right = Vector3.Cross(up, forward).normalized * radius;

        //Radial circles
        DebugCircle(start, up, color, radius, duration, depthTest);
        DebugCircle(end, -up, color, radius, duration, depthTest);
        DebugCircle((start + end) * 0.5f, up, color, radius, duration, depthTest);

        //Side lines
        Debug.DrawLine(start + right, end + right, color, duration, depthTest);
        Debug.DrawLine(start - right, end - right, color, duration, depthTest);

        Debug.DrawLine(start + forward, end + forward, color, duration, depthTest);
        Debug.DrawLine(start - forward, end - forward, color, duration, depthTest);

        //Start endcap
        Debug.DrawLine(start - right, start + right, color, duration, depthTest);
        Debug.DrawLine(start - forward, start + forward, color, duration, depthTest);

        //End endcap
        Debug.DrawLine(end - right, end + right, color, duration, depthTest);
        Debug.DrawLine(end - forward, end + forward, color, duration, depthTest);
    }

    public static void DebugArrow(Vector3 position, Vector3 direction, Color color, float duration = 0, bool depthTest = true)
    {
        Debug.DrawRay(position, direction, color, duration, depthTest);
        DrawUtils.DebugCone(position + direction, -direction * 0.333f, color, 15, duration, depthTest);
    }

    public static void DebugCone(Vector3 position, Vector3 direction, Color color, float angle = 45, float duration = 0, bool depthTest = true)
    {
        float length = direction.magnitude;

        Vector3 _forward = direction;
        Vector3 _up = Vector3.Slerp(_forward, -_forward, 0.5f);
        Vector3 _right = Vector3.Cross(_forward, _up).normalized * length;

        direction = direction.normalized;

        Vector3 slerpedVector = Vector3.Slerp(_forward, _up, angle / 90.0f);

        float dist;
        var farPlane = new Plane(-direction, position + _forward);
        var distRay = new Ray(position, slerpedVector);

        farPlane.Raycast(distRay, out dist);

        Debug.DrawRay(position, slerpedVector.normalized * dist, color);
        Debug.DrawRay(position, Vector3.Slerp(_forward, -_up, angle / 90.0f).normalized * dist, color, duration, depthTest);
        Debug.DrawRay(position, Vector3.Slerp(_forward, _right, angle / 90.0f).normalized * dist, color, duration, depthTest);
        Debug.DrawRay(position, Vector3.Slerp(_forward, -_right, angle / 90.0f).normalized * dist, color, duration, depthTest);

        DebugCircle(position + _forward, direction, color, (_forward - (slerpedVector.normalized * dist)).magnitude, duration, depthTest);
        DebugCircle(position + (_forward * 0.5f), direction, color, ((_forward * 0.5f) - (slerpedVector.normalized * (dist * 0.5f))).magnitude, duration, depthTest);
    }

    public static void DrawCone(Vector3 position, Vector3 direction, Color color, float angle = 45)
    {
        float length = direction.magnitude;

        Vector3 _forward = direction;
        Vector3 _up = Vector3.Slerp(_forward, -_forward, 0.5f);
        Vector3 _right = Vector3.Cross(_forward, _up).normalized * length;

        direction = direction.normalized;

        Vector3 slerpedVector = Vector3.Slerp(_forward, _up, angle / 90.0f);

        float dist;
        var farPlane = new Plane(-direction, position + _forward);
        var distRay = new Ray(position, slerpedVector);

        farPlane.Raycast(distRay, out dist);

        Color oldColor = Gizmos.color;
        Gizmos.color = color;

        Gizmos.DrawRay(position, slerpedVector.normalized * dist);
        Gizmos.DrawRay(position, Vector3.Slerp(_forward, -_up, angle / 90.0f).normalized * dist);
        Gizmos.DrawRay(position, Vector3.Slerp(_forward, _right, angle / 90.0f).normalized * dist);
        Gizmos.DrawRay(position, Vector3.Slerp(_forward, -_right, angle / 90.0f).normalized * dist);

        DrawCircle(position + _forward, direction, color, (_forward - (slerpedVector.normalized * dist)).magnitude);
        DrawCircle(position + (_forward * 0.5f), direction, color, ((_forward * 0.5f) - (slerpedVector.normalized * (dist * 0.5f))).magnitude);

        Gizmos.color = oldColor;
    }

    public static void DebugPoint(Vector3 position, Color color, float scale = 1.0f, float duration = 0, bool depthTest = true)
    {
        color = (color == default(Color)) ? Color.white : color;

        Debug.DrawRay(position + (Vector3.up * (scale * 0.5f)), -Vector3.up * scale, color, duration, depthTest);
        Debug.DrawRay(position + (Vector3.right * (scale * 0.5f)), -Vector3.right * scale, color, duration, depthTest);
        Debug.DrawRay(position + (Vector3.forward * (scale * 0.5f)), -Vector3.forward * scale, color, duration, depthTest);
    }

    public static void DebugCircle(Vector3 position, Vector3 up, Color color, float radius = 1.0f, float duration = 0, bool depthTest = true)
    {
        Vector3 _up = up.normalized * radius;
        Vector3 _forward = Vector3.Slerp(_up, -_up, 0.5f);
        Vector3 _right = Vector3.Cross(_up, _forward).normalized * radius;

        Matrix4x4 matrix = new Matrix4x4();

        matrix[0] = _right.x;
        matrix[1] = _right.y;
        matrix[2] = _right.z;

        matrix[4] = _up.x;
        matrix[5] = _up.y;
        matrix[6] = _up.z;

        matrix[8] = _forward.x;
        matrix[9] = _forward.y;
        matrix[10] = _forward.z;

        Vector3 _lastPoint = position + matrix.MultiplyPoint3x4(new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)));
        Vector3 _nextPoint = Vector3.zero;

        color = (color == default(Color)) ? Color.white : color;

        for (var i = 0; i < 91; i++)
        {
            _nextPoint.x = Mathf.Cos((i * 4) * Mathf.Deg2Rad);
            _nextPoint.z = Mathf.Sin((i * 4) * Mathf.Deg2Rad);
            _nextPoint.y = 0;

            _nextPoint = position + matrix.MultiplyPoint3x4(_nextPoint);

            Debug.DrawLine(_lastPoint, _nextPoint, color, duration, depthTest);
            _lastPoint = _nextPoint;
        }
    }

    public static void DrawPoint(Vector3 position, Color color, float scale = 1.0f)
    {
        Color oldColor = Gizmos.color;

        Gizmos.color = color;
        Gizmos.DrawRay(position + (Vector3.up * (scale * 0.5f)), -Vector3.up * scale);
        Gizmos.DrawRay(position + (Vector3.right * (scale * 0.5f)), -Vector3.right * scale);
        Gizmos.DrawRay(position + (Vector3.forward * (scale * 0.5f)), -Vector3.forward * scale);

        Gizmos.color = oldColor;
    }

    public static void DrawCircle(Vector3 position, Vector3 up, Color color, float radius = 1.0f)
    {
        up = ((up == Vector3.zero) ? Vector3.up : up).normalized * radius;
        Vector3 _forward = Vector3.Slerp(up, -up, 0.5f);
        Vector3 _right = Vector3.Cross(up, _forward).normalized * radius;

        Matrix4x4 matrix = new Matrix4x4();

        matrix[0] = _right.x;
        matrix[1] = _right.y;
        matrix[2] = _right.z;

        matrix[4] = up.x;
        matrix[5] = up.y;
        matrix[6] = up.z;

        matrix[8] = _forward.x;
        matrix[9] = _forward.y;
        matrix[10] = _forward.z;

        Vector3 _lastPoint = position + matrix.MultiplyPoint3x4(new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)));
        Vector3 _nextPoint = Vector3.zero;

        Color oldColor = Gizmos.color;
        Gizmos.color = (color == default(Color)) ? Color.white : color;

        for (var i = 0; i < 91; i++)
        {
            _nextPoint.x = Mathf.Cos((i * 4) * Mathf.Deg2Rad);
            _nextPoint.z = Mathf.Sin((i * 4) * Mathf.Deg2Rad);
            _nextPoint.y = 0;

            _nextPoint = position + matrix.MultiplyPoint3x4(_nextPoint);

            Gizmos.DrawLine(_lastPoint, _nextPoint);
            _lastPoint = _nextPoint;
        }

        Gizmos.color = oldColor;
    }
}

