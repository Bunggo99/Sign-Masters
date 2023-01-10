using UnityEngine;

public class PersistenceSpawner : SingletonManager<PersistenceSpawner>
{
    #region Variables

    [SerializeField] private GameObject soundManager;

    #endregion

    #region Awake

    protected override void Awake()
    {
        base.Awake();

        if (SoundManager.instance == null)
            Instantiate(soundManager);
    }

    #endregion
}
