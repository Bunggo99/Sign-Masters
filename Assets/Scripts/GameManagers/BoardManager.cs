using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : SingletonManager<BoardManager>
{
    #region Variables

    [SerializeField] private BoardSize size;

    [SerializeField] private EventNoParam OnLayoutSpawned;
    [SerializeField] private EventTransform OnRandomizeObjectPosition;
    [SerializeField] private EventTwoVector3 OnPositionUpdated;

    private readonly List<Vector3> _gridPositions = new();
    private readonly List<Vector3> _outerPositions = new();

    #endregion

    #region Enable/Disable

    private void OnEnable()
    {
        OnRandomizeObjectPosition.AddListener(RandomizeObjectPosition);
        OnPositionUpdated.AddListener(UpdateAvailableGridPos);
    }

    private void OnDisable()
    {
        OnRandomizeObjectPosition.RemoveListener(RandomizeObjectPosition);
        OnPositionUpdated.RemoveListener(UpdateAvailableGridPos);
    }

    #endregion

    #region Setup Level

    private void Start()
    {
        SetupLevel();
    }

    private void SetupLevel()
    {
        InitializeAvailablePositions();

        var boardLayout = GetComponent<BoardLayout>();
        if (boardLayout != null) boardLayout.SetupBoard(size.Columns, size.Rows);

        var levelLayout = GetComponentInChildren<LevelLayout>();
        if (levelLayout != null) levelLayout.LayoutObjects(this);
        else
        {
            OnLayoutSpawned.AddListener(LayoutObject);
        }
    }

    private void LayoutObject()
    {
        var levelLayout = GetComponentInChildren<LevelLayout>();
        if (levelLayout != null) levelLayout.LayoutObjects(this);
        OnLayoutSpawned.RemoveListener(LayoutObject);
    }

    #endregion

    #region Init Pos List

    private void InitializeAvailablePositions()
    {
        _gridPositions.Clear();

        for (int x = 1; x <= size.Columns - 2; x++)
        {
            for (int y = 1; y <= size.Rows - 2; y++)
            {
                _gridPositions.Add(new Vector3(x, y, 0f));
            }
        }

        _outerPositions.Clear();
        for (int x = 0; x <= (size.Columns - 1); x += (size.Columns - 1))
        {
            for (int y = 0; y <= (size.Rows - 1); y++)
                _outerPositions.Add(new Vector3(x, y, 0f));
        }
        for (int y = 0; y <= (size.Rows - 1); y += (size.Rows - 1))
        {
            for (int x = 1; x <= (size.Columns - 2); x++)
                _outerPositions.Add(new Vector3(x, y, 0f));
        }
    }

    //To Remove Outer Positions Obscured By Sign Langague UI
    public void RemoveUnwantedOuterPositions()
    {
        _outerPositions.Remove(new Vector3(9, 7, 0f));
        _outerPositions.Remove(new Vector3(10, 7, 0f));
        _outerPositions.Remove(new Vector3(11, 7, 0f));
        _outerPositions.Remove(new Vector3(11, 6, 0f));
        _outerPositions.Remove(new Vector3(11, 5, 0f));
    }

    #endregion

    #region Randomize Object Position

    private void RandomizeObjectPosition(Transform obj)
    {
        obj.position = GetRandomGridPos();
    }

    #endregion

    #region Get Positions

    public Vector3 GetRandomGridPos()
    {
        int randomIndex = Random.Range(0, _gridPositions.Count);
        Vector3 randomPosition = _gridPositions[randomIndex];
        _gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    public Vector3 GetRandomOuterPos()
    {
        int randomIndex = Random.Range(0, _outerPositions.Count);
        Vector2 randomPosition = _outerPositions[randomIndex];
        _outerPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    public Vector3 GetPlayerPosAcrossExit(Vector3 exitPos)
    {
        float posX = (size.Columns - 1) - exitPos.x;
        float posY = (size.Rows - 1) - exitPos.y;
        Vector3 playerPos = new(posX, posY, 0f);
        _outerPositions.Remove(playerPos);

        return playerPos;
    } 

    #endregion

    #region Update Available Positions

    private void UpdateAvailableGridPos(Vector3 availablePos, Vector3 usedPos)
    {
        if (_gridPositions.Contains(usedPos))
            _gridPositions.Remove(usedPos);

        if(availablePos.x >= 1 && availablePos.x <= size.Columns - 2 
           && availablePos.y >= 1 && availablePos.y <= size.Rows - 2)
        {
            _gridPositions.Add(availablePos);
        }
    }

    #endregion
}
