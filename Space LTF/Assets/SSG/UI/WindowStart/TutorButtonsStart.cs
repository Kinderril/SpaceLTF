using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TutorButtonsStart : MonoBehaviour
{
    public GameObject _causeTransform;
    public void Init()
    {
        gameObject.SetActive(true);
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
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void StartSimpleTutor()
    {

        WindowManager.Instance.ConfirmWindow.Init(() =>
        {
            StartTutor(EGameMode.simpleTutor);
        }, null, Namings.Tag("wantStartTutor"));
    }   
    public void StartAdvTutor()
    {
        WindowManager.Instance.ConfirmWindow.Init(() =>
        {
            StartTutor(EGameMode.advTutor);
        }, null, Namings.Tag("wantStartAdvTutor"));
    }

    void StartTutor(EGameMode mode)
    {
        var gameData = UpdateStartData(mode);
        MainController.Instance.CreateNewPlayerAndStartGame(gameData);
    }

    private StartNewGameData UpdateStartData(EGameMode eGameMode)
    {
        var posibleStartSpells = new List<SpellType>()
        {
            SpellType.lineShot,
            SpellType.engineLock,
            SpellType.shildDamage,
            SpellType.mineField,
            SpellType.throwAround,
            SpellType.distShot,
            SpellType.artilleryPeriod,
            SpellType.repairDrones,
            SpellType.vacuum,
            SpellType.hookShot,
//            SpellType.spaceWall,
        };
        List<WeaponType> posibleStartWeapons = new List<WeaponType>();
        var posibleSpells = posibleStartSpells.RandomElement(2);
#if UNITY_EDITOR
        //        posibleSpells.Add(SpellType.repairDrones);
#endif
        posibleSpells.Add(SpellType.rechargeShield);
        // posibleSpells.Add(SpellType.roundWave);
        // gameData = new StartNewGameData(PlayerStartParametersUI.GetCurrentLevels(),
        var gameData = new StartNewGameData(new Dictionary<PlayerParameterType, int>(),
            ShipConfig.mercenary, posibleStartWeapons, 4, 1, 
            2, 0, EStartGameDifficulty.Normal, posibleSpells, 0, eGameMode);
        var dif = Utils.FloatToChance(gameData.CalcDifficulty());
        return gameData;
    }
}
