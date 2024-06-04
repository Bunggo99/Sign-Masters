using System.Collections;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    #region Variables

    [SerializeField] protected float moveDuration = 0.1f;
    [SerializeField] protected LayerMask blockingLayer;
    [SerializeField] protected EventTwoVector3 OnPositionUpdated;

    protected BoxCollider2D boxCollider;
    protected bool isMoving;
    private float moveSpeed;

    #endregion

    #region Start

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        if (moveDuration == 0f) moveDuration = 0.1f;
        moveSpeed = 1f / moveDuration;
    }

    #endregion

    #region Attempt Move

    protected bool AttemptMove(int xDir, int yDir)
    {
        Vector2 start = new((int)transform.position.x, (int)transform.position.y);
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        else
        {
            return OnCantMove(hit.transform);
        }
    }

    #endregion

    #region Smooth Movement

    protected IEnumerator SmoothMovement(Vector3 endPos)
    {
        isMoving = true;
        OnMoveStarted(endPos);
        OnPositionUpdated.Invoke(transform.position, endPos);

        float sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);
            transform.position = newPos;
            sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;
            yield return null;
        }

        transform.position = endPos;
        OnMoveEnded(endPos);
        isMoving = false;
    }

    #endregion

    #region Move Callbacks

    protected abstract bool OnCantMove(Transform hitTransform);
    protected abstract void OnMoveStarted(Vector3 endPos);
    protected abstract void OnMoveEnded(Vector3 endPos);

    #endregion
}
