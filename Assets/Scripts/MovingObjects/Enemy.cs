using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MovingObject
{
    #region Variables

    private static int totalDamageThisTurn = 0;
    private static readonly List<Enemy> enemyStackingList = new();

    [SerializeField] private int damage;
    [SerializeField] private float moveDelay = 0.1f;
    [SerializeField] private float attackDelay = 0.5f;
    [SerializeField] private AudioClip enemyAttack1;
    [SerializeField] private AudioClip enemyAttack2;
    [SerializeField] private GameObject textCanvas;

    [SerializeField] private EventObject OnEnemySpawned;
    [SerializeField] private EventObject OnEnemyDeleted;
    [SerializeField] private EventNoParam OnEnemyHasMoved;
    [SerializeField] private EventNoParam OnComputerVisionSceneLoading;
    [SerializeField] private EventObject OnEnemyInteracted;
    [SerializeField] private EventInt OnEnemyAttacked;
    [SerializeField] private EventNoParam OnEnemyKilled;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform target;

    public static int TotalDamageThisTurn => totalDamageThisTurn;
    public int Damage => damage;
    public float AttackDelay => attackDelay;

    #endregion

    #region Start

    protected override void Start()
    {
        OnEnemySpawned.Invoke(this);

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        base.Start();
    }

    #endregion

    #region Move Enemy

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        float distanceInYAxis = Mathf.Abs(target.position.y - transform.position.y);
        if (distanceInYAxis > float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        if (target.position.x != transform.position.x)
            spriteRenderer.flipX = target.position.x < transform.position.x;

        AttemptMove(xDir, yDir);
    }

    private void MoveEnemyHorizontal()
    {
        int xDir = 0;

        float distanceInXAxis = Mathf.Abs(target.position.x - transform.position.x);
        if (distanceInXAxis > float.Epsilon)
            xDir = target.position.x > transform.position.x ? 1 : -1;

        if (target.position.x != transform.position.x)
            spriteRenderer.flipX = target.position.x < transform.position.x;

        AttemptMove(xDir, 0);
    }

    #endregion

    #region On Cant Move

    protected override bool OnCantMove(Transform hitTransform)
    {
        hitTransform.TryGetComponent(out Player hitPlayer);

        if (hitPlayer)
        {
            DamagePlayer(hitPlayer);
        }
        else
        {
            float yDistanceWithHit = Mathf.Abs(hitTransform.position.y - transform.position.y);
            if (yDistanceWithHit > float.Epsilon)
                MoveEnemyHorizontal();
            else
                EnemyHasMovedCallback();
        }
        return true;
    }

    private void DamagePlayer(Player hitPlayer)
    {
        SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);

        totalDamageThisTurn += damage;

        hitPlayer.DecreaseFood(damage, totalDamageThisTurn);

        PlayEnemyAttackAnim();

        Invoke(nameof(EnemyHasMovedCallback), attackDelay);
    }

    #endregion

    #region On Move Callbacks

    protected override void OnMoveStarted(Vector3 endPos) { }

    protected override void OnMoveEnded(Vector3 endPos)
    {
        CheckEnemyOverlap();

        Invoke(nameof(EnemyHasMovedCallback), moveDelay);
    }

    #endregion

    #region Check Enemy Overlap

    private void CheckEnemyOverlap()
    {
        Vector2 pos = new((int)transform.position.x, (int)transform.position.y);

        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(pos, pos, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform != null && hit.transform.CompareTag("Enemy"))
            OnOverlapWithOtherEnemy(this);
    }

    private static void OnOverlapWithOtherEnemy(Enemy currEnemy)
    {
        bool existsSamePos = false;
        // if there are no other enemy with the same position
        for (int i = 0; i < enemyStackingList.Count; i++)
        {
            Enemy enemy = enemyStackingList[i];
            if (Vector2.Distance(currEnemy.transform.position,
                enemy.transform.position) < Mathf.Epsilon)
            {
                existsSamePos = true;

                if (currEnemy.damage > enemy.damage)
                {
                    enemyStackingList.RemoveAt(i);
                    enemyStackingList.Add(currEnemy);

                    enemy.DeleteEnemy();

                    return;
                }
            }
        }

        if (!existsSamePos) enemyStackingList.Add(currEnemy);
        else
        {
            currEnemy.DeleteEnemy();
        }
    }

    private void DeleteEnemy()
    {
        gameObject.SetActive(false);
        OnEnemyDeleted.Invoke(this);
    }

    #endregion

    #region Enemy Has Moved Callback

    private void EnemyHasMovedCallback()
    {
        OnEnemyHasMoved.Invoke();
    }

    #endregion

    #region Public Methods

    public void PlayEnemyAttackAnim()
    {
        animator.SetTrigger("enemyAttack");
    }

    public void SetActiveCanvas(bool enabled)
    {
        textCanvas.SetActive(enabled);
    }

    public void LoadComputerVisionScene()
    {
        OnComputerVisionSceneLoading.Invoke();
        OnEnemyInteracted.Invoke(this);
        SceneManager.LoadSceneAsync("ComputerVision", LoadSceneMode.Additive);
    }

    public void EnemyBattled(bool success)
    {
        if (success)
        {
            Destroy(gameObject);
            OnEnemyKilled.Invoke();
        }
        else
        {
            SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
            animator.SetTrigger("enemyAttack");
            OnEnemyAttacked.Invoke(damage);
        }
    }

    #endregion

    #region Static Functions

    public static void ResetTotalDamageThisTurn()
    {
        totalDamageThisTurn = 0;
    }

    public static void AddTotalDamageThisTurn(int dmg)
    {
        totalDamageThisTurn += dmg;
    }

    public static void ClearEnemyStackingList()
    {
        enemyStackingList.Clear();
    }

    #endregion
}
