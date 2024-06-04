using TMPro;
using UnityEngine;

public class SignLanguageDisplay : MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI signText = null;
    [SerializeField] private TextMeshProUGUI arrowText = null;
    [SerializeField] private string defaultColor = "FFFFFF";
    [SerializeField] private string highlightedColor = "00FF00";

    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI wordText = null;

    #endregion

    #region Awake

    private void Awake()
    {
        Alphabet.SetRandomWordThisStage(GetRandomWord());
        Alphabet.ResetCorrectLetterCount();
        UpdateColoredWordDisplay();
        arrowText.text = "<sprite name=\"Arrow\">";
    }

    protected virtual string GetRandomWord()
    {
        return WordList.RandomizeWord();
    }

    #endregion

    #region Update Random Word Display And Arrow

    public void UpdateColoredWordDisplay()
    {
        string word = Alphabet.RandomWordThisStage;
        int correctNum = Alphabet.CorrectLetterCount;
        string coloredWord = "<color=\"green\">" + word.Substring(0, correctNum)
                            + "</color>" + word[correctNum..];
        wordText.text = coloredWord;

//#if !UNITY_EDITOR
        //wordText.gameObject.SetActive(false);
//#endif

        string coloredSignText = string.Empty;
        for (int i = 0; i < word.Length; i++)
        {
            char ch = char.ToUpper(word[i]);
            string letter = $"<sprite name=\"{ch}\"";
            if (i < correctNum)
                letter += $" color=#{highlightedColor}>";
            else
                letter += $" color=#{defaultColor}>";

            coloredSignText += letter;
        }

        signText.text = coloredSignText;
    }

    public void ShiftArrow(bool AllLettersCorrect)
    {
        if (!AllLettersCorrect)
            arrowText.text = " " + arrowText.text;
        else
            arrowText.gameObject.SetActive(false);
    }

    #endregion
}
