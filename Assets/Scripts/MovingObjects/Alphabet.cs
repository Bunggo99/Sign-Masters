using System.Collections;
using TMPro;
using UnityEngine;

public class Alphabet : MovingObject
{
    #region Variables

    private static string randomWordThisStage = "";
    private static int correctLetterCount = 0;

    public static string RandomWordThisStage => randomWordThisStage;
    public static int CorrectLetterCount => correctLetterCount;
    public static bool AllLettersCorrect =>
        correctLetterCount >= randomWordThisStage.Length;

    [SerializeField] private Animation anim;
    [SerializeField] private TextMeshProUGUI alphabetText;
    [SerializeField] private EventNoParam OnWrongLetter;
    [SerializeField] private AudioClip correctLetterSfx;
    [SerializeField] private EventTransform OnRandomizeObjectPosition;
    [SerializeField] private EventNoParam OnGameOver;

    private char letter;
    private bool gameOver = false;

    #endregion

    #region Setup

    public void Setup(char letter)
    {
        alphabetText.text = letter.ToString();
        this.letter = letter;
    }

    #endregion

    #region Enable/Disable

    private void OnEnable()
    {
        OnGameOver.AddListener(GameOver);
    }

    private void OnDisable()
    {
        OnGameOver.RemoveListener(GameOver);
    }

    private void GameOver()
    {
        gameOver = true;
    }

    #endregion

    #region Attempt Move

    public bool AttemptMoveTile(int xDir, int yDir)
    {
        return AttemptMove(xDir, yDir);
    }

    #endregion

    #region On Cant Move

    protected override bool OnCantMove(Transform hitTransform)
    {
        Alphabet hitAlphabet = hitTransform.GetComponent<Alphabet>();
        if (hitAlphabet != null)
        {
            Vector3 direction = hitTransform.position - transform.position;
            bool canPush = hitAlphabet.AttemptMoveTile((int)direction.x, (int)direction.y);

            if (canPush) StartCoroutine(SmoothMovement(transform.position + direction));

            return canPush;
        }
        else if(hitTransform.CompareTag("Water") ||
                hitTransform.CompareTag("Exit"))
        {
            Vector3 direction = hitTransform.position - transform.position;
            StartCoroutine(SmoothMovement(transform.position + direction));
            return true;
        }
        return false;
    }

    #endregion

    #region On Move Callbacks

    protected override void OnMoveStarted(Vector3 endPos) { }

    protected override void OnMoveEnded()
    {
        CheckBlockingOverlap();
    }

    #endregion

    #region Check Blocking Overlap

    private void CheckBlockingOverlap()
    {
        Vector2 pos = new((int)transform.position.x, (int)transform.position.y);

        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(pos, pos, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform != null && hit.transform.CompareTag("Exit"))
            OnOverlapWithExit();
        else if (hit.transform != null && hit.transform.CompareTag("Water"))
            StartCoroutine(RandomizePosition());
    }

    private void OnOverlapWithExit()
    {
        bool letterCorrect = (letter == randomWordThisStage[correctLetterCount]);
        if (letterCorrect)
        {
            correctLetterCount++;
            
            var display = FindObjectOfType<SignLanguageDisplay>();
            if (display != null)
            {
                display.UpdateColoredWordDisplay();
                display.ShiftArrow(AllLettersCorrect);

                if(!gameOver)
                    SoundManager.instance.PlaySingleShot(correctLetterSfx);
            }

            StartCoroutine(DisableObject());
        }
        else
        {
            OnWrongLetter.Invoke();
            StartCoroutine(RandomizePosition());
        }
    }

    private IEnumerator RandomizePosition()
    {
        anim.Play("AlphabetShrink");
        yield return new WaitForSeconds(anim.clip.length);

        bool teleported = OnRandomizeObjectPosition.Invoke(transform);

        if (teleported) anim.Play("AlphabetExpand");
        else gameObject.SetActive(false);
    }

    private IEnumerator DisableObject()
    {
        anim.Play("AlphabetShrink");
        yield return new WaitForSeconds(anim.clip.length);
        gameObject.SetActive(false);
    }

    #endregion

    #region Static Functions

    public static void ResetCorrectLetterCount()
    {
        correctLetterCount = 0;
    }

    public static void SetRandomWordThisStage(string word)
    {
        randomWordThisStage = word;
    }

    #endregion
}
