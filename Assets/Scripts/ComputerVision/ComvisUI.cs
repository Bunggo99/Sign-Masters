using System;
using TMPro;
using UnityEngine;

public class ComvisUI : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject comvisPanel;
    [SerializeField] private TMP_Text comvisText;
    [SerializeField] private LevelInfo levelInfo;
    [SerializeField] private EventBool OnPhotoTaken;

    public static char charQuestion;

    #endregion

    #region Functions

    private void OnEnable()
    {
        OnPhotoTaken.AddListener(PhotoTaken);

        charQuestion = GetCharQuestion();

        comvisText.text = $"Demonstrate the letter {charQuestion} to kill enemy";
    }

    private void OnDisable()
    {
        OnPhotoTaken.RemoveListener(PhotoTaken);
    }

    private char GetCharQuestion()
    {
        if (!levelInfo.isEndless)
        {
            int currChar = 'A' + (levelInfo.StageNumber - 1);
            return (char)currChar;
        }
        else
        {
            int currChar = 'A' + (26 - 1);
            return WordList.RandomizeCharacter((char)currChar);
        }
    }

    private void PhotoTaken(bool success)
    {
        comvisPanel.SetActive(false);
    }

    #endregion
}
