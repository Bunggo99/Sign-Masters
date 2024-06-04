using UnityEngine;

public class LevelButtonList : MonoBehaviour
{
    #region Variables

    [SerializeField] private LevelInfo stageInfo;
    [SerializeField] private GameObject levelButtonPrefab;

    #endregion

    #region Enable

    private void OnEnable()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        for (int i = 0; i < stageInfo.MaxStageNumber; i++)
        {
            if (i >= transform.childCount)
            {
                Instantiate(levelButtonPrefab, transform);
            }

            GameObject obj = transform.GetChild(i).gameObject;
            if (!obj.activeSelf) obj.SetActive(true);
        }
    }

    #endregion
}
