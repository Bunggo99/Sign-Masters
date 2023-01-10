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

    public bool Invoke()
    {
        GameEvent?.Invoke();

        return GameEvent != null;
    }

    public void ClearListeners()
    {
        GameEvent = null;
    }

    #endregion
}
