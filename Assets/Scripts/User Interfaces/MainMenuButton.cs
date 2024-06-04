using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    #region Function

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMainMenu();
            enabled = false;
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(SceneName.MainMenu);
    }

    #endregion
}
