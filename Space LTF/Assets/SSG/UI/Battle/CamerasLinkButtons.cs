using UnityEngine;
using System.Collections;

public class CamerasLinkButtons : MonoBehaviour
{
    public CameraController MainCamera { get; set; }
    public ShipPursuitCameraHolder ShipPursuitCameraHolder;
    private ShipBase _lastSelectedShip;
    public GameObject ButtonOn;
    public GameObject ButtonOff;
    private int _isLinked;

    public void Init(CameraController mainCamera)
    {
        _isLinked = -1;
        MainCamera = mainCamera;
        CanActivate(true);
    }

    private void CanActivate(bool val)
    {
        var preCocl = _isLinked <= 0;
        var canActivate = val && preCocl;

        ButtonOn.SetActive(canActivate);
        ButtonOff.SetActive(!canActivate);
    }

    public void SelectedShip(ShipBase selected)
    {
        _lastSelectedShip = selected;
        CanActivate(true);
    }

    public void OnClickLink()
    {
        if (_lastSelectedShip != null)
        {
            LinkTo();
        }
    }

    public void OnClickUnlink()
    {
        _isLinked = -1;
        MainCamera.ReturnCamera();
        CanActivate(true);
    }

    public void LinkTo()
    {
        _isLinked = _lastSelectedShip.Id;
        ShipPursuitCameraHolder.Init(_lastSelectedShip, MainCamera.RotateHolder.position, ShipPursuitDeath);
        MainCamera.Camera.transform.SetParent(ShipPursuitCameraHolder.transform);
        MainCamera.Camera.transform.localPosition = Vector3.zero;
        MainCamera.Camera.transform.localRotation = Quaternion.identity;
        CanActivate(false);
    }

    private void ShipPursuitDeath()
    {
        _isLinked = -1;
        MainCamera.ReturnCamera();
        CanActivate(true);
    }

    public void Dispose()
    {
        ShipPursuitCameraHolder.Dispose();
    }
}
