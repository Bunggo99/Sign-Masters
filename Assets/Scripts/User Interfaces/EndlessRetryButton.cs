using UnityEngine;
using UnityEngine.SceneManagement;

public class EndlessRetryButton : MonoBehaviour
{
    #region Functions

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RetryEndless();
            enabled = false;
        }
    }

    public void RetryEndless()
    {
        LevelInfo.ResetStageAndSave();
        SceneManager.LoadScene(SceneName.EndlessMode);
    }

    #endregion
}
