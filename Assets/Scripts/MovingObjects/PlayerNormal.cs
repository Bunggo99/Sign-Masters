using UnityEngine;

public class PlayerNormal : Player
{
    //[SerializeField] private int stageToEnableTurnText = 7;
    [SerializeField] private EventNoParam OnWinGame;

    protected override void Start()
    {
        base.Start();

        //if(levelInfo.StageNumber < stageToEnableTurnText)
        //turnText.gameObject.SetActive(false);
    }

    protected override void AddFoodPerLevel() { }

    protected override void OnEnable()
    {
        base.OnEnable();

        movementDisablers.Add(DisableMovementReason.IsPopupActive);
    }

    protected override void LevelClear()
    {
        OnWinGame.Invoke();
    }
}
