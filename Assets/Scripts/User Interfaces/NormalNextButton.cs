using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalNextButton : MonoBehaviour
{
    #region Variables

    [SerializeField] private LevelInfo levelInfo;

    #endregion

    #region Functions

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.Return)
            || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
        {
            NextLevel();
            enabled = false;
        }
    }

    public void NextLevel()
    {
        LevelInfo.SetStageAndSave(levelInfo.StageNumber+1);
        SceneManager.LoadScene(SceneName.NormalMode);
    }

    #endregion
}
