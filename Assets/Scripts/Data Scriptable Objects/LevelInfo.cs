using UnityEngine;

[CreateAssetMenu(fileName = "Level Info", menuName = "SO/Level Info")]
public class LevelInfo : ScriptableObject
{
    #region Variables

    [SerializeField] private int stageNumber;
    [SerializeField] private int maxStageNumber;
    public bool isEndless;

    private int startStageNumber;

    #endregion

    #region Properties

    public int StageNumber => stageNumber;
    public int MaxStageNumber => maxStageNumber;

    #endregion

    #region Functions

    public void LoadStageNumber()
    {
        startStageNumber = stageNumber;
        if (PlayerPrefs.HasKey(PlayerPrefsKey.StageNumber))
        {
            stageNumber = PlayerPrefs.GetInt(PlayerPrefsKey.StageNumber);
            PlayerPrefs.DeleteKey(PlayerPrefsKey.StageNumber);
        }
    }

    public void ResetStageNumber()
    {
        stageNumber = startStageNumber;
    }

    public static void ResetStageAndSave()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey.StageNumber, 1);
    }

    public static void SetStageAndSave(int savedStageNumber)
    {
        PlayerPrefs.SetInt(PlayerPrefsKey.StageNumber, savedStageNumber);
    }

    #endregion
}
