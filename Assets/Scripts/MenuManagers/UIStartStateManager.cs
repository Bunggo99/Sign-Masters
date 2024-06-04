using UnityEngine;

public class UIStartStateManager : MonoBehaviour
{
    [SerializeField] private GameObject[] disableAtStart;
    [SerializeField] private GameObject[] enableAtStart;

    private void Start()
    {
        foreach (var obj in disableAtStart)
        {
            if (obj.activeInHierarchy)
                obj.SetActive(false);
        }

        foreach (var obj in enableAtStart)
        {
            if (!obj.activeInHierarchy)
                obj.SetActive(true);
        }
    }
}