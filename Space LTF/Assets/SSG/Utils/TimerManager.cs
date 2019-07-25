using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

public class TimerManager
{
    public interface ITimer
    {
        event Action OnTimer;

        float Duration { get; }
        float StartTime { get; }
        float EndTime { get; }
        float TimeLeft { get; }
        bool IsLoopped { get; }
        bool IsActive { get; }

        void Stop(bool shallFire = false);
        void Restart(float endTime);
    }

    private class Timer : ITimer
    {
        private Action onStop;
        private Action onStart;

        public event Action OnTimer;

        public float EndTime { get; private set; }
        public float Duration { get; private set; }
        public float TimeLeft
        {
            get
            {
                return EndTime - Time.time;
            }
        }

        public float StartTime
        {
            get
            {
                return EndTime - Duration;
            }
        }
        public bool IsLoopped { get; private set; }
        public bool IsActive { get; private set; }
        public void Init(Action onStop, Action onStart)
        {
            this.onStop = onStop;
            this.onStart = onStart;
        }

        public void Start(float endTime, float duration, bool isLopped = false)
        {
            EndTime = endTime;
            Duration = duration;
            IsLoopped = isLopped;
            onStart();
            IsActive = true;
        }

        public void Stop(bool shallFire = false)
        {
//            Assert.IsTrue(IsActive, "Trying to stop inactive timer.");
            onStop();
            IsActive = false;

            if (shallFire)
            {
                Fire();
            }
        }

        public void Fire()
        {
            try
            {
                if (OnTimer != null)
                {
                    OnTimer();
                }
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                if (IsActive)
                {
                    Stop();
                }
            }
        }

        public void Restart(float endTime)
        {
            onStop();
            EndTime = endTime;
            onStart();
        }
    }

    private LinkedList<Timer> timers = new LinkedList<Timer>();

    public ITimer MakeTimer(float endTime, float duration, bool isLopped = false)
    {
        Timer timer = new Timer();
        timer.Init(() =>
        {
            timers.Remove(timer);
        }, () =>
        {
            AddTimer(timer);
        });
        timer.Start(endTime, duration, isLopped);
        return timer;
    }

    public ITimer MakeTimer(float durationSec, bool isLopped = false)
    {
        return MakeTimer(Time.time + durationSec, durationSec, isLopped);
    }

//    public ITimer MakeTimer(DateTime endTime)
//    {
//        return MakeTimer(endTime, endTime - DateTime.Now);
//    }

    public void Update()
    {
        var node = timers.First;
        while (node != null)
        {
            var next = node.Next;
            if (Time.time > node.Value.EndTime)
            {
                var timer = node.Value;
                if (timer.IsLoopped)
                {
                    timer.Restart(timer.EndTime + timer.Duration);
                }
                else
                {
                    timer.Stop();
                }

                timer.Fire();
            }
            else
            {
                break;
            }
            node = next;
        }
    }

    private void AddTimer(Timer timer)
    {
        var node = timers.First;
        if (node == null)
        {
            timers.AddFirst(timer);
        }
        else
        {
            while (node != null)
            {
                var next = node.Next;
                if (node.Value.EndTime > timer.EndTime)
                {
                    timers.AddBefore(node, timer);
                    return;
                }
                node = next;
            }
            timers.AddLast(timer);
        }
    }
}