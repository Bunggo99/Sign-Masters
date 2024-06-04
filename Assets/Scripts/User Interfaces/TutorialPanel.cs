using TMPro;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private LevelInfo levelInfo;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private string[] texts;

    private void Start()
    {
        int stage = levelInfo.StageNumber - 1;
        if (stage < texts.Length && !string.IsNullOrEmpty(texts[stage]))
            tutorialText.text = texts[stage];
        else
            gameObject.SetActive(false);
    }
}
