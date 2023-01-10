using UnityEngine;

public class SignLanguageDisplayNormal : SignLanguageDisplay
{
    #region Variables

    [SerializeField] private int wordLength = 4;
    [SerializeField] private LevelInfo levelInfo;

    #endregion

    #region Functions

    protected override string GetRandomWord()
    {
        int currChar = 'A' + (levelInfo.StageNumber - 1);
        return WordList.RandomizeLetters(wordLength, (char)currChar);
    }

    #endregion
}
