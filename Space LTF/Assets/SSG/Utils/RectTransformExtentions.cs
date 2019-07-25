using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

public static class RectTransformExtensions
{
    public static int ActiveChildCount(this Transform transform)
    {
        return transform.Cast<Transform>().Count(child => child.gameObject.activeSelf);
    }

    public static void SetRectTransformParent(this RectTransform transform, RectTransform objectToCopy)
    {
        transform.SetParent(objectToCopy);
        transform.anchorMin = new Vector2(0, 0);
        transform.anchorMax = new Vector2(1, 1);
        transform.offsetMax = new Vector2(0, 0);
        transform.offsetMin = new Vector2(0, 0);

        var localPosition = transform.localPosition;
        localPosition = new Vector3(localPosition.x, localPosition.y, 0);

        transform.localPosition = localPosition;
        transform.localScale = Vector3.one;
    }

    public static void ResetTransform(this Transform target)
    {
        target.localPosition = Vector3.zero;
        target.localRotation = Quaternion.identity;
        target.localScale = Vector3.one;
    }

    public static void CopyValues(this Transform target, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        target.localPosition = position;
        target.localRotation = Quaternion.Euler(rotation);
        target.localScale = scale;
    }

    public static void CopyValues(this Transform target, Transform template)
    {
        target.localPosition = template.localPosition;
        target.localRotation = template.localRotation;
        target.localScale = template.localScale;
    }

    public static void CopyValues(this Transform target, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        target.localPosition = position;
        target.localRotation = rotation;
        target.localScale = scale;
    }

    public static void SetInCenter(this Transform transform)
    {
        var rectTransform = transform as RectTransform;

        Assert.IsTrue(rectTransform != null);

        SetInCenter(rectTransform);
    }


    public static void CorrectPositionResolution(this Transform transform)
    {
        var rectTransform = transform as RectTransform;

        Assert.IsTrue(rectTransform != null);

        CorrectPositionResolution(rectTransform);
    }



    private static void FitTransformToRect(RectTransform rectTransform, Rect rect, [CanBeNull] Camera camera)
    {
        var corners = new Vector3[4];

        rectTransform.GetWorldCorners(corners);

        var point1 = RectTransformUtility.WorldToScreenPoint(camera, corners[0]);
        var point2 = RectTransformUtility.WorldToScreenPoint(camera, corners[2]);

        var diff1X = point1.x - rect.x;

        if (diff1X > 0)
            diff1X = 0;

        var diff1Y = point1.y - rect.y;

        if (diff1Y > 0)
            diff1Y = 0;

        rectTransform.anchoredPosition -= new Vector2(diff1X, diff1Y);

        var diff2X = rect.x + rect.width - point2.x;

        if (diff2X > 0)
            diff2X = 0;

        var diff2Y = rect.y + rect.height - point2.y;

        if (diff2Y > 0)
            diff2Y = 0;

        rectTransform.anchoredPosition += new Vector2(diff2X, diff2Y);
    }
}