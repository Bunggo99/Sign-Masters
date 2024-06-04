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

    #endregion

    #region Functions

    private void OnEnable()
    {
        OnPhotoTaken.AddListener(PhotoTaken);

        comvisText.text = $"Demonstrate the letter {GetRandomChar()} to kill enemy";
    }

    private void OnDisable()
    {
        OnPhotoTaken.RemoveListener(PhotoTaken);
    }

    private char GetRandomChar()
    {
        if (!levelInfo.isEndless)
        {
            int currChar = 'A' + (levelInfo.StageNumber - 1);
            return WordList.RandomizeCharacter((char)currChar);
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
