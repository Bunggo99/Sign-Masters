using UnityEngine;
using UnityEngine.SceneManagement;

public class EndlessRetryButton : MonoBehaviour
{
    #region Functions
    
    public void RetryEndless()
    {
        LevelInfo.ResetStageAndSave();
        SceneManager.LoadScene(SceneName.EndlessMode);
    }

    #endregion
}
