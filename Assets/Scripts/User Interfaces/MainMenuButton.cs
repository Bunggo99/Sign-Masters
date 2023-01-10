using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    #region Function

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(SceneName.MainMenu);
    }

    #endregion
}
