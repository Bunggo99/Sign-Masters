using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalNextButton : MonoBehaviour
{
    #region Variables

    [SerializeField] private LevelInfo levelInfo;

    #endregion

    #region Functions

    public void NextLevel()
    {
        LevelInfo.SetStageAndSave(levelInfo.StageNumber+1);
        SceneManager.LoadScene(SceneName.NormalMode);
    }

    #endregion
}
