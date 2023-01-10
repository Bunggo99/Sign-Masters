using UnityEngine;
using TMPro;

public class DictionaryUI : MonoBehaviour
{
    [SerializeField] private GameObject dictionaryPanel;
    [SerializeField] private TextMeshProUGUI alphabetText;
    [SerializeField] private TextMeshProUGUI signText;
    [SerializeField] protected TextMeshProUGUI pageText;
    [SerializeField] protected GameObject prevBtn;
    [SerializeField] protected GameObject nextBtn;
    [SerializeField] protected LevelInfo levelInfo;

    protected int page = 1;
    protected int Page
    {
        get => page;
        set 
        {
            page = value;
            UpdateDictionaryContent();
        }
    }
    private int maxPage;

    private void OnEnable()
    {
        UpdateDictionaryContent();
    }

    protected virtual void UpdateDictionaryContent()
    {
        maxPage = PlayerPrefs.GetInt(MainMenu.UNLOCKED_STAGES_KEY);
        maxPage = Mathf.Min(maxPage, levelInfo.MaxStageNumber);
        pageText.text = page.ToString() + " out of " + maxPage;

        if(maxPage == 1)
        {
            prevBtn.SetActive(false);
            nextBtn.SetActive(false);
        }

        char alphabet = GetAlphabet();
        alphabetText.text = alphabet.ToString();
        signText.text = GetSignSprite(alphabet);
    }

    private char GetAlphabet()
    {
        int currChar = 'A' + (Page - 1);
        return (char)currChar;
    }

    private string GetSignSprite(char alphabet)
    {
        string letter = $"<sprite name=\"{alphabet}\">";
        return letter;
    }


    public void NextChar()
    {
        if (Page < maxPage)
            Page++;
        else 
            Page = 1;
    }

    public void PrevChar()
    {
        if (Page > 1)
            Page--;
        else
            Page = maxPage;
    }

    public void ToggleDictionaryPanel()
    {
        dictionaryPanel.SetActive(!dictionaryPanel.activeSelf);
    }
}