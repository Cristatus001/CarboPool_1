using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SnakeController : MonoBehaviour
{
    [Header("Snake")]
    public GameObject snakePrefab;

    public float moveSpeed = 3f;
    public float turnSmooth = 8f;

    [Tooltip("Wave strength for the slither motion.")]
    public float slitherAmount = 12f;

    [Tooltip("Wave speed for the slither motion.")]
    public float slitherSpeed = 10f;

    [Tooltip("Distance used to detect food near the head.")]
    public float eatRadius = 0.18f;

    public LayerMask ballLayer;

    [Header("Snake Entry / Exit")]
    public float entryX = -6f;
    public float exitX = 6f;
    public float entryY = -3.5f;
    public float exitY = 3.5f;

    [Header("Rig")]
    public Transform headBone;

    private Vector2 moveDirection = Vector2.right;
    private Vector2 targetDirection = Vector2.right;

    private bool isMoving = false;
    private bool isStopped = false;
    private bool isEatingLocked = false;

    private GameObject snakeObject;
    private Transform snakeTransform;

    private readonly List<Transform> bones = new List<Transform>();
    private Quaternion[] boneBaseRotations;
    private float moveTime = 0f;

    private Vector2 swipeStart;
    private bool isSwiping = false;
    private float minSwipeDistance = 50f;

    public void Initialize()
    {
        CreateSnake();
        isMoving = true;
    }

    void CreateSnake()
    {
        float x = Random.Range(-3f, 3f);
        float y = Random.Range(-2f, 2f);
        Vector3 startPos = new Vector3(x, y, 0f);

        if (snakePrefab == null)
        {
            Debug.LogError("Snake prefab is not assigned in SnakeController.");
            return;
        }

        snakeObject = Instantiate(snakePrefab, startPos, Quaternion.Euler(0f, 0f, - 90f));
        snakeObject.name = "Snake";
        snakeTransform = snakeObject.transform;

        CacheBones();
    }

    void CacheBones()
    {
        bones.Clear();
        headBone = null;

        Transform[] allChildren = snakeObject.GetComponentsInChildren<Transform>(true);

        foreach (Transform t in allChildren)
        {
            if (t == snakeObject.transform)
                continue;

            // Bones in a SpriteSkin rig are usually child transforms without SpriteRenderer.
            if (t.GetComponent<SpriteRenderer>() != null)
                continue;

            bones.Add(t);

            if (headBone == null)
            {
                string lower = t.name.ToLower();
                if (lower.Contains("head"))
                    headBone = t;
            }
        }

        if (bones.Count == 0)
        {
            // Fallback: include all children if the rig setup is unusual.
            foreach (Transform t in allChildren)
            {
                if (t == snakeObject.transform)
                    continue;

                bones.Add(t);
            }
        }

        if (headBone == null && bones.Count > 0)
        {
            headBone = bones[bones.Count - 1];
        }

        boneBaseRotations = new Quaternion[bones.Count];
        for (int i = 0; i < bones.Count; i++)
        {
            boneBaseRotations[i] = bones[i].localRotation;
        }
    }

    void Update()
    {
        if (isStopped || !isMoving)
            return;

        HandleInput();
        MoveSnake();
        CheckBallEating();
    }

    void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                swipeStart = touch.position;
                isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Ended && isSwiping)
            {
                ProcessSwipe(swipeStart, touch.position);
                isSwiping = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            swipeStart = Input.mousePosition;
            isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            ProcessSwipe(swipeStart, Input.mousePosition);
            isSwiping = false;
        }
    }

    void ProcessSwipe(Vector2 start, Vector2 end)
    {
        Vector2 delta = end - start;
        if (delta.magnitude < minSwipeDistance)
            return;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            Vector2 newDir = delta.x > 0 ? Vector2.right : Vector2.left;
            targetDirection = newDir;
        }
        else
        {
            Vector2 newDir = delta.y > 0 ? Vector2.up : Vector2.down;
            targetDirection = newDir;
        }
    }

    void MoveSnake()
    {
        // Smooth turning
        moveDirection = Vector2.Lerp(
            moveDirection,
            targetDirection,
            Time.deltaTime * turnSmooth
        );

        if (moveDirection.sqrMagnitude > 0.0001f)
            moveDirection.Normalize();
        else
            moveDirection = targetDirection;

        moveTime += Time.deltaTime;

        Vector3 nextPos =
            snakeTransform.position +
            (Vector3)moveDirection * moveSpeed * Time.deltaTime;

        nextPos = WrapPosition(nextPos);
        snakeTransform.position = nextPos;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        snakeTransform.rotation = Quaternion.Euler(0, 0, angle);

        // Actual slither: wiggle the bones, not just the root.
        if (bones.Count > 0 && boneBaseRotations != null && boneBaseRotations.Length == bones.Count)
        {
            for (int i = 0; i < bones.Count; i++)
            {
                float t = moveTime * slitherSpeed - (i * 0.45f);
                float falloff = Mathf.Lerp(1f, 0.15f, i / Mathf.Max(1f, bones.Count - 1f));
                float wave = Mathf.Sin(t) * slitherAmount * falloff;

                bones[i].localRotation = boneBaseRotations[i] * Quaternion.Euler(0f, 0f, wave);
            }
        }
    }

    void CheckBallEating()
    {
        if (isEatingLocked)
            return;

        if (headBone == null)
            return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            headBone.position,
            eatRadius,
            ballLayer
        );

        foreach (Collider2D hit in hits)
        {
            Ball ball = hit.GetComponent<Ball>();
            if (ball != null && !ball.isPotted)
            {
                ball.isPotted = true;
                isEatingLocked = true;

                if (SnakeGameManager.Instance != null)
                {
                    SnakeGameManager.Instance.BallEaten(
                        hit.gameObject,
                        ball.fragmentType
                    );
                }

                break;
            }
        }
    }

    // No body attachment anymore. Ball disappears immediately after a correct eat.
    public void AddBallToBody(GameObject ball)
    {
        if (ball != null)
            Destroy(ball);

        isEatingLocked = false;
    }

    public void UnlockEating()
    {
        isEatingLocked = false;
    }

    public IEnumerator BlinkSnake()
    {
        if (snakeObject == null)
            yield break;

        SpriteRenderer[] srs = snakeObject.GetComponentsInChildren<SpriteRenderer>(true);

        for (int i = 0; i < 4; i++)
        {
            foreach (var sr in srs)
                if (sr != null) sr.enabled = false;

            yield return new WaitForSeconds(0.12f);

            foreach (var sr in srs)
                if (sr != null) sr.enabled = true;

            yield return new WaitForSeconds(0.12f);
        }

        UnlockEating();
    }

    public void StopSnake()
    {
        isStopped = true;
        isMoving = false;
    }

    Vector3 WrapPosition(Vector3 pos)
    {
        if (pos.x > exitX) pos.x = entryX;
        if (pos.x < entryX) pos.x = exitX;
        if (pos.y > exitY) pos.y = entryY;
        if (pos.y < entryY) pos.y = exitY;

        return pos;
    }

    void OnDrawGizmos()
    {
        Transform gizmoHead = headBone;

        if (gizmoHead == null && Application.isPlaying)
            gizmoHead = headBone;

        if (gizmoHead == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gizmoHead.position, eatRadius);
    }
}