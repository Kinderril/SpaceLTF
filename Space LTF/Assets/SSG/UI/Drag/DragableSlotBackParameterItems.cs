using System;
using UnityEngine;
using System.Collections;

public class DragableSlotBackParameterItems : MonoBehaviour
{
    public GameObject Cocpit;
    public GameObject Engine;
    public GameObject Wings;

    public void Init(DragItemType type)
    {
        switch (type)
        {
            case DragItemType.cocpit:
                gameObject.SetActive(true);
                Cocpit.SetActive(true);
                Engine.SetActive(false);
                Wings.SetActive(false);
                break;
            case DragItemType.engine:
                gameObject.SetActive(true);
                Cocpit.SetActive(false);
                Engine.SetActive(true);
                Wings.SetActive(false);
                break;
            case DragItemType.wings:
                gameObject.SetActive(true);
                Cocpit.SetActive(false);
                Engine.SetActive(false);
                Wings.SetActive(true);
                break;
            default:
                gameObject.SetActive(false);
                break;
        }
    }

}
