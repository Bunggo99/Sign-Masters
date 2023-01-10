using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalRetryButton : MonoBehaviour
{
    #region Variables

    [SerializeField] private LevelInfo levelInfo;

    #endregion

    #region Functions

    public void RetryLevel()
    {
        LevelInfo.SetStageAndSave(levelInfo.StageNumber);
        SceneManager.LoadScene(SceneName.NormalMode);
    }

    #endregion
}
