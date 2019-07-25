using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LayoutChangeer : MonoBehaviour
{
    public Transform NexTransform;

    void Start()
    {
        Act();
    }

    private void Act()
    {
        
        var layouyut = transform.GetComponent<RectTransform>();
        var count = transform.childCount;
        var layout = transform.GetComponent<HorizontalLayoutGroup>();
        var spacing = layout.spacing;
        var totalW = layouyut.rect.width;
        var w = (totalW - spacing*(count - 2))/count;
        var b = layouyut.rect.height;
        var half = w/2;

        var size = new Vector2(w,b);
        


        for (int i = 0; i < count; i++)
        {
            var tr = transform.GetChild(0);
            var rectTransfrom = tr.GetComponent<RectTransform>();
            var px = half + i*(spacing+w) - totalW/2;
//            Debug.Log(tr.position + "  " + tr.localPosition + "   " + tr.GetComponent<RectTransform>() + "   " + tr.name);
//            Debug.Log(rectTransfrom.sizeDelta + "  " + rectTransfrom.pivot + "  " + rectTransfrom.anchoredPosition + "  " + rectTransfrom.anchoredPosition3D);
//            var el = tr.GetComponent<LayoutElement>();
//            el.
//            el.ignoreLayout = true;
//            var pos = rectTransfrom.position;
            tr.SetParent(NexTransform, true);
            rectTransfrom.sizeDelta = size;
            rectTransfrom.localPosition = new Vector3(px,0);
        }
        
    }

    private IEnumerator YYY()
    {
        yield return new WaitForSeconds(1f);
        Act();
    }
}

