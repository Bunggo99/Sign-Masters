using System;
using System.Collections;
using CJM.MediaDisplay;
using UnityEngine;

public class WebcamSetup : MonoBehaviour
{
    [SerializeField] private Vector2Int webcamResolution = new(1280, 720);
    [SerializeField, Range(0, 60)] private int webcamFrameRate = 60;
    [SerializeField] private EventBool OnPhotoTaken;

    private WebCamDevice[] webcamDevices;
    private string currentWebcam;
    private WebCamTexture currentWebcamTexture;

    private void Start()
    {
        webcamDevices = WebCamTexture.devices;
        currentWebcam = webcamDevices.Length > 0 ? webcamDevices[0].name : "";

        SetupWebcam();
        StartCoroutine(UpdateScreenTextureAsync());
    }

    private void SetupWebcam()
    {
        if (webcamDevices.Length == 0) return;

        InitializeWebcam(ref currentWebcamTexture, currentWebcam, webcamResolution, webcamFrameRate);
        Application.targetFrameRate = webcamFrameRate * 4;
    }

    private void InitializeWebcam(ref WebCamTexture webcamTexture, string deviceName, Vector2Int webcamDimensions, int webcamFrameRate = 60)
    {
        if (webcamTexture != null && webcamTexture.isPlaying) webcamTexture.Stop();

        webcamTexture = new WebCamTexture(deviceName, webcamDimensions.x, webcamDimensions.y, webcamFrameRate);
        webcamTexture.Play();
    }

    private IEnumerator UpdateScreenTextureAsync()
    {
        while (currentWebcamTexture.isPlaying && currentWebcamTexture.width <= 16)
        {
            yield return null;
        }

        MediaDisplayManager.UpdateScreenTexture(gameObject, currentWebcamTexture, Camera.main.gameObject, true);
    }

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
        currentWebcamTexture.Stop();
        gameObject.SetActive(false);
    }
}
