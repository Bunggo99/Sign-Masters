using UnityEngine;

public class Wall : MonoBehaviour
{
    #region Variables

    [SerializeField] private Sprite dmgSprite;
    [SerializeField] private int hp = 3;
    [SerializeField] private AudioClip chopSound1;
    [SerializeField] private AudioClip chopSound2;

    private SpriteRenderer spriteRenderer;

    #endregion

    #region Awake

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    #endregion

    #region Damage Wall

    public void DamageWall(int damage)
    {
        hp -= damage;

        spriteRenderer.sprite = dmgSprite;
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);

        if (hp <= 0) gameObject.SetActive(false);
    }

    #endregion
}
