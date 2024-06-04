using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalRetryButton : MonoBehaviour
{
    #region Variables

    [SerializeField] private LevelInfo levelInfo;

    #endregion

    #region Functions

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RetryLevel();
            enabled = false;
        }
    }

    public void RetryLevel()
    {
        LevelInfo.SetStageAndSave(levelInfo.StageNumber);
        SceneManager.LoadScene(SceneName.NormalMode);
    }

    #endregion
}
