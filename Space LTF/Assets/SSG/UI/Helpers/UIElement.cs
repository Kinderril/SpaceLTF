using System;
using System.Collections.Generic;
using UnityEngine;

public class UIElement : PoolElement, IDisposable
{
    // ReSharper disable once InconsistentNaming
    protected readonly UIParent UI = new UIParent();

    protected virtual void CorrectPosition()
    {
        transform.CorrectPositionResolution();
    }

    public void Dispose()
    {
        Close();
    }

    public virtual void Display()
    {
        ShowGameObject();
    }

    public virtual void Close()
    {
        UI.Dispose();

        HideGameObject();
    }

    public void ShowGameObject()
    {
        gameObject.SetActive(true);
    }

    public void HideGameObject()
    {
        gameObject.SetActive(false);
    }
}

// ReSharper disable once InconsistentNaming
public static class UIParentExtensions
{
    public static void AddTo(this Action disposable, UIParent parent)
    {
        parent.AddDisposable(disposable);
    }
}

// ReSharper disable once InconsistentNaming
public class UIParent : IDisposable
{
    private readonly List<Action> _destroys = new List<Action>();

    public T AddDisposable<T>(T disposable) where T : IDisposable
    {
        _destroys.Add(disposable.Dispose);
        return disposable;
    }

    public void AddDisposable(Action destroy)
    {
        _destroys.Add(destroy);
    }



    public void AddSubscription(Action<Action> subscribe, Action<Action> unsubscribe, Action handler)
    {
        subscribe(handler);
        AddDisposable(() => unsubscribe(handler));

        handler();
    }



    public void Dispose()
    {
        foreach (var destroy in _destroys)
            destroy();

        _destroys.Clear();
    }
}