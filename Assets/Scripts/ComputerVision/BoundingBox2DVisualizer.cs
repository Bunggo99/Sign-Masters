using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using CJM.BBox2DToolkit;

namespace Bunggo99
{
    /// <summary>
    /// BoundingBox2DVisualizer is a MonoBehaviour class responsible for displaying 2D bounding boxes, labels, and label backgrounds
    /// on a Unity canvas. It creates, updates, and manages UI elements for visualizing the bounding boxes based on the provided
    /// BBox2DInfo array. This class supports customizable settings such as bounding box transparency and the ability to toggle
    /// the display of bounding boxes.
    /// </summary>
    public class BoundingBox2DVisualizer : MonoBehaviour
    {
        // UI components
        [Header("Components")]
        [Tooltip("Container for holding the bounding box UI elements")]
        [SerializeField] private RectTransform boundingBoxContainer;
        [Tooltip("Container for holding the label UI elements")]
        [SerializeField] private RectTransform labelContainer;

        // Prefabs for creating UI elements
        [Header("Prefabs")]
        [Tooltip("Prefab for the bounding box UI element")]
        [SerializeField] private RectTransform boundingBoxPrefab;
        [Tooltip("Prefab for the label UI element")]
        [SerializeField] private TMP_Text labelPrefab;
        [Tooltip("Prefab for the label background UI element")]
        [SerializeField] private Image labelBackgroundPrefab;
        [Tooltip("Prefab for the dot UI element")]
        [SerializeField] private Image dotPrefab;

        // Settings for customizing the bounding box visualizer
        [Header("Settings")]
        [Tooltip("Flag to control whether bounding boxes should be displayed or not")]
        [SerializeField] private bool displayBoundingBoxes = true;
        [Tooltip("Transparency value for the bounding boxes, ranging from 0 (completely transparent) to 1 (completely opaque)")]
        [SerializeField, Range(0f, 1f)] private float bboxTransparency = 1f;

        // GUIDs of the default assets for the bounding box, label, label background, and dot prefabs
        private const string BoundingBoxPrefabGUID = "be0edeacc0f249fab31ac75426ad8a2a";
        private const string LabelPrefabGUID = "4e39b47d4b984862aeab14255855fcc9";
        private const string LabelBackgroundPrefabGUID = "9074ea186151430084312ba891bad58e";
        private const string DotPrefabGUID = "3eb64b4f1a4e4e2595066ed269be9532";

        // Lists for storing and managing instantiated UI elements
        private List<RectTransform> boundingBoxes = new List<RectTransform>(); // List of instantiated bounding box UI elements
        private List<TMP_Text> labels = new List<TMP_Text>(); // List of instantiated label UI elements
        private List<Image> labelBackgrounds = new List<Image>(); // List of instantiated label background UI elements
        private List<Image> dots = new List<Image>(); // List of instantiated dot UI elements

        /// <summary>
        /// Reset is called when the user hits the Reset button in the Inspector's context menu
        /// or when adding the component the first time. This function is only called in editor mode.
        /// </summary>
        private void Reset()
        {
            // Load default assets only in the Unity Editor, not in a build
#if UNITY_EDITOR
            boundingBoxPrefab = LoadDefaultAsset<RectTransform>(BoundingBoxPrefabGUID);
            labelPrefab = LoadDefaultAsset<TMP_Text>(LabelPrefabGUID);
            labelBackgroundPrefab = LoadDefaultAsset<Image>(LabelBackgroundPrefabGUID);
            dotPrefab = LoadDefaultAsset<Image>(DotPrefabGUID);
#endif
        }

        /// <summary>
        /// Loads the default asset for the specified type using its GUID.
        /// </summary>
        /// <typeparam name="T">The type of asset to be loaded.</typeparam>
        /// <param name="guid">The GUID of the default asset.</param>
        /// <returns>The loaded asset of the specified type.</returns>
        /// <remarks>
        /// This method is only executed in the Unity Editor, not in builds.
        /// </remarks>
        private T LoadDefaultAsset<T>(string guid) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            // Load the asset from the AssetDatabase using its GUID
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
#else
            return null;
#endif
        }

