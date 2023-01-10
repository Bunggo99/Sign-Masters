using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private GameObject arrowCanvas;

    public void EnableArrow()
    {
        arrowCanvas.SetActive(true);
    }
}
