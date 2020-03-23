using UnityEngine;

public class YMoveRotation
{
    private float _xzMoveCoef = 1f;
    private float _yMoveCoef = 0f;
    public YMoveRotation()
    {
        RotateQuaternion = Quaternion.identity;
    }

    // public bool ImplementedY { get; private set; }
    public Quaternion RotateQuaternion { get; private set; }

    public float YMoveCoef => _yMoveCoef;

    public float XzMoveCoef => _xzMoveCoef;

    // public void CompleteY()
    // {
    //     // ImplementedY = true;
    // }

    public void SetYDir(Quaternion quaternion, float moveCoef)
    {
        _xzMoveCoef = Mathf.Clamp01(moveCoef);
        _yMoveCoef = 1f - XzMoveCoef * .5f;
        RotateQuaternion = quaternion;
    }

}