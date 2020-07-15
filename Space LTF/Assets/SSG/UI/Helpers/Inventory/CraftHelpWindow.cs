using UnityEngine;
using System.Collections;
using TMPro;

public class CraftHelpWindow : MonoBehaviour
{

    public Transform Layout;
    public TextMeshProUGUI First;

    void Awake()
    {
        var crLib = CraftLibrary.GetAllPatternsTag();
        foreach (var tag in crLib)
        {
            var obj = DataBaseController.GetItem(First);
            obj.transform.SetParent(Layout);
            obj.text = Namings.Tag(tag);
        }
        First.gameObject.SetActive(false);
    }

    public void Open()
    {

        gameObject.SetActive(true);

    }     
    public void Close()
    {
        
          gameObject.SetActive(false);
    }
}
