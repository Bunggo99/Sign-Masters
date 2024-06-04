using UnityEngine;

public class BoardLayout : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private GameObject[] floorTiles;
    [SerializeField] private GameObject[] outerFloorTiles;
    [SerializeField] private GameObject riverBottomLeft;
    [SerializeField] private GameObject riverBottomRight;
    [SerializeField] private GameObject riverUpperLeft;
    [SerializeField] private GameObject riverUpperRight;
    [SerializeField] private GameObject riverLeft;
    [SerializeField] private GameObject riverRight;
    [SerializeField] private GameObject riverBottom;
    [SerializeField] private GameObject riverUpper;

    #endregion

    #region Setup Floor and Walls For The Board

    public void SetupBoard(int columns, int rows)
    {
        //var randFloorColor = Random.ColorHSV(0.125f, 0.6f, 0, 0.75f, 0.75f, 1f);
        Transform boardHolder = new GameObject("Board").transform;

        for (int x = -1; x <= columns; x++)
        {
            for (int y = -1; y <= rows; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                if (x == -1 || y == -1 || y == rows || x == columns)
                    toInstantiate = GetOuterWall(x, y, columns, rows);
                //else
                //{
                //    var spriteRenderer = toInstantiate.GetComponent<SpriteRenderer>();
                //    spriteRenderer.color = randFloorColor;
                //}

                Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity, boardHolder);
            }
        }

        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10));
        Vector3 upperRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 10));

        int left = Mathf.FloorToInt(bottomLeft.x);
        int bottom = Mathf.FloorToInt(bottomLeft.y);
        int right = Mathf.CeilToInt(upperRight.x);
        int upper = Mathf.CeilToInt(upperRight.y);

        for (int x = left; x <= right; x++)
        {
            for (int y = bottom; y <= upper; y++)
            {    
                if (x >= -1 && x <= columns && y >= -1 && y <= rows) continue;

                GameObject toInstantiate = outerFloorTiles[Random.Range(0, outerFloorTiles.Length)];
                Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity, boardHolder);
            }
        }
    }

    private GameObject GetOuterWall(int x, int y, int columns, int rows)
    {
        if (y == -1 && x == -1)
            return riverBottomLeft;
        else if (y == -1 && x == columns)
            return riverBottomRight;
        else if (y == rows && x == -1)
            return riverUpperLeft;
        else if (y == rows && x == columns)
            return riverUpperRight;
        else if (x == -1)
            return riverLeft;
        else if (x == columns)
            return riverRight;
        else if (y == -1)
            return riverBottom;
        else if (y == rows)
            return riverUpper;
        else 
            return null;
    }

    #endregion
}
