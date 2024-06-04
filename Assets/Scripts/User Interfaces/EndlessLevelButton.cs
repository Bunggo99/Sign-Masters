using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndlessLevelButton : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private Button button;
    [SerializeField] private int unlockedAtStage;

    #endregion

    #region Start
    
    private void Start()
    {
        if (unlockedAtStage > PlayerPrefs.GetInt(MainMenu.UNLOCKED_STAGES_KEY))
            button.interactable = false;
    }

    #endregion

    #region Functions

    public void LoadLevel()
    {
        LevelInfo.ResetStageAndSave();
        SceneManager.LoadScene(SceneName.EndlessMode);
    }

    #endregion
}