        /// <summary>
        /// Update the visualization of bounding boxes based on the given BBox2DInfo array.
        /// </summary>
        /// <param name="bboxInfoArray">An array of BBox2DInfo objects containing bounding box information</param>
        public void UpdateBoundingBoxVisualizations(BBox2DInfo[] bboxInfoArray, bool showLabel)
        {
            // Depending on the displayBoundingBoxes flag, either update or disable bounding box UI elements
            if (displayBoundingBoxes)
            {
                UpdateBoundingBoxes(bboxInfoArray, showLabel);
            }
            else
            {
                // Disable bounding boxes, labels, and label backgrounds for all existing UI elements
                for (int i = 0; i < boundingBoxes.Count; i++)
                {
                    boundingBoxes[i].gameObject.SetActive(false);
                    labelBackgrounds[i].gameObject.SetActive(false);
                    labels[i].gameObject.SetActive(false);
                    dots[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Update the bounding box UI element with the information from the given BBox2DInfo object.
        /// </summary>
        /// <param name="boundingBox">The RectTransform object representing the bounding box UI element</param>
        /// <param name="bboxInfo">The BBox2DInfo object containing the information for the bounding box</param>
        private void UpdateBoundingBox(RectTransform boundingBox, BBox2DInfo bboxInfo)
        {
            // Convert the screen point to a local point in the RectTransform space of the bounding box container
            Vector2 localPosition = ScreenToCanvasPoint(boundingBoxContainer, new Vector2(bboxInfo.bbox.x0, bboxInfo.bbox.y0));
            boundingBox.anchoredPosition = localPosition;
            boundingBox.sizeDelta = new Vector2(bboxInfo.bbox.width, bboxInfo.bbox.height);

            // Set the color of the bounding box with the specified transparency
            Color color = GetColorWithTransparency(bboxInfo.color);
            Image[] sides = boundingBox.GetComponentsInChildren<Image>();
            foreach (Image side in sides)
            {
                side.color = color;
            }
        }

        /// <summary>
        /// Convert a screen point to a local point in the RectTransform space of the given canvas.
        /// </summary>
        /// <param name="canvas">The RectTransform object of the canvas</param>
        /// <param name="screenPoint">The screen point to be converted</param>
        /// <returns>A Vector2 object representing the local point in the RectTransform space of the canvas</returns>
        private Vector2 ScreenToCanvasPoint(RectTransform canvas, Vector2 screenPoint)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPoint, null, out Vector2 localPoint);
            return localPoint;
        }

        /// <summary>
        /// Update bounding box UI elements to match the provided BBox2DInfo array.
        /// </summary>
        /// <param name="bboxInfoArray">An array of BBox2DInfo objects containing bounding box information</param>
        private void UpdateBoundingBoxes(BBox2DInfo[] bboxInfoArray, bool showLabel)
        {
            // Create or remove bounding box UI elements to match the number of detected objects
            while (boundingBoxes.Count < bboxInfoArray.Length)
            {
                RectTransform newBoundingBox = Instantiate(boundingBoxPrefab, boundingBoxContainer);
                boundingBoxes.Add(newBoundingBox);

                Image newLabelBackground = Instantiate(labelBackgroundPrefab, labelContainer);
                labelBackgrounds.Add(newLabelBackground);

                TMP_Text newLabel = Instantiate(labelPrefab, labelContainer);
                labels.Add(newLabel);

                Image newDot = Instantiate(dotPrefab, boundingBoxContainer);
                dots.Add(newDot);
            }

            // Update bounding boxes, labels, and label backgrounds for each detected object, or disable UI elements if not needed
            for (int i = 0; i < boundingBoxes.Count; i++)
            {
                if (i < bboxInfoArray.Length)
                {
                    BBox2DInfo bboxInfo = bboxInfoArray[i];

                    // Get UI elements for the current bounding box, label, and label background
                    RectTransform boundingBox = boundingBoxes[i];
                    TMP_Text label = labels[i];
                    Image labelBackground = labelBackgrounds[i];
                    Image dot = dots[i];

                    UpdateBoundingBox(boundingBox, bboxInfo);
                    UpdateLabelAndBackground(label, labelBackground, bboxInfo);
                    UpdateDot(dot, bboxInfo);

                    // Enable bounding box, label, and label background UI elements
                    boundingBox.gameObject.SetActive(true);
                    labelBackground.gameObject.SetActive(true);
                    label.gameObject.SetActive(showLabel);
                    dots[i].gameObject.SetActive(true);
                }
                else
                {
                    // Disable UI elements for extra bounding boxes, labels, and label backgrounds
                    boundingBoxes[i].gameObject.SetActive(false);
                    labelBackgrounds[i].gameObject.SetActive(false);
                    labels[i].gameObject.SetActive(false);
                    dots[i].gameObject.SetActive(false);
                }
            }
        }

        public void UpdateBoundingBoxPos(BBox2DInfo[] bboxInfoArray,
            Vector2 pictureSize, RectTransform resultUI)
        {
            for (int i = 0; i < boundingBoxes.Count; i++)
            {
                if (i < bboxInfoArray.Length)
                {
                    UpdateBoundingBoxPos(boundingBoxes[i], pictureSize, resultUI);
                    UpdateLabelAndBackgroundPos(labels[i], labelBackgrounds[i], pictureSize, resultUI);
                    UpdateDotPos(dots[i], pictureSize, resultUI);
                }
            }
        }

        private void UpdateDotPos(Image dot, Vector2 resultPicture, RectTransform resultUI)
        {
            var screenDimX = resultUI.rect.width;
            var screenDimY = resultUI.rect.height;

            var resDimX = resultPicture.x;
            var resDimY = resultPicture.y;


            var newPosX = dot.rectTransform.anchoredPosition.x / screenDimX * resDimX;
            var newPosY = dot.rectTransform.anchoredPosition.y / screenDimY * resDimY;

            dot.rectTransform.anchoredPosition = new Vector2(newPosX, newPosY);
        }

        private void UpdateLabelAndBackgroundPos(TMP_Text label, Image labelBackground, Vector2 resultPicture, RectTransform resultUI)
        {
            var screenDimX = resultUI.rect.width;
            var screenDimY = resultUI.rect.height;

            var resDimX = resultPicture.x;
            var resDimY = resultPicture.y;

            var newLabelPosX = label.rectTransform.anchoredPosition.x / screenDimX * resDimX;
            var newLabelPosY = label.rectTransform.anchoredPosition.y / screenDimY * resDimY;

            label.rectTransform.anchoredPosition = new Vector2(newLabelPosX, newLabelPosY);

            var newWidth0 = label.rectTransform.sizeDelta.x / screenDimX * resDimX;
            var newHeight0 = label.rectTransform.sizeDelta.y / screenDimY * resDimY;

            label.rectTransform.sizeDelta = new Vector2(newWidth0, newHeight0);

            var newLabelBGPosX = labelBackground.rectTransform.anchoredPosition.x / screenDimX * resDimX;
            var newLabelBGPosY = labelBackground.rectTransform.anchoredPosition.y / screenDimY * resDimY;

            labelBackground.rectTransform.anchoredPosition = new Vector2(newLabelBGPosX, newLabelBGPosY);

            var newWidth = labelBackground.rectTransform.sizeDelta.x / screenDimX * resDimX;
            var newHeight = labelBackground.rectTransform.sizeDelta.y / screenDimY * resDimY;

            labelBackground.rectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        }

        private void UpdateBoundingBoxPos(RectTransform boundingBox, Vector2 resultPicture, RectTransform resultUI)
        {
            var screenDimX = resultUI.rect.width;
            var screenDimY = resultUI.rect.height;

            var resDimX = resultPicture.x;
            var resDimY = resultPicture.y;

            var newPosX = boundingBox.anchoredPosition.x / screenDimX * resDimX;
            var newPosY = boundingBox.anchoredPosition.y / screenDimY * resDimY;

            boundingBox.anchoredPosition = new Vector2(newPosX, newPosY);

            var newWidth = boundingBox.sizeDelta.x / screenDimX * resDimX;
            var newHeight = boundingBox.sizeDelta.y / screenDimY * resDimY;

            boundingBox.sizeDelta = new Vector2(newWidth, newHeight);
        }

        /// <summary>
        /// Update the label and label background UI elements with the information from the given BBox2DInfo object.
        /// </summary>
        /// <param name="label">The TMP_Text object representing the label UI element</param>
        /// <param name="labelBackground">The Image object representing the label background UI element</param>
        /// <param name="bboxInfo">The BBox2DInfo object containing the information for the label and label background</param>
        private void UpdateLabelAndBackground(TMP_Text label, Image labelBackground, BBox2DInfo bboxInfo)
        {
            // Convert the screen point to a local point in the RectTransform space of the bounding box container
            Vector2 localPosition = ScreenToCanvasPoint(boundingBoxContainer, new Vector2(bboxInfo.bbox.x0, bboxInfo.bbox.y0));

            // Set the label text and position
            var confidence = (bboxInfo.bbox.prob * 100).ToString("0.##");
            label.text = $"{bboxInfo.label}; {confidence}";
            label.rectTransform.anchoredPosition = new Vector2(localPosition.x, localPosition.y - label.preferredHeight);

            // Set the label color based on the grayscale value of the bounding box color
            Color color = GetColorWithTransparency(bboxInfo.color);
            label.color = color.grayscale > 0.5 ? Color.black : Color.white;

            // Set the label background position and size
            labelBackground.rectTransform.anchoredPosition = new Vector2(localPosition.x, localPosition.y - label.preferredHeight);
            labelBackground.rectTransform.sizeDelta = new Vector2(Mathf.Max(label.preferredWidth, bboxInfo.bbox.width), label.preferredHeight);

            // Set the label background color with the specified transparency
            labelBackground.color = color;
        }

        /// <summary>
        /// Update the dot UI element with the information from the given BBox2DInfo object.
        /// </summary>
        /// <param name="dot">The Image object representing the dot UI element</param>
        /// <param name="bboxInfo">The BBox2DInfo object containing the information for the bounding box</param>
        private void UpdateDot(Image dot, BBox2DInfo bboxInfo)
        {
            // Calculate the center of the bounding box
            Vector2 center = new Vector2(bboxInfo.bbox.x0 + bboxInfo.bbox.width / 2, bboxInfo.bbox.y0 - bboxInfo.bbox.height / 2);

            // Convert the screen point to a local point in the RectTransform space of the bounding box container
            Vector2 localPosition = ScreenToCanvasPoint(boundingBoxContainer, center);

            // Set the dot position
            dot.rectTransform.anchoredPosition = localPosition;

            // Set the dot color with the specified transparency
            Color color = GetColorWithTransparency(bboxInfo.color);
            dot.color = color;
        }

        /// <summary>
        /// Get a new color based on the input color with the adjusted transparency.
        /// </summary>
        /// <param name="color">The input color to be modified</param>
        /// <returns>A new color with the specified transparency</returns>
        private Color GetColorWithTransparency(Color color)
        {
            color.a = bboxTransparency;
            return color;
        }

    }
}