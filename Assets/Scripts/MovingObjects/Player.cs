using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Player : MovingObject
{
    #region Enums

    private enum DisableMovementReason
    {
        IsCurrentlyMoving,
        MovementButtonIsAlreadyPressed,
        IsNotPlayerTurn,
        IsLevelTransition,
        IsPopupActive,
        IsLevelPaused
    }

    #endregion

    #region Variables

    private const string FOOD_KEY = "FoodPersist";

    [SerializeField] private int startingFood = 200;
    [SerializeField] private int wallDamage = 1;
    [SerializeField] private float moveSfxVolume = 0.3f;

    [SerializeField] private AudioClip moveSound1;
    [SerializeField] private AudioClip moveSound2;
    [SerializeField] private AudioClip playerDeath;
    [SerializeField] private AudioClip collideWithEnemy;

    [SerializeField] protected TextMeshProUGUI turnText;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private int wrongLetterDamage = 4;
    [SerializeField] private int foodAdditionPerLevel = 50;
    [SerializeField] protected LevelInfo levelInfo;

    [SerializeField] private EventNoParam OnWrongLetter;
    [SerializeField] private EventObject OnPlayerSpawned;
    [SerializeField] private EventNoParam OnLevelClearEndless;
    [SerializeField] private EventNoParam OnGameOver;
    [SerializeField] private EventNoParam OnLevelSetupStarted;
    [SerializeField] private EventNoParam OnLevelSetupEnded;
    [SerializeField] private EventNoParam OnLevelResumed;
    [SerializeField] private EventNoParam OnLevelPaused;
    [SerializeField] private EventNoParam OnEnemyTurn;
    [SerializeField] private EventNoParam OnPlayerTurn;
    [SerializeField] private EventNoParam OnDecreasePlayerTurn;
    [SerializeField] private EventNoParam OnPopupEnabled;
    [SerializeField] private EventNoParam OnPopupDisabled;
    [SerializeField] private EventObject OnDecreasePlayerTurnAfterHittingEnemy;

    private readonly HashSet<DisableMovementReason> movementDisablers = new();
    private Animator animator = null;
    private SpriteRenderer spriteRenderer = null;
    private FoodDisplay foodDisplay = null;
    private int food = 0;
    private bool foodCollided = false;
    private bool gameOver = false;

    private bool MovementDisabled => movementDisablers.Count > 0;
    public TextMeshProUGUI TurnText => turnText;

    #endregion

    #region Start

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (PlayerPrefs.HasKey(FOOD_KEY))
        {
            food = PlayerPrefs.GetInt(FOOD_KEY);
            PlayerPrefs.DeleteKey(FOOD_KEY);
        }
        else food = startingFood;

        foodDisplay = FindObjectOfType<FoodDisplay>();
        foodDisplay.UpdateText(food);

        OnPlayerSpawned.Invoke(this);

        AddFoodPerLevel();

        base.Start();
    }

    protected virtual void AddFoodPerLevel()
    {
        if (levelInfo.StageNumber <= 1) return;

        food += foodAdditionPerLevel;
        foodDisplay.UpdateTextWithIncrement(foodAdditionPerLevel, food);
    }

    #endregion

    #region Enable/Disable

    private void OnEnable()
    {
        OnWrongLetter.AddListener(DecreaseFoodOnWrongLetter);
        OnLevelSetupStarted.AddListener(LevelSetupStateEnabled);
        OnLevelSetupEnded.AddListener(LevelSetupStateDisabled);
        OnEnemyTurn.AddListener(EnemyTurnStarted);
        OnPlayerTurn.AddListener(PlayerTurnStarted);
        OnLevelResumed.AddListener(LevelResumed);
        OnLevelPaused.AddListener(LevelPaused);
        OnPopupDisabled.AddListener(PopupDisabled);
        OnPopupEnabled.AddListener(PopupEnabled);

        movementDisablers.Add(DisableMovementReason.IsLevelTransition);
        movementDisablers.Add(DisableMovementReason.IsPopupActive);
    }

    private void OnDisable()
    {
        OnWrongLetter.RemoveListener(DecreaseFoodOnWrongLetter);
        OnLevelSetupStarted.RemoveListener(LevelSetupStateEnabled);
        OnLevelSetupEnded.RemoveListener(LevelSetupStateDisabled);
        OnEnemyTurn.RemoveListener(EnemyTurnStarted);
        OnPlayerTurn.RemoveListener(PlayerTurnStarted);
        OnLevelResumed.RemoveListener(LevelResumed);
        OnLevelPaused.RemoveListener(LevelPaused);
        OnPopupDisabled.RemoveListener(PopupDisabled);
        OnPopupEnabled.RemoveListener(PopupEnabled);
    }

    #endregion

    #region Movement Update

    private void Update()
    {
        PlayerMovement();

        CheckMovementButton();
    }

    private void PlayerMovement()
    {
        if (MovementDisabled) return;

        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0 && vertical != 0)
        {
            movementDisablers.Add(DisableMovementReason.MovementButtonIsAlreadyPressed);

            vertical = 0;
            horizontal = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            movementDisablers.Add(DisableMovementReason.MovementButtonIsAlreadyPressed);

            if(horizontal != 0)
                spriteRenderer.flipX = horizontal < 0;
            
            AttemptMove(horizontal, vertical);
        }
    }

    private void CheckMovementButton()
    {
        var HorizontalPressed = Input.GetButton("Horizontal");
        var VerticalPressed = Input.GetButton("Vertical");
        
        if (!HorizontalPressed && !VerticalPressed &&
            movementDisablers.Contains(DisableMovementReason.MovementButtonIsAlreadyPressed))
            movementDisablers.Remove(DisableMovementReason.MovementButtonIsAlreadyPressed);
    }

    #endregion

    #region Movement Disabler

    private void LevelSetupStateEnabled()
    {
        movementDisablers.Add(DisableMovementReason.IsLevelTransition);
    }
    private void LevelSetupStateDisabled()
    {
        movementDisablers.Remove(DisableMovementReason.IsLevelTransition);
    }

    private void LevelPaused()
    {
        movementDisablers.Add(DisableMovementReason.IsLevelPaused);
    }
    private void LevelResumed()
    {
        movementDisablers.Remove(DisableMovementReason.IsLevelPaused);
    }

    private void EnemyTurnStarted()
    {
        movementDisablers.Add(DisableMovementReason.IsNotPlayerTurn);
    }
    private void PlayerTurnStarted()
    {
        movementDisablers.Remove(DisableMovementReason.IsNotPlayerTurn);
    }

    private void PopupEnabled()
    {
        movementDisablers.Add(DisableMovementReason.IsPopupActive);
    }
    private void PopupDisabled()
    {
        movementDisablers.Remove(DisableMovementReason.IsPopupActive);
    }

    #endregion

    #region On Move Callback

    protected override void OnMoveStarted(Vector3 endPos)
    {
        movementDisablers.Add(DisableMovementReason.IsCurrentlyMoving);
        CheckInteractables(endPos);
        DecrementFood();

        if (foodCollided)
        {
            foodCollided = false; 
            return;
        }

        if(!gameOver)
            SoundManager.instance.RandomizeSfx(moveSfxVolume, moveSound1, moveSound2);
    }

    protected override void OnMoveEnded()
    {
        if (Alphabet.AllLettersCorrect && !gameOver)
        {
            LevelClear();
            enabled = false;
        }

        OnDecreasePlayerTurn.Invoke();
        movementDisablers.Remove(DisableMovementReason.IsCurrentlyMoving);
    }

    #endregion

    #region Interactables

    private void CheckInteractables(Vector3 end)
    {
        Vector2 start = new((int)transform.position.x, (int)transform.position.y);

        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(start, end, interactableLayer);
        boxCollider.enabled = true;

        if (hit.transform != null)
            OnCollideWithInteractable(hit.transform);
    }

    private void OnCollideWithInteractable(Transform interactable)
    {
        if (interactable.CompareTag("Food"))
        {
            Food foodItem = interactable.GetComponent<Food>();

            foodCollided = true;
            int randPoints = Random.Range(foodItem.Points.Min, foodItem.Points.Max + 1);
            food += randPoints;
            foodDisplay.UpdateTextWithIncrement(randPoints, food);
            SoundManager.instance.RandomizeSfx(foodItem.EatSound1, foodItem.EatSound2);
            interactable.gameObject.SetActive(false);
        }
    }

    #endregion

    #region On Cant Move

    protected override bool OnCantMove(Transform hitTransform)
    {
        hitTransform.TryGetComponent(out Wall hitWall);
        if (hitWall != null)
        {
            hitWall.DamageWall(wallDamage);

            animator.SetTrigger("playerChop");

            DecrementFood();
            OnDecreasePlayerTurn.Invoke();
        }

        hitTransform.TryGetComponent(out Enemy hitEnemy);
        if (hitEnemy != null)
        {
            SoundManager.instance.PlaySingle(collideWithEnemy);
            hitEnemy.PlayEnemyAttackAnim();
            Enemy.AddTotalDamageThisTurn(hitEnemy.Damage);
            DecreaseFood(hitEnemy.Damage, Enemy.TotalDamageThisTurn);

            bool turnExist = OnDecreasePlayerTurnAfterHittingEnemy.Invoke(hitEnemy);

            if (!turnExist)
                Enemy.ResetTotalDamageThisTurn();
        }

        hitTransform.TryGetComponent(out Alphabet hitAlphabet);
        if (hitAlphabet != null)
        {
            Vector3 direction = hitTransform.position - transform.position;
            bool canPush = hitAlphabet.AttemptMoveTile((int)direction.x, (int)direction.y);

            if(canPush) StartCoroutine(SmoothMovement(transform.position + direction));
        }

        return true;
    }

    #endregion

    #region Level Clear

    protected virtual void LevelClear()
    {
        PlayerPrefs.SetInt(FOOD_KEY, food);
        OnLevelClearEndless.Invoke();
    }

    #endregion

    #region Decrease Food

    public void DecreaseFood(int damage, int totalDamage)
    {
        animator.SetTrigger("playerHit");
        food -= damage;
        food = Mathf.Max(0, food);
        foodDisplay.UpdateTextWithDecrement(totalDamage, food);
        CheckGameOver();
    }

    private void DecrementFood()
    {
        if (foodCollided) return;

        food--;
        food = Mathf.Max(0, food);
        foodDisplay.UpdateText(food);
        CheckGameOver();
    }

    private void DecreaseFoodOnWrongLetter()
    {
        SoundManager.instance.PlaySingle(collideWithEnemy);
        animator.SetTrigger("playerHit");
        food -= wrongLetterDamage;
        food = Mathf.Max(0, food);
        //the plus 1 is to also show the food decrease because of turn
        foodDisplay.UpdateTextWithDecrement(wrongLetterDamage + 1, food);
        CheckGameOver();
    }

    #endregion

    #region Check Game Over

    private void CheckGameOver()
    {
        if (food > 0) return;

        SoundManager.instance.StopMusic();
        SoundManager.instance.PlaySingle(playerDeath);
        animator.SetTrigger("playerDeath");

        gameOver = true;
        OnGameOver.Invoke();
        enabled = false;
    }

    #endregion

    #region Debug

    [ContextMenu("Kill")]
    private void Kill()
    {
        food = 0;
        CheckGameOver();
    }

    [ContextMenu("Dying")]
    private void Dying()
    {
        DecreaseFood(food - 1, food - 1);
    }

    #endregion
}
