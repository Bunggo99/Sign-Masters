using UnityEngine;

[CreateAssetMenu(fileName = "Tile Prefabs", menuName = "SO/Tile Prefabs")]
public class TilePrefabs : ScriptableObject
{
    #region Variables

    [SerializeField] private GameObject[] wallTiles;
    [SerializeField] private GameObject[] foodTiles;
    [SerializeField] private GameObject[] enemyTiles;
    [SerializeField] private GameObject alphabetTile;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject exit;

    #endregion

    #region Properties

    public GameObject[] WallTiles => wallTiles;
    public GameObject[] FoodTiles => foodTiles;
    public GameObject[] EnemyTiles => enemyTiles;
    public GameObject AlphabetTile => alphabetTile;
    public GameObject Player => player;
    public GameObject Exit => exit;

    #endregion
}
