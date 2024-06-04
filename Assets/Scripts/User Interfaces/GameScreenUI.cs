using UnityEngine;

public class GameScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject[] disableOnComVisLoading;
    [SerializeField] private GameObject stageText;
    [SerializeField] private EventNoParam OnComputerVisionSceneLoading;
    [SerializeField] private EventBool OnComputerVisionSceneUnloading;
    [SerializeField] private EventObject OnEnemyInteracted;
    [SerializeField] private EventBool OnPhotoTaken;

    private Enemy interactedEnemy;

    private void OnEnable()
    {
        OnComputerVisionSceneLoading.AddListener(ComputerVisionSceneLoading);
        OnComputerVisionSceneUnloading.AddListener(ComputerVisionSceneUnloading);
        OnEnemyInteracted.AddListener(EnemyInteracted);
        OnPhotoTaken.AddListener(PhotoTaken);
    }

    private void OnDisable()
    {
        OnComputerVisionSceneLoading.RemoveListener(ComputerVisionSceneLoading);
        OnComputerVisionSceneUnloading.RemoveListener(ComputerVisionSceneUnloading);
        OnEnemyInteracted.RemoveListener(EnemyInteracted);
        OnPhotoTaken.RemoveListener(PhotoTaken);
    }

    private void ComputerVisionSceneLoading()
    {
        foreach (var obj in disableOnComVisLoading)
        {
            obj.SetActive(false);
        }
    }

    private void PhotoTaken(bool success)
    {
        stageText.SetActive(false);
    }

    private void ComputerVisionSceneUnloading(bool success)
    {
        foreach (var obj in disableOnComVisLoading)
        {
            obj.SetActive(true);
        }
        stageText.SetActive(true);
        interactedEnemy.EnemyBattled(success);
    }

    private void EnemyInteracted(object enemy)
    {
        interactedEnemy = (Enemy)enemy;
    }
}
