using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour, IPointerEnterHandler
    , IPointerExitHandler
{
//    private int _index;
    public TextMeshProUGUI DataFiled;
    public TMP_InputField InputFiled;
    private string _haveData = null;
    private bool _isOpen;
    private bool _haveName;

    private bool _inited = false;
    private Action<SaveSlot> OnClickCallback;
    private Action<SaveSlot> OnSaveCallback;
    public void Init(string name, Action<SaveSlot> OnClick, Action<SaveSlot> OnSave)
    {
        OnClickCallback = OnClick;
        OnSaveCallback = OnSave;
        _inited = true;
        _haveName = true;
        _haveData = name;
        InputFiled.text = DataFiled.text = name;
        InputFiled.gameObject.SetActive(false);
        DataFiled.gameObject.SetActive(true);
    }

    public void Init( Action<SaveSlot> OnClick, Action<SaveSlot> OnSave)
    {
        OnClickCallback = OnClick;
        OnSaveCallback = OnSave;
        _inited = true;
        _haveName = false;
        InputFiled.gameObject.SetActive(true);
        DataFiled.gameObject.SetActive(false);
    }

//    public void SetSaveInfo(string name)
//    {
//        DataFiled.text = name;
//        InputFiled.text = name;
//       _haveData = true;
//    }

    public void OnSlotClick()
    {
        if (_haveName)
        {
            OnClickCallback(this);
            _isOpen = true;
            InputFiled.gameObject.SetActive(true);
            DataFiled.gameObject.SetActive(false);
        }
    }

    public void OnSaveClick()
    {
#if UNITY_EDITOR
        if (InputFiled == null)
        {
            Debug.LogError($"WFT:{gameObject.name}   _inited:{_inited}");
        }
#endif

        if (InputFiled.text.Length == 0)
        {
            WindowManager.Instance.InfoWindow.Init(OnOkNull,  Namings.Tag("WrongName"));
            return;
        }
        if (_haveData != null)
        {
            WindowManager.Instance.ConfirmWindow.Init(OnOk,OnReject,Namings.Tag("SureSaveSlot"));
        }
        else
        {
            OnOk();
        }
    }

    private void OnOkNull()
    {
        
    }

    private void OnReject()
    {
        Exit();
    }

    private void OnOk()
    {
        if (InputFiled.text.Length == 0)
        {
            WindowManager.Instance.InfoWindow.Init(OnOkNull,  Namings.Tag("WrongName"));
            return;
        }
        if (_haveData != null)
        {
            MainController.Instance.Campaing.DeleteSave(_haveData);
        }

        if (!MainController.Instance.Campaing.SaveGame(InputFiled.text,false))
        {
            ErrorSaveGame();
        }
        else
        {
            OnSaveCallback(this);
        }
    }

    private void ErrorSaveGame()
    {

        WindowManager.Instance.InfoWindow.Init(OnOkNull,  Namings.Tag("SaveError"));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
//        if (_isOpen)
//        {
//            Exit();
//        }
    }

    public void OnClickClose()
    {
        Exit();
    }

    private void Exit()
    {

        _isOpen = false;
        if (_haveName)
        {
            InputFiled.gameObject.SetActive(false);
            DataFiled.gameObject.SetActive(true);
        }
    }
}
