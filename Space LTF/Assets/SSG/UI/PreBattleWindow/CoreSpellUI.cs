using UnityEngine;
using System.Collections;

public class CoreSpellUI : MonoBehaviour
{
    public int index = 0;

    public void OnClick()
    {
        WindowManager.Instance.ItemInfosController.Init(index);
    }
}
