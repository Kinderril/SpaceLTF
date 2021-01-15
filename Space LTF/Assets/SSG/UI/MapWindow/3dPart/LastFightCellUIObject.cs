using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public  class LastFightCellUIObject : MonoBehaviour
{
    public Animator EnemiesAnim;
    public Animator AlliesAnim;

    public void DoAlliesWin()
    {
        EnemiesAnim.gameObject.SetActive(false);
        AlliesAnim.gameObject.SetActive(true);
        AlliesAnim.SetTrigger("Play");
    }   
        public void DoEnemiesWin()
    {
        AlliesAnim.gameObject.SetActive(false);
        EnemiesAnim.gameObject.SetActive(true);
        EnemiesAnim.SetTrigger("Play");

    }

        public void Disable()
    {
        AlliesAnim.gameObject.SetActive(false);
        EnemiesAnim.gameObject.SetActive(false);
    }
}

