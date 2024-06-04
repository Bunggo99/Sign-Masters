using UnityEngine;

public class StageLayout1 : LevelLayout
{
    #region Variables

    [SerializeField] private TilePrefabs prefabs;

    #endregion

    #region Layout Objects Implementation

    //plain: alphabet, enemy, player and exit
    public override void LayoutObjects(BoardManager board)
    {
        base.LayoutObjects(board);

        LayoutAlphabetAtRandom(prefabs.AlphabetTile);

        if (levelInfo.StageNumber == 1)
        {
            board.RemoveUnwantedOuterPositions();
        }

        int enemyCount = 1;
        LayoutTilesAtRandom(prefabs.EnemyTiles, enemyCount, enemyCount);

        GameObject exitObj = LayoutExitAtRandomOuterPos(prefabs.Exit);
        LayoutPlayerAcrossExit(prefabs.Player, exitObj.transform);

        if(levelInfo.StageNumber == 1)
        {
            exitObj.GetComponent<Exit>().EnableArrow();
        }
    }

    #endregion
}
