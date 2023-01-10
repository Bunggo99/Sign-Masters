using UnityEngine;

public abstract class LevelLayout : MonoBehaviour
{
    #region Variables

    private Transform objectHolder;
    private BoardManager boardManager;

    #endregion

    #region Layout Objects Base

    public virtual void LayoutObjects(BoardManager board)
    {
        objectHolder = new GameObject("Objects").transform;
        boardManager = board;
    }

    #endregion

    #region Functions to Layout An Object

    protected void LayoutTilesAtRandom(GameObject[] tilesArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = boardManager.GetRandomGridPos();
            GameObject chosenTile = tilesArray[Random.Range(0, tilesArray.Length)];
            Instantiate(chosenTile, randomPosition, Quaternion.identity, objectHolder);
        }
    }

    protected void LayoutAlphabetAtRandom(GameObject alphabetTile)
    {
        string word = Alphabet.RandomWordThisStage;

        for (int i = 0; i < word.Length; i++)
        {
            Vector3 randomPosition = boardManager.GetRandomGridPos();
            GameObject obj = Instantiate(alphabetTile, randomPosition, Quaternion.identity, objectHolder);
            obj.GetComponent<Alphabet>().Setup(word[i]);
        }
    }

    protected GameObject LayoutExitAtRandomOuterPos(GameObject exit)
    {
        return Instantiate(exit, boardManager.GetRandomOuterPos(), Quaternion.identity, objectHolder);
    }

    protected void LayoutPlayerAcrossExit(GameObject player, Transform exit)
    {
        Instantiate(player, boardManager.GetPlayerPosAcrossExit(exit.position), Quaternion.identity, objectHolder);
    }

    #endregion
}
