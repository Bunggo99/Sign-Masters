using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerEndless : LevelManager
{
    #region Variables

    [SerializeField] private EventNoParam OnLevelClearEndless;

    #endregion

    #region Enable/Disable

    protected override void OnEnable()
    {
        base.OnEnable();

        OnLevelClearEndless.AddListener(NextLevel);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnLevelClearEndless.RemoveListener(NextLevel);
    }

    #endregion

    #region Next Level

    [ContextMenu("Endless Next Level")]
    private void NextLevel()
    {
        gameEnded = true;
        OnGameEnded.Invoke();

        LevelInfo.SetStageAndSave(levelInfo.StageNumber + 1);

        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(nextLevelDelay);
        SceneManager.LoadScene(SceneName.EndlessMode);
    }

    #endregion

    #region Game Over

    protected override void GameOver()
    {
        gameEnded = true;
        OnGameEnded.Invoke();

        StartCoroutine(EnableGameOverScreen());
    }

    private IEnumerator EnableGameOverScreen()
    {
        yield return new WaitForSeconds(gameOverDelay);

        //high score
        int prevHighScore = PlayerPrefs.GetInt(PlayerPrefsKey.EndlessHighScore);
        if (levelInfo.StageNumber > prevHighScore)
            PlayerPrefs.SetInt(PlayerPrefsKey.EndlessHighScore, levelInfo.StageNumber);

        //lose UI
        loseText.text = "After " + levelInfo.StageNumber + " days,\nyou starved.";
        loseScreen.SetActive(true);
    }

    #endregion
}

//coop, versus, rush, multi rush, dual rush