using UnityEngine;
using System.Collections;

public class WindowExprolerGlobalMap : BaseWindow
{
    public ExprolerGlobalMap GlobalMap;
    public ExprolerLastBattleInfo LastBattleInfo;
    public MoneySlotUI CreditsField;
    public MoneySlotUI MicrochipsField;
    private PlayerSafe _player;

    public override void Init()
    {
        var exproler = MainController.Instance.Exproler;
        _player = exproler.CurrentPlayer;

        if (!exproler.PlayInfo.Showed)
        {
            exproler.PlayInfo.Showed = true;
            LastBattleInfo.Init(exproler.PlayInfo);
        }
        else
        {
            LastBattleInfo.OnClose();
        }
        GlobalMap.Init(_player);
        _player.OnCreditsChange += OnCreditsChange;
        _player.OnMicroChipsChange+= OnMicroChipsChange;
        UpdateFields();
        base.Init();
    }

    private void OnMicroChipsChange(int obj)
    {
        UpdateFields();
    }

    private void OnCreditsChange(int obj)
    {
        UpdateFields();
    }

    private void UpdateFields()
    {
        CreditsField.Field.Init(_player.Credits);
        MicrochipsField.Field.Init(_player.Microchips);
    }

    public override void Close()
    {
        UnSubscribe();
        base.Close();
    }
    private void UnSubscribe()
    {

        _player.OnCreditsChange -= OnCreditsChange;
        _player.OnMicroChipsChange -= OnMicroChipsChange;
    }

    public void OnInventory()
    {
             Debug.LogError("not ready");
    }   
    public void OnSettings()
    {
          WindowManager.Instance.OpenSettingsSettings(EWindowSettingsLauch.exprolerGlobalMap);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
