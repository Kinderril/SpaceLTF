using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageWithTooltip : UIElementWithTooltip
{
    public Image Image;
    private string _data;
    public void Init(Sprite sprite,string data)
    {
        _data = data;
        Image.sprite = sprite;
    }

    protected override string TextToTooltip()
    {
        return _data;
    }
}
