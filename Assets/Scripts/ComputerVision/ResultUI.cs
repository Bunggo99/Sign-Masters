using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject resultPanel;
    [SerializeField] private EventBool OnPhotoTaken;
    [SerializeField] private EventBool OnComputerVisionSceneUnloading;
    [SerializeField] private RawImage resultPicture;
    [SerializeField] private MeshRenderer liveFeed;
    [SerializeField] private TMP_Text resultText;

    private bool takingPhoto = true;
    private bool resultSuccess = true;

    #endregion

    #region Functions

    private void OnEnable()
    {
        OnPhotoTaken.AddListener(PhotoTaken);
    }

    private void OnDisable()
    {
        OnPhotoTaken.RemoveListener(PhotoTaken);
    }

    private void PhotoTaken(bool success)
    {
        if (takingPhoto)
        {
            takingPhoto = false;
            resultPanel.SetActive(true);

            resultSuccess = success;
            if (success)
            {
                resultText.text = "You got it right!\nYou killed the Enemy!";
            }
            else
            {
                resultText.text = "Sorry, you got it wrong :(\nYou got damaged by the enemy...";
            }

            resultPicture.texture = liveFeed.material.mainTexture;

            float aspectRatio = (float)resultPicture.texture.width / resultPicture.texture.height;
            resultPicture.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;
        }
    }

    public void CloseButtonClicked()
    {
        OnComputerVisionSceneUnloading.Invoke(resultSuccess);
        SceneManager.UnloadSceneAsync("ComputerVision");
    }

    #endregion
}
