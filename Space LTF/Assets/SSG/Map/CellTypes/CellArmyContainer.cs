using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class CellArmyContainer
{
    private HashSet<MovingArmy> _curArmies = new HashSet<MovingArmy>();
    [field: NonSerialized]
    public event Action<bool> OnFightWas;
    [field: NonSerialized]
    public event Action OnNoFight;

    public void ArmyCome(MovingArmy army)
    {
        _curArmies.Add(army);
    }

    public bool CanArmyMove(MovingArmy army)
    {
        if (army.IsAllies)
        {
            var haveAllies = Allies() != null;
            if (haveAllies)
            {
                return false;
            }
        }
        else
        {
            var haveEnemies = Enemies() != null;
            if (haveEnemies)
            {
                return false;
            }

        }

        return true;
    }

    public void CheckIfHaveFight()
    {
        if (_curArmies.Count < 1)
        {
            OnNoFight?.Invoke();
            return;
        } 

        float sumAllies = 0;
        float sumEnemies = 0;
        foreach (var movingArmy in _curArmies)
        {
            var sum = movingArmy.Power;
            if (!movingArmy.Destroyed && movingArmy.IsAllies)
            {
                sumAllies += sum;
            }
            else
            {
                sumEnemies += sum;
            }
        }

        if (sumAllies > 1 && sumEnemies > 1)
        {
           var wDd = new WDictionary<bool>(new Dictionary<bool, float>()
           {
               {true,sumAllies },
               {false,sumEnemies },
           });
           var result = wDd.Random();
           var player = MainController.Instance.MainPlayer.MapData.GalaxyData.GalaxyEnemiesArmyController;
           var enemies = _curArmies.Where(x => !x.Destroyed && result != x.IsAllies).ToList();
           foreach (var movingArmy in enemies)
           {
               player.DestroyArmy(movingArmy);
           }

           foreach (var movingArmy in _curArmies)
           {
               if (!movingArmy.Destroyed)
               {
                   movingArmy.AddWin(sumEnemies);
               }
           }

           OnFightWas?.Invoke(result);
        }
        else
        {
            OnNoFight?.Invoke();
        }
    }

    public void ArmyRemove(MovingArmy army)
    {
        _curArmies.Remove(army);
    }

    public MessageDialogData GetDialog(Action fightMovingArmy, MessageDialogData getDialog)
    {

        var enemies = Enemies();
        if (enemies != null)
        {
            return enemies.GetDialog(fightMovingArmy, getDialog);
        }   
        var allise = Allies();
        if (allise != null)
        {
            return allise.GetDialog(fightMovingArmy, getDialog);
        }

        return null;

    }

    public MessageDialogData MoverArmyLeaverEnd()
    {

        var enemies = Enemies();
        if (enemies != null)
        {
            return enemies.MoverArmyLeaverEnd();
        }
        var allise = Allies();
        if (allise != null)
        {
            return allise.MoverArmyLeaverEnd();
        }

        return null;
    }

    public Player GetArmyToFight()
    {

        var enemies = Enemies();
        if (enemies != null)
        {
            return enemies.GetArmyToFight();
        }

        var allise = Allies();
        
        Debug.LogError($" GetArmyToFight is null   allise:{allise != null}");
        return null;

    }

    public bool HaveArmy()
    {
        return _curArmies.Count != 0;

    }

    public bool NoAmry()
    {

        return _curArmies.Count == 0;

    }

    private MovingArmy Allies()
    {
        return _curArmies.FirstOrDefault(x => !x.Destroyed && x.IsAllies);
    }      
    private MovingArmy Enemies()
    {
        return _curArmies.FirstOrDefault(x => !x.Destroyed && !x.IsAllies);
    }

    public string ShortDesc()
    {
        var enemies = Enemies();
        if (enemies != null)
        {
            return enemies.ShortDesc();
        }
        var allise = Allies();
        if (allise != null)
        {
            return allise.ShortDesc();
        }

        return null;
    }

    public IEnumerable<MovingArmy> GetAllArmies()
    {
        return _curArmies;
    }

    public bool HaveAlliesAmry()
    {
        return Allies() != null;
    }

    public bool HaveEnemiesAmry()
    {
        return Enemies() != null;
    }
}

