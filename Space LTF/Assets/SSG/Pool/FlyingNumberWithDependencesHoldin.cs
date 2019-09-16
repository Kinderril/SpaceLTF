using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FlyingNumberWithDependencesHoldin : MonoBehaviour
{
    private const float UP_TIME = 1f;
    private const float STAY_TIME = 1f;

    private ShipBase _ship;
    private bool isActive = false;
    public TextMeshProUGUI Field;
    private float _lastChangeDelta;
    private bool _goindUp = false;
    private bool leftSide = false;
    private float _remainStepTime;
    public Animator Animation;
    private CameraController cam;
    private Transform dependence;
    private Vector3 _offset;
    private const float offsetConst = 15f;

    public static FlyingNumberWithDependencesHoldin Create(Transform flyingInfosContainer)
    {
        FlyingNumberWithDependencesHoldin prefab
            = DataBaseController.Instance.DataStructPrefabs.FlyingNumberWithDependencesHoldinPrefab;
        var elelemt = DataBaseController.GetItem(prefab);
        elelemt.gameObject.transform.SetParent(flyingInfosContainer);
        elelemt.cam = CamerasController.Instance.GameCamera;
        elelemt._offset = new Vector3(MyExtensions.Random(-offsetConst, offsetConst), MyExtensions.Random(-offsetConst, offsetConst));
//        elelemt.dependence = dependence;
        return elelemt;
    }

    public void LinkWithShip(ShipBase ship,bool health)
    {
        dependence = ship.transform;
        ship.OnDeath += OnDispose;
        ship.OnDispose += OnDispose;
        isActive = false;
        _lastChangeDelta = 0f;
        _ship = ship;
        leftSide = health;
        if (health)
        {
            Field.color = Color.red;
            _ship.ShipParameters.OnHealthChanged += OnHealthChanged;
        }
        else
        {
            Field.color = Color.blue;
            _ship.ShipParameters.ShieldParameters.OnShildChanged += OnShildChanged;
        }
    }

    private void OnDispose(ShipBase shipBase)
    {
        Dispose();
    }

    void Update()
    {
        if (isActive)
        {
            if (dependence != null)
            {
                var p = cam.WorldToScreenPoint(dependence.position);
                //            Debug.LogError("pos " + p + "   <>   " + dependence.position);
                transform.position = p + _offset;
            }
            _remainStepTime -= Time.deltaTime;
            if (_goindUp)
            {
                if (_remainStepTime < 0)
                {
                    _goindUp = false;
                    _remainStepTime = STAY_TIME;

                }
            }
            else
            {
                if (_remainStepTime < 0)
                {
                    AnimEnds();
                    _remainStepTime = STAY_TIME;

                }
            }
        }
    }

    private void OnShildChanged(float curent, float max, float delta, ShieldChangeSt state, ShipBase shipowner)
    {
        if (Mathf.Abs(delta) > 0.5f)
            Merge(delta);
    }

    private void OnHealthChanged(float curent, float max, float delta, ShipBase shipowner)
    {
        if (Mathf.Abs(delta) > 0.5f)
            Merge(delta);
    }

    private void Merge(float delta)
    {
//        Field.gameObject.SetActive(true);
        if (isActive)
        {
            ResetStayingTimer();
            _lastChangeDelta += delta;
        }
        else
        {
            Activated();
            _lastChangeDelta = delta;
        }
        var curInfo = _lastChangeDelta.ToString("0");
        Field.text = _lastChangeDelta > 0 ? String.Format("+{0}", curInfo) : curInfo;
    }

    private void ResetStayingTimer()
    {
        if (Animation != null)
            Animation.SetTrigger("Jump");
        _remainStepTime = STAY_TIME;
    }

    private void Activated()
    {
        _remainStepTime = UP_TIME;
        _goindUp = true;
        isActive = true;
        Field.gameObject.SetActive(isActive);
    }

    private void AnimEnds()
    {
        isActive = false;
        Field.gameObject.SetActive(isActive);
        _lastChangeDelta = 0f;
//        Field.gameObject.SetActive(isActive);
    }

    public void Dispose()
    {
        if (leftSide)
        {
            _ship.ShipParameters.OnHealthChanged -= OnHealthChanged;

        }
        else
        {
            _ship.ShipParameters.ShieldParameters.OnShildChanged -= OnShildChanged;
        }
    }
}

