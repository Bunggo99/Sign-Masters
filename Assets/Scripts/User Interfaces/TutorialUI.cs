using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI alphabetText;
    [SerializeField] private TextMeshProUGUI signText;
    [SerializeField] protected LevelInfo levelInfo;

    private void OnEnable()
    {
        UpdateTutorialContent();
    }

    protected void UpdateTutorialContent()
    {
        char alphabet = GetAlphabet();
        alphabetText.text = alphabet.ToString();
        signText.text = GetSignSprite(alphabet);
    }

    private char GetAlphabet()
    {
        int currChar = 'A' + (levelInfo.StageNumber - 1);
        return (char)currChar;
    }

    private string GetSignSprite(char alphabet)
    {
        string letter = $"<sprite name=\"{alphabet}\">";
        return letter;
    }
}