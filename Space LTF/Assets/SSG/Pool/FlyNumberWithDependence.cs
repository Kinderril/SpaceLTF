using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FlyNumberWithDependence : PoolElement
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI subText;
    public Image image;
    private Animator anim;
    private Transform dependence;
    private CameraController cam;
    private Vector3 _offset;
    private float offsetConst = 15f;


    public static FlyNumberWithDependence Create(Transform tr, string str, Color color, FlyNumerDirection flyDir)
    {
        return Create(tr,str,color, WindowManager.Instance.TopPanel.transform, flyDir);
    }

    public static FlyNumberWithDependence Create(Transform tr, string str, Color color,Transform transformHolder, FlyNumerDirection flyDir)
    {
        var fn = DataBaseController.Instance.Pool.GetItemFromPool<FlyNumberWithDependence>(PoolType.flyNumberInGame);
        fn.transform.SetParent(transformHolder,false);
        fn.Init(tr, str, color, flyDir);
        return fn;
    }

    private void Init(Transform dependence, string msg, Color textColor, FlyNumerDirection flyDir = FlyNumerDirection.random, int size = 42)
    {
        base.Init();
        _offset= new Vector3(MyExtensions.Random(-offsetConst, offsetConst), MyExtensions.Random(-offsetConst, offsetConst));
        cam = CamerasController.Instance.GameCamera;
        this.dependence = dependence;
//        this.OnDead = OnDead;
        FlyingNumberInit(msg, textColor, flyDir, size);
    }

    private void FlyingNumberInit(string msg, Color textColor, FlyNumerDirection flyDir = FlyNumerDirection.random, int size = 42)
    {
        base.Init();
        if (image != null)
            image.enabled = false;
        text.text = msg;
        text.fontSize = size;
        if (textColor != default(Color))
            text.color = textColor;
        if (subText != null)
        {
            subText.gameObject.SetActive(false);
        }
        subInit(flyDir);
    }


    private void subInit(FlyNumerDirection flyDir)
    {
        //        transform.localScale = Vector3.one;
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
        if (anim != null)
        {
            //            anim.SetTrigger(keyRight);
            switch (flyDir)
            {
                case FlyNumerDirection.random:
                    if (UnityEngine.Random.Range(0, 100) > 50)
                    {
                        anim.SetTrigger(FlyingNumbers.keyLeft);
                    }
                    else
                    {
                        anim.SetTrigger(FlyingNumbers.keyRight);
                    }
                    break;
                case FlyNumerDirection.left:
                    anim.SetTrigger(FlyingNumbers.keyLeft);
                    break;
                case FlyNumerDirection.right:
                    anim.SetTrigger(FlyingNumbers.keyRight);
                    break;
            }
        }
    }

    private void OnDead()
    {
        EndUse();
    }

    void Update()
    {
        if (IsUsing && dependence != null)
        {
            var p = cam.WorldToScreenPoint(dependence.position);
//            Debug.LogError("pos " + p + "   <>   " + dependence.position);
            transform.position = p + _offset;
        }
    }
}

