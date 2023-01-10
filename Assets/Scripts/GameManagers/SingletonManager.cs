using UnityEngine;

public class SingletonManager<T> : MonoBehaviour where T: SingletonManager<T>
{
    #region Variables
    
    private static T _instance;

    #endregion

    #region Awake
    
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
