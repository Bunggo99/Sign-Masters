using UnityEngine;

public class EndlessLayout : LevelLayout
{
    #region Variables

    [SerializeField] private RangedNum<int> wallCount = new(5, 8);
    [SerializeField] private RangedNum<int> foodCount = new(1, 4);
    [SerializeField] private int maxEnemyCount = 3;
    [SerializeField] private int enemyLogIncrease = 2;
    [SerializeField] private TilePrefabs prefabs;
    [SerializeField] private LevelInfo levelInfo;

    #endregion

    #region Layout Objects Implementation

    public override void LayoutObjects(BoardManager board)
    {
        base.LayoutObjects(board);

        LayoutTilesAtRandom(prefabs.WallTiles, wallCount.Min, wallCount.Max);
        LayoutAlphabetAtRandom(prefabs.AlphabetTile);
        LayoutTilesAtRandom(prefabs.FoodTiles, foodCount.Min, foodCount.Max);

        int enemyCount = (int)Mathf.Log(levelInfo.StageNumber, enemyLogIncrease);
        enemyCount = Mathf.Min(enemyCount, maxEnemyCount);
        LayoutTilesAtRandom(prefabs.EnemyTiles, enemyCount, enemyCount);

        GameObject exitObj = LayoutExitAtRandomOuterPos(prefabs.Exit);
        LayoutPlayerAcrossExit(prefabs.Player, exitObj.transform);
    }

    #endregion
}
