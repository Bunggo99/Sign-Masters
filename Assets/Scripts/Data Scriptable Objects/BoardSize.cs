using UnityEngine;

[CreateAssetMenu(fileName = "Board Size", menuName = "SO/Board Size")]
public class BoardSize : ScriptableObject
{
    #region Variables

    [SerializeField] private int columns = 12;
    [SerializeField] private int rows = 8;

    #endregion

    #region Properties

    public int Columns => columns;
    public int Rows => rows;

    #endregion
}
