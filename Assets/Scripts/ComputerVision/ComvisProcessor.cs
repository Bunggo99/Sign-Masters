using CJM.BarracudaInference.YOLOX;
using CJM.BBox2DToolkit;
using CJM.DeepLearningImageProcessor;
using UnityEngine;
using UnityEngine.UI;

public class ComvisProcessor : MonoBehaviour
{
    #region Variables

    [SerializeField] private EventNoParam OnPhotoButtonClicked;
    [SerializeField] private EventBool OnPhotoTaken;
    [SerializeField] private MeshRenderer liveFeed;
    [SerializeField] private ImageProcessor imageProcessor;
    [SerializeField] private YOLOXObjectDetector modelRunner;
    [SerializeField] private BoundingBox2DVisualizer boundingBoxVisualizer;
    [SerializeField] private int targetDim = 224;
    [SerializeField] private bool normalizeInput = false;
    [SerializeField, Range(0, 1)] private float confidenceThreshold = 0.5f;
    [SerializeField, Range(0, 1)] private float nmsThreshold = 0.45f;

    [SerializeField] private bool resultSuccessfull;

    private bool photoButtonClicked = false;
    private Vector2Int offset; // Offset used when cropping the input image
    private BBox2DInfo[] bboxInfoArray; // Array to store bounding box information

    #endregion

    #region Functions

    private void OnEnable()
    {
        OnPhotoButtonClicked.AddListener(PhotoButtonClicked);
    }

    private void OnDisable()
    {
        OnPhotoButtonClicked.RemoveListener(PhotoButtonClicked);
    }

    private void Update()
    {
        // Get the source image and dimensions
        var sourceTexture = liveFeed.material.mainTexture;
        if (sourceTexture == null) return;

        var sourceDims = new Vector2Int(sourceTexture.width, sourceTexture.height);
        sourceDims = imageProcessor.CalculateInputDims(sourceDims, targetDim);

        // Calculate input dimensions for model input
        var inputDims = modelRunner.CropInputDims(sourceDims);

        // Prepare and process the input texture
        RenderTexture inputRenderTexture = PrepareRenderTexture(inputDims);
        ProcessInputImage(inputRenderTexture, inputDims, sourceTexture, sourceDims);

        // Get the model output and process the detected objects
        float[] outputArray = GetModelOutput(inputRenderTexture);
        bboxInfoArray = modelRunner.ProcessOutput(outputArray, confidenceThreshold, nmsThreshold);

        // Update bounding boxes and user interface
        UpdateBoundingBoxes(inputDims);
        boundingBoxVisualizer.UpdateBoundingBoxVisualizations(bboxInfoArray, photoButtonClicked);
    }

    private void PhotoButtonClicked()
    {
        photoButtonClicked = true;

        // Get the source image and dimensions
        var sourceTexture = liveFeed.material.mainTexture;
        if (sourceTexture == null) return;

        var sourceDims = new Vector2Int(sourceTexture.width, sourceTexture.height);
        Debug.Log(sourceTexture.width + " " + sourceTexture.height);
        sourceDims = imageProcessor.CalculateInputDims(sourceDims, targetDim);
        Debug.Log(sourceDims.x + " " + sourceDims.y);

        // Calculate input dimensions for model input
        var inputDims = modelRunner.CropInputDims(sourceDims);

        // Prepare and process the input texture
        RenderTexture inputRenderTexture = PrepareRenderTexture(inputDims);
        ProcessInputImage(inputRenderTexture, inputDims, sourceTexture, sourceDims);
        Debug.Log(inputRenderTexture.width + " " + inputRenderTexture.height);

        // Get the model output and process the detected objects
        float[] outputArray = GetModelOutput(inputRenderTexture);
        bboxInfoArray = modelRunner.ProcessOutput(outputArray, confidenceThreshold, nmsThreshold);
        Debug.Log(bboxInfoArray[0].bbox.x0 + " " + bboxInfoArray[0].bbox.y0);

        // Update bounding boxes and user interface
        UpdateBoundingBoxes(inputDims);
        Debug.Log(bboxInfoArray[0].bbox.x0 + " " + bboxInfoArray[0].bbox.y0);
        boundingBoxVisualizer.UpdateBoundingBoxVisualizations(bboxInfoArray, photoButtonClicked);

        resultSuccessfull = false;
        for (int i = 0; i < bboxInfoArray.Length; i++)
        {
            BBox2DInfo bboxInfo = bboxInfoArray[i];
            if (bboxInfo.label.ToCharArray()[0] == ComvisUI.charQuestion)
                resultSuccessfull = true;
        }

        OnPhotoTaken.Invoke(resultSuccessfull);
    }

    private RenderTexture PrepareRenderTexture(Vector2Int dims)
    {
        return RenderTexture.GetTemporary(dims.x, dims.y, 0, RenderTextureFormat.ARGBHalf);
    }

    private void ProcessInputImage(RenderTexture inputRenderTexture, Vector2Int inputDims, Texture sourceTexture, Vector2Int sourceDims)
    {
        // Calculate the offset for cropping the input image
        offset = (sourceDims - inputDims) / 2;

        // Create a temporary render texture to store the cropped image
        RenderTexture sourceRenderTexture = PrepareRenderTexture(sourceDims);
        Graphics.Blit(sourceTexture, sourceRenderTexture);

        // Crop and normalize the input image using Compute Shaders or fallback to Shader processing
        ProcessImageShader(sourceRenderTexture, inputRenderTexture, sourceDims, inputDims);

        // Release the temporary render texture
        RenderTexture.ReleaseTemporary(sourceRenderTexture);
    }

    private void ProcessImageShader(RenderTexture sourceTexture, RenderTexture inputTexture, Vector2Int sourceDims, Vector2Int inputDims)
    {
        // Calculate the scaled offset and size for cropping the input image
        Vector2 scaledOffset = offset / (Vector2)sourceDims;
        Vector2 scaledSize = inputDims / (Vector2)sourceDims;

        // Create offset and size arrays for the Shader
        float[] offsetArray = new float[] { scaledOffset.x, scaledOffset.y };
        float[] sizeArray = new float[] { scaledSize.x, scaledSize.y };

        // Crop and normalize the input image using Shaders
        imageProcessor.CropImageShader(sourceTexture, inputTexture, offsetArray, sizeArray);
        if (normalizeInput) imageProcessor.ProcessImageShader(inputTexture);
    }

    private float[] GetModelOutput(RenderTexture inputTexture)
    {
        // Run the model with the processed input texture
        modelRunner.ExecuteModel(inputTexture);
        RenderTexture.ReleaseTemporary(inputTexture);
        return modelRunner.CopyOutputToArray();
    }

    private void UpdateBoundingBoxes(Vector2Int inputDims)
    {
        // Get the screen dimensions
        var screenDims = new Vector2(liveFeed.transform.localScale.x, liveFeed.transform.localScale.y);

        // Scale and position the bounding boxes based on the input and screen dimensions
        for (int i = 0; i < bboxInfoArray.Length; i++)
        {
            bboxInfoArray[i].bbox = BBox2DUtility.ScaleBoundingBox(bboxInfoArray[i].bbox, inputDims, screenDims, offset, true);
        }
    }

    #endregion
}
