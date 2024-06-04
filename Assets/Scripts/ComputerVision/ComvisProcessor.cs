using UnityEngine;

public class ComvisProcessor : MonoBehaviour
{
    #region Variables

    [SerializeField] private EventNoParam OnPhotoButtonClicked;
    [SerializeField] private EventBool OnPhotoTaken;
    [SerializeField] private bool resultSuccessfull;

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

    private void PhotoButtonClicked()
    {
        OnPhotoTaken.Invoke(resultSuccessfull);
    }

    #endregion
}
