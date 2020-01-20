using System;
using System.Threading.Tasks;
using UnityEngine;


public class AsynTaskClassA : MonoBehaviour
{
    private Action _callback;
    private float EndPeriod;
    private bool _isActive = false;
    public void Do(Action callback)
    {

        _callback = callback;
        EndPeriod = Time.time + 3f;
        _isActive = true;
    }

    public async Task DoAsTask()
    {

        EndPeriod = Time.time + 3f;
        _isActive = true;
        await Task.Yield();
    }

    void Update()
    {
        if (!_isActive)
        {
            return;
        }
        if (EndPeriod < Time.time)
        {
            _callback();
            _isActive = false;
        }
    }


}

