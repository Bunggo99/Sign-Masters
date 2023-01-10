using UnityEngine;

public class NormalLayoutSpawner : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private LevelLayout[] layouts;
    [SerializeField] private LevelInfo levelInfo;
    [SerializeField] private EventNoParam OnLayoutSpawned;

    #endregion

    #region Start

    private void Start()
    {
        int stage = levelInfo.StageNumber - 1;
        stage = Mathf.Min(stage, layouts.Length - 1);
        Instantiate(layouts[stage], transform);
        OnLayoutSpawned.Invoke();
    }

    #endregion
}
