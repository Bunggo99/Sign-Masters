using UnityEngine;

public class StageLayout4 : LevelLayout
{
    #region Variables

    [SerializeField] private RangedNum<int> wallCount = new(5, 8);
    [SerializeField] private RangedNum<int> foodCount = new(1, 4);
    [SerializeField] private TilePrefabs prefabs;

    #endregion

    #region Layout Objects Implementation

    //alphabet, enemy, player, exit, food/drink, and obstacles
    public override void LayoutObjects(BoardManager board)
    {
        base.LayoutObjects(board);

        LayoutTilesAtRandom(prefabs.WallTiles, wallCount.Min, wallCount.Max);
        LayoutTilesAtRandom(prefabs.FoodTiles, foodCount.Min, foodCount.Max);
        LayoutAlphabetAtRandom(prefabs.AlphabetTile);

        int enemyCount = (int)Mathf.Log(levelInfo.StageNumber, enemyLogIncrease);
        LayoutTilesAtRandom(prefabs.EnemyTiles, enemyCount, enemyCount);

        GameObject exitObj = LayoutExitAtRandomOuterPos(prefabs.Exit);
        LayoutPlayerAcrossExit(prefabs.Player, exitObj.transform);
    }

    #endregion
}
