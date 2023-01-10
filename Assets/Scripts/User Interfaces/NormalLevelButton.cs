using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NormalLevelButton : MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Button button;

    private int stageNum;

    #endregion

    #region Start

    private void Start()
    {
        var index = transform.GetSiblingIndex() + 1;
        buttonText.text = index.ToString();
        stageNum = index;

        if (stageNum > PlayerPrefs.GetInt(MainMenu.UNLOCKED_STAGES_KEY))
            button.interactable = false;
    }

    #endregion

    #region Functions

    public void LoadLevel()
    {
        LevelInfo.SetStageAndSave(stageNum);
        SceneManager.LoadScene(SceneName.NormalMode);
    }

    #endregion
}
