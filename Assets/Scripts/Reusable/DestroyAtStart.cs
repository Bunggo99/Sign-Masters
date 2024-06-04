using UnityEngine;

public class DestroyAtStart : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject);
    }
}
