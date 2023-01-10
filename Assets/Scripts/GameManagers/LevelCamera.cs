using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    #region Variables

    [SerializeField] private BoardSize size;

    private Camera _cam;

    #endregion

    #region Awake

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    #endregion

    #region Setup Camera Pos And Size

    private void Start()
    {
        SetupPosAndSize(size.Columns, size.Rows);
    }

    private void SetupPosAndSize(int columns, int rows)
    {
        float xPos = (columns - 1) / 2f;
        float yPos = (rows - 1) / 2f;
        transform.position = new Vector3(xPos, yPos, transform.position.z);

        float size = 0f;
        Vector3 mostUpperRightPos = new(columns + 0.5f, rows + 0.5f);
        Vector3 camPosToTargetPos = mostUpperRightPos - Camera.main.transform.position;

        size = Mathf.Max(size, Mathf.Abs(camPosToTargetPos.y));
        size = Mathf.Max(size, Mathf.Abs(camPosToTargetPos.x) / Camera.main.aspect);
        _cam.orthographicSize = size;
    }

    #endregion
}
