using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;


public class BaseTooltip : UIElement
{
    [SerializeField]
    private RectTransform _mainTransform;
    private RectTransform _myRectTransform;
    private GameObject _causeTransform;

    //        private Transform baseParent;
    public bool Displayed { get; private set; }
    private Vector2 _offset;
//    private bool _isOpen = false;

    void Awake()
    {
        if (_myRectTransform == null)
        {
            _myRectTransform = GetComponent<RectTransform>();
        }
    }

    protected void Init(GameObject causeTransform)
    {
        _causeTransform = causeTransform;
        base.Init();
        transform.SetParent(WindowManager.Instance.TopPanel);
        Show();
    }

    protected void Show(Vector2 offset = default(Vector2), float delay = 0)
    {
        _offset = offset;

        SetPosition(Input.mousePosition);
//        _isOpen = true;
        ShowGameObject();
        Displayed = true;
    }

    private void SetPosition(Vector2 position)
    {
        var w = _myRectTransform.rect.width / 2f;
        var h= _myRectTransform.rect.height / 2f;

        _offset.x = w;
        _offset.y = h;
        var trgPos = position + _offset;
        float padding = 10f;

        var maxX = Screen.width - w - padding;
        var maxZ = Screen.height -h - padding;
        var minX = w + padding;
        var minZ =h + padding;


        var xx = Mathf.Clamp(trgPos.x, minX, maxX);
        var zz = Mathf.Clamp(trgPos.y, minZ, maxZ);

        var finalPos = new Vector2(xx,zz);

//        Debug.LogError($"postion: {position}   finalPos: {finalPos} minZ:{minZ}  maxZ:{maxZ}   minX:{minX}  maxX:{maxX}");

        transform.position = finalPos;
    }

    private void OnDisable()
    {
        Close();
    }

    public override void Close()
    {
        if (!Displayed)
        {
            return;
        }
//        _isOpen = false;
        base.Close();
        Displayed = false;
    }

    private void Update()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);
        foreach (RaycastResult result in raycastResults)
        {
            var tmpSlot = result.gameObject;
            if (tmpSlot != null)
            {
                if (tmpSlot == gameObject)
                {
                    return;
                }

                if (_causeTransform == tmpSlot)
                {
                    return;
                }
            }
        }
        Close();
    }

    public void SetBaseParent(Transform baseParent)
    {
        //            this.baseParent = baseParent;
        transform.SetParent(baseParent, false);
        _mainTransform = baseParent.GetComponent<RectTransform>();

    }
}
