using System.Collections;
using UnityEditor;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    #region Variables

    public const string UNLOCKED_STAGES_KEY = "UnlockedStages";

    [SerializeField] private GameObject gameModeSelection;
    [SerializeField] private GameObject levelSelection;
    [SerializeField] private GameObject exitPopup;
    [SerializeField] private GameObject dictionaryPopup;

    private bool exitOnce = false;

    #endregion

    #region Start

    private void Start()
    {
        if (!PlayerPrefs.HasKey(UNLOCKED_STAGES_KEY))
            PlayerPrefs.SetInt(UNLOCKED_STAGES_KEY, 1);

        PlayerPrefs.SetInt(UNLOCKED_STAGES_KEY, 26);

        Time.timeScale = 1;
        SoundManager.instance.StartMusic();
    }

    #endregion

    #region Update

    private void Update()
    {
        //ESC Buttons

        //dictionary back btn
        if (Input.GetKeyDown(KeyCode.Escape) && dictionaryPopup.activeInHierarchy)
        {
            dictionaryPopup.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && levelSelection.activeSelf)
        {
            levelSelection.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && gameModeSelection.activeSelf)
        {
            gameModeSelection.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!exitOnce) StartCoroutine(EnableExitPopup());
            else Exit();
        }
    }

    #endregion

    #region Button Functions

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator EnableExitPopup()
    {
        exitOnce = true;
        exitPopup.SetActive(true);

        float popupDuration = exitPopup.GetComponent<Animation>().clip.length;
        yield return new WaitForSeconds(popupDuration);

        exitPopup.SetActive(false);
        exitOnce = false;
    }

    #endregion

    #region Debug

    [ContextMenu("ResetLevelProgress")]
    private void ResetLevelProgress()
    {
        PlayerPrefs.DeleteKey(UNLOCKED_STAGES_KEY);
    }

    [ContextMenu("NearlyUnlockAllLevels")]
    private void NearlyUnlockAllLevels()
    {
        PlayerPrefs.SetInt(UNLOCKED_STAGES_KEY, 25);
    }

    [ContextMenu("UnlockAllLevels")]
    private void UnlockAllLevels()
    {
        PlayerPrefs.SetInt(UNLOCKED_STAGES_KEY, 26);
    }

    [ContextMenu("UnlockEndless")]
    private void UnlockEndless()
    {
        PlayerPrefs.SetInt(UNLOCKED_STAGES_KEY, 27);
    }

    #endregion
}
