using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManager : SingletonManager<TurnManager>
{
    #region Variables

    [SerializeField] private int playerTurnAmount = 7;
    [SerializeField] private int enemyTurnAmount = 3;

    [SerializeField] private EventObject OnEnemySpawned;
    [SerializeField] private EventObject OnEnemyDeleted;
    [SerializeField] private EventObject OnPlayerSpawned;
    [SerializeField] private EventNoParam OnEnemyTurn;
    [SerializeField] private EventNoParam OnPlayerTurn;
    [SerializeField] private EventNoParam OnEnemyHasMoved;
    [SerializeField] private EventNoParam OnDecreasePlayerTurn;
    [SerializeField] private EventObject OnDecreasePlayerTurnAfterHittingEnemy;
    [SerializeField] private EventNoParam OnGameEnded;

    private int remainingPlayerTurn = 0;
    private TextMeshProUGUI playerTurnText;
    private int remainingEnemyTurn = 0;

    private readonly List<Enemy> enemies = new();
    private int movingEnemies = 0;

    private bool gameEnded = false;

    #endregion

    #region Properties

    private int RemainingPlayerTurn
    {
        get => remainingPlayerTurn;
        set
        {
            remainingPlayerTurn = value;
            if (playerTurnText != null)
                playerTurnText.text = remainingPlayerTurn.ToString();
        }
    }

    #endregion

    #region Start

    private void Start()
    {
        RemainingPlayerTurn = playerTurnAmount;
        remainingEnemyTurn = 0;
    }

    #endregion

    #region Enable/Disable

    private void OnEnable()
    {
        OnEnemySpawned.AddListener(AddEnemyToList);
        OnEnemyDeleted.AddListener(RemoveEnemyFromList);
        OnPlayerSpawned.AddListener(AssignPlayerTurnText);
        OnEnemyHasMoved.AddListener(DecreaseMovingEnemyCount);
        OnDecreasePlayerTurn.AddListener(DecreasePlayerTurn);
        OnDecreasePlayerTurnAfterHittingEnemy.AddListener(DecreasePlayerTurnAfterHittingEnemy);
        OnGameEnded.AddListener(GameEnded);
    }

    private void OnDisable()
    {
        OnEnemySpawned.RemoveListener(AddEnemyToList);
        OnEnemyDeleted.RemoveListener(RemoveEnemyFromList);
        OnPlayerSpawned.RemoveListener(AssignPlayerTurnText);
        OnEnemyHasMoved.RemoveListener(DecreaseMovingEnemyCount);
        OnDecreasePlayerTurn.RemoveListener(DecreasePlayerTurn);
        OnDecreasePlayerTurnAfterHittingEnemy.RemoveListener(DecreasePlayerTurnAfterHittingEnemy);
        OnGameEnded.RemoveListener(GameEnded);
    }

    #endregion

    #region Player and Enemy Init

    private void AssignPlayerTurnText(object player)
    {
        playerTurnText = ((Player)player).TurnText;
        playerTurnText.text = remainingPlayerTurn.ToString();
    }

    private void AddEnemyToList(object enemy)
    {
        enemies.Add((Enemy)enemy);
    }
    private void RemoveEnemyFromList(object enemy)
    {
        enemies.Remove((Enemy)enemy);
    }

    #endregion

    #region Decrease Player Turn

    private void DecreasePlayerTurn()
    {
        RemainingPlayerTurn--;
        
        if (RemainingPlayerTurn <= 0) 
            SwapToEnemyTurn();
    }

    //if this is the last turn of the player, 
    //and the player hit the enemy, then delay the 
    //turn change so that the enemy can finish it's
    //attack animation first
    private void DecreasePlayerTurnAfterHittingEnemy(object enemy)
    {
        RemainingPlayerTurn--;

        if (RemainingPlayerTurn <= 0)
        {
            OnEnemyTurn.Invoke();
            Invoke(nameof(SwapToEnemyTurn), ((Enemy)enemy).AttackDelay);
        }
    }

    #endregion

    #region Swap Turns

    private void SwapToPlayerTurn()
    {
        if (gameEnded) return;

        RemainingPlayerTurn = playerTurnAmount;
        OnPlayerTurn.Invoke();
    }

    private void SwapToEnemyTurn()
    {
        if (gameEnded) return;

        if (remainingPlayerTurn > 0) return;
        else if (enemies.Count == 0)
        {
            SwapToPlayerTurn();
            return;
        }

        remainingEnemyTurn = enemyTurnAmount;
        OnEnemyTurn.Invoke();
        StartCoroutine(MoveEnemies());
    }

    #endregion

    #region Move Enemies

    private IEnumerator MoveEnemies()
    {
        while (remainingEnemyTurn > 0)
        {
            movingEnemies = enemies.Count;
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].MoveEnemy();
            }
            while (movingEnemies > 0) yield return null;
            remainingEnemyTurn--;
            Enemy.ClearEnemyStackingList();
        }
        Enemy.ResetTotalDamageThisTurn();
        SwapToPlayerTurn();
    }

    private void DecreaseMovingEnemyCount()
    {
        if (movingEnemies > 0) movingEnemies--;
    }

    #endregion

    #region Game Ended

    private void GameEnded()
    {
        gameEnded = true;
    }

    #endregion
}
