using System.Collections;
using UnityEngine;
using CJM.MediaDisplay;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class DemoScreenManager : BaseScreenManager
{
    [SerializeField] private EventBool OnPhotoTaken;

    // Called when the script instance is being loaded.
    private void Start()
    {
        cameraObject = Camera.main.gameObject;
        Initialize();
        UpdateDisplay();
    }

    // Updates the display with the current texture (either a test texture or the webcam feed).
    protected override void UpdateDisplay()
    {
        // Call the base class implementation first
        base.UpdateDisplay();
    }

    // Update the current webcam device and the display when the webcam dropdown selection changes
    public void UpdateWebcamDevice()
    {
        currentWebcam = webcamDevices[0].name;
        useWebcam = webcamDevices.Length > 0 ? useWebcam : false;
        UpdateDisplay();
    }

    // Handle the texture change event.
    protected override void HandleMainTextureChanged(Material material)
    {
        // Call the base class implementation first
        base.HandleMainTextureChanged(material);
    }

    protected override void OnEnable()
    {
        OnPhotoTaken.AddListener(PhotoTaken);
    }

    protected override void OnDisable()
    {
        OnPhotoTaken.RemoveListener(PhotoTaken);
    }

    private void PhotoTaken(bool success)
    {
        webcamTexture.Stop();
        gameObject.SetActive(false);
    }
}
