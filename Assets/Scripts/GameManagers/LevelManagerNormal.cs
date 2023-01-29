using System.Collections;
using UnityEngine;

public class LevelManagerNormal : LevelManager
{
    #region Variables

    [SerializeField] private GameObject winScreen = null;
    [SerializeField] private GameObject nextLevelButton = null;
    [SerializeField] private LevelInfo stageInfo = null;
    [SerializeField] private GameObject tutorialPopup = null;
    [SerializeField] private GameObject highScoreText = null;

    [SerializeField] private EventNoParam OnWinGame;

    #endregion

    #region Start

    protected override void Start()
    {
        if (winScreen.activeInHierarchy)
            winScreen.SetActive(false);

        base.Start();
    }

    #endregion

    #region Update

    protected override void Update()
    {
        //ESC Buttons

        //tutorial back btn
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return) 
            || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space)) 
            && tutorialPopup.activeInHierarchy)
        {
            DisableTutorialPopup();
            return;
        }

        base.Update();
    }

    public void DisableTutorialPopup()
    {
        tutorialPopup.SetActive(false);
        OnPopupDisabled.Invoke();
    }

    #endregion

    #region Enable/Disable

    protected override void OnEnable()
    {
        base.OnEnable();

        OnWinGame.AddListener(WinGame);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnWinGame.RemoveListener(WinGame);
    }

    #endregion

    #region Setup

    protected override void SetupStageTexts()
    {
        levelText.text = "Stage " + levelInfo.StageNumber;
        stageText.text = "Stage: " + levelInfo.StageNumber;
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

        highScoreText.SetActive(false);
        loseText.text = "You lost the stage.";
        loseScreen.SetActive(true);
    }

    #endregion

    #region Win Game

    [ContextMenu("Normal Win Game")]
    private void WinGame()
    {
        gameEnded = true;
        OnGameEnded.Invoke();

        if(PlayerPrefs.GetInt(MainMenu.UNLOCKED_STAGES_KEY) <= levelInfo.StageNumber + 1)
            PlayerPrefs.SetInt(MainMenu.UNLOCKED_STAGES_KEY, levelInfo.StageNumber + 1);

        StartCoroutine(EnableWinScreen());
    }

    private IEnumerator EnableWinScreen()
    {
        yield return new WaitForSeconds(nextLevelDelay);
        winScreen.SetActive(true);

        if (stageInfo.StageNumber == stageInfo.MaxStageNumber)
            nextLevelButton.SetActive(false);
    }

    #endregion
}
