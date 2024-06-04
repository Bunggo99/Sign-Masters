using System.Collections;
using TMPro;
using UnityEngine;

public abstract class LevelManager : SingletonManager<LevelManager>
{
    #region Variables

    [SerializeField] private float levelStartDelay = 1f;
    [SerializeField] protected float nextLevelDelay = 1f;
    [SerializeField] protected float gameOverDelay = 1.5f;

    [SerializeField] private GameObject levelTransitionPanel = null;
    [SerializeField] protected TextMeshProUGUI levelText = null;
    [SerializeField] protected TextMeshProUGUI stageText = null;

    [SerializeField] private GameObject pauseScreen = null;
    [SerializeField] protected GameObject loseScreen = null;
    [SerializeField] protected TextMeshProUGUI loseText = null;
    [SerializeField] private GameObject dictionaryPopup = null;

    [SerializeField] protected EventNoParam OnGameOver;
    [SerializeField] private EventNoParam OnLevelSetupStarted;
    [SerializeField] private EventNoParam OnLevelSetupEnded;
    [SerializeField] private EventNoParam OnLevelResumed;
    [SerializeField] private EventNoParam OnLevelPaused;
    [SerializeField] protected EventNoParam OnGameEnded;
    [SerializeField] protected EventNoParam OnPopupDisabled;

    [SerializeField] protected LevelInfo levelInfo;

    protected bool gameEnded = false;

    #endregion

    #region Awake

    protected override void Awake()
    {
        base.Awake();

        levelInfo.LoadStageNumber();
    }

    #endregion

    #region Update

    protected virtual void Update()
    {
        //ESC Buttons

        //dictionary back btn
        if (Input.GetKeyDown(KeyCode.Escape) && dictionaryPopup.activeInHierarchy)
        {
            dictionaryPopup.SetActive(false);
            OnPopupDisabled.Invoke();
        }
        //Resume
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseScreen.activeInHierarchy)
        {
            Resume();
        }
        //Pause
        else if (Input.GetKeyDown(KeyCode.Escape) && !levelTransitionPanel.activeSelf && !gameEnded)
        {
            pauseScreen.SetActive(true);
            OnLevelPaused.Invoke();
            Time.timeScale = 0;
        }
    }

    public void Resume()
    {
        pauseScreen.SetActive(false);
        OnLevelResumed.Invoke();
        Time.timeScale = 1;
    }

    #endregion

    #region Enable/Disable

    protected virtual void OnEnable()
    {
        OnGameOver.AddListener(GameOver);
    }

    protected virtual void OnDisable()
    {
        levelInfo.ResetStageNumber();

        OnGameOver.RemoveListener(GameOver);
    }

    #endregion

    #region Setup Level

    protected virtual void Start()
    {
        Time.timeScale = 1;
        SoundManager.instance.StartMusic();

        SetupStageTexts();
        StartCoroutine(LevelTransition());
    }

    protected virtual void SetupStageTexts()
    {
        levelText.text = "Day " + levelInfo.StageNumber;
        stageText.text = "Day: " + levelInfo.StageNumber;
    }

    private IEnumerator LevelTransition()
    {
        levelTransitionPanel.SetActive(true);
        yield return new WaitForEndOfFrame();
        OnLevelSetupStarted.Invoke();

        yield return new WaitForSeconds(levelStartDelay);
        levelTransitionPanel.SetActive(false);

        OnLevelSetupEnded.Invoke();
    }

    #endregion

    #region Game Over

    protected abstract void GameOver();

    #endregion
}