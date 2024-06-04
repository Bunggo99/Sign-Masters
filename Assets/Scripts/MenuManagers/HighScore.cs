using TMPro;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI text;
    
    [TextArea]
    [SerializeField] private string highScorePrefix;

    [TextArea]
    [SerializeField] private string highScorePostfix;

    #endregion

    #region Start

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt(PlayerPrefsKey.EndlessHighScore);
        text.text = highScorePrefix + highScore.ToString() + highScorePostfix;
    }

    #endregion
}
