using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/No Param")]
public class EventNoParam : ScriptableObject
{
    #region Variables

    private event Action GameEvent = null;

    #endregion

    #region Functions

    public void AddListener(Action action)
    {
        GameEvent += action;
    }

    public void RemoveListener(Action action)
    {
        GameEvent -= action;
    }

    public void Invoke()
    {
        GameEvent?.Invoke();
    }

    public void ClearListeners()
    {
        GameEvent = null;
    }

    internal void AddListener(EventNoParam onComputerVisionSceneLoaded)
    {
        throw new NotImplementedException();
    }

    #endregion
}
