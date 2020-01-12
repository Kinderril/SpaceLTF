using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class DemoVersionObject : MonoBehaviour
{

    private float _nextCheckTime;
    private bool _isEndGame;
    void Update()
    {
        if (_isEndGame)
        {

        }
        else
        {
            if (_nextCheckTime < Time.time)
            {
                _nextCheckTime = Time.time + 10f;
                var player = MainController.Instance.MainPlayer;
                if (player == null)
                {
                    return;
                }

                if (player.MapData == null)
                {
                    return;
                }

                if (player.MapData.VisitedSectors > 2)
                {
                    EndGame();
                }
            }
        }
    }

    private void EndGame()
    {
        _isEndGame = true;

    }
}

