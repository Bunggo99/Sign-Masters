using System;
using UnityEngine;

public abstract class EventWithParam<T> : ScriptableObject
{
    #region Variables
    
    private event Action<T> GameEvent = null;

    #endregion

    #region Functions

    public void AddListener(Action<T> action)
    {
        GameEvent += action;
    }

    public void RemoveListener(Action<T> action)
    {
        GameEvent -= action;
    }

    public bool Invoke(T param)
    {
        GameEvent?.Invoke(param);

        return GameEvent != null;
    }

    public void ClearListeners()
    {
        GameEvent = null;
    } 

    #endregion
}

public abstract class EventWithParam<T1, T2> : ScriptableObject
{
    #region Variables
    
    private event Action<T1, T2> GameEvent = null;

    #endregion
    
    #region Functions

    public void AddListener(Action<T1, T2> action)
    {
        GameEvent += action;
    }

    public void RemoveListener(Action<T1, T2> action)
    {
        GameEvent -= action;
    }

    public bool Invoke(T1 param1, T2 param2)
    {
        GameEvent?.Invoke(param1, param2);

        return GameEvent != null;
    }

    public void ClearListeners()
    {
        GameEvent = null;
    } 

    #endregion
}