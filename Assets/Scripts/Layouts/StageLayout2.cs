using UnityEngine;

public class StageLayout2 : LevelLayout
{
    #region Variables

    [SerializeField] private RangedNum<int> foodCount = new(1, 4);
    [SerializeField] private TilePrefabs prefabs;

    #endregion

    #region Layout Objects Implementation

    //alphabet, player, exit, and food/drink
    public override void LayoutObjects(BoardManager board)
    {
        base.LayoutObjects(board);

        LayoutTilesAtRandom(prefabs.FoodTiles, foodCount.Min, foodCount.Max);
        LayoutAlphabetAtRandom(prefabs.AlphabetTile);
        GameObject exitObj = LayoutExitAtRandomOuterPos(prefabs.Exit);
        LayoutPlayerAcrossExit(prefabs.Player, exitObj.transform);
    }

    #endregion
}
