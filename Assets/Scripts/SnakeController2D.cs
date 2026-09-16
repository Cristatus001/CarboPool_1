using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class SnakeController2D : MonoBehaviour
{
    // ─────────────────────────────────────────
    //  EDIT MODE PREVIEW
    // ─────────────────────────────────────────
    [Header("Edit Mode Preview")]
    [Tooltip("Show snake in Scene view without pressing Play.")]
    public bool    previewInEditor  = false;
    [Tooltip("Which direction the preview snake faces.")]
    public Vector2 previewDirection = Vector2.up;

    // ─────────────────────────────────────────
    //  MOVEMENT
    // ─────────────────────────────────────────
    [Header("Movement")]
    public float moveSpeed    = 4f;
    [Range(0f, 20f)]
    public float turnSmooth   = 8f;
    [Range(0f, 1f)]
    public float wiggleAmount = 0.15f;
    public float wiggleSpeed  = 6f;

    // ─────────────────────────────────────────
    //  BALL EATING
    // ─────────────────────────────────────────
    [Header("Ball Eating")]
    [Tooltip("Layer mask that contains the ball objects.")]
    public LayerMask ballLayer;
    [Tooltip("Radius around the head used to detect balls.")]
    public float eatRadius = 0.18f;

    // ─────────────────────────────────────────
    //  SWIPE / TURN CONTROL
    // ─────────────────────────────────────────
    [Header("Turn Points / Swipe Control")]
    public bool useTurnPoints = false;
    public Transform[] turnPoints;
    public float turnTriggerDistance = 0.15f;
    public bool preventReverseTurn   = true;
    public bool allowKeyboardInput   = false;
    [Tooltip("Minimum swipe distance in pixels.")]
    public float minSwipeDistance = 50f;

    // ─────────────────────────────────────────
    //  BOUNDARY WALL TRIGGERS
    // ─────────────────────────────────────────
    [System.Serializable]
    public struct WallTrigger
    {
        public enum Axis { X, Y }

        [Tooltip("X = vertical wall (left/right)\nY = horizontal wall (top/bottom)")]
        public Axis    axis;
        [Tooltip("World-space coordinate of the wall line.")]
        public float   position;
        [Tooltip("Direction to turn when the head crosses this wall.\n" +
                 "X walls → (1,0) or (-1,0)   Y walls → (0,1) or (0,-1)")]
        public Vector2 turnTo;

        // Gizmo display color (shown in Scene view)
        [Tooltip("Color used to draw this wall's gizmo in the Scene view.")]
        public Color   gizmoColor;
    }

    [Header("Boundary Wall Triggers")]
    [Tooltip("One entry per boundary wall. The snake turns to 'Turn To' when it crosses the line.\n" +
             "Each wall is drawn as a colored line in the Scene view for easy setup.")]
    public List<WallTrigger> wallTriggers = new List<WallTrigger>();

    // ─────────────────────────────────────────
    //  BODY
    // ─────────────────────────────────────────
    [Header("Body Size & Thickness")]
    public int   segments  = 80;
    public float spacing   = 0.12f;
    [Tooltip("Body shape curve — script never writes to this.")]
    public AnimationCurve bodyWidth = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    [Range(0.01f, 5f)]
    public float widthScale = 1f;

    // ─────────────────────────────────────────
    //  SCALE / HEX COLORS
    // ─────────────────────────────────────────
    [Header("Scale Colors (Hexagon Pattern)")]
    public Color[] scaleColors = new Color[]
    {
        new Color(0.10f, 0.70f, 0.10f, 1f),
        new Color(0.05f, 0.45f, 0.05f, 1f),
        new Color(0.15f, 0.60f, 0.15f, 1f),
        new Color(0.02f, 0.35f, 0.02f, 1f),
    };
    [Range(1, 16)]    public int   hexColumns      = 5;
    [Range(1, 32)]    public int   hexRows         = 12;
    [Range(0f, 1f)]   public float groutBrightness = 0.15f;
    [Range(0f, 0.5f)] public float groutThickness  = 0.08f;

    // ─────────────────────────────────────────
    //  HEAD SPRITE
    // ─────────────────────────────────────────
    [Header("Head Sprite")]
    public Sprite  headSprite;
    [Range(0.05f, 3f)] public float headSize  = 0.4f;
    public Color   headColor  = Color.white;
    [Tooltip("X = sideways, Y = forward along movement direction.")]
    public Vector2 headOffset = Vector2.zero;

    // ─────────────────────────────────────────
    //  TONGUE
    // ─────────────────────────────────────────
    [Header("Tongue")]
    public bool  tongueEnabled      = true;
    public float tongueRootOffset   = 0.05f;
    [Range(0.01f, 2f)]    public float tongueLength    = 0.25f;
    [Range(0f, 60f)]      public float tongueForkAngle = 18f;
    [Range(0.001f, 0.1f)] public float tongueWidth     = 0.012f;
    public Color tongueColor = new Color(0.85f, 0.1f, 0.1f, 1f);
    [Space]
    public float tongueRestTime     = 1.4f;
    public float tongueFlickOutTime = 0.08f;
    public float tongueHoldTime     = 0.18f;
    public float tongueFlickInTime  = 0.12f;

    // ─────────────────────────────────────────
    //  PRIVATE — general
    // ─────────────────────────────────────────
    private Rigidbody2D    rb;
    private LineRenderer   line;
    private SpriteRenderer headRenderer;

    private Vector2        direction;
    private Vector2        moveDirection;
    private Vector2        targetDirection;
    private Vector2        queuedDirection;
    private bool           turnQueued;
    private float          wiggle;
    private List<Vector3>  bodyPoints = new List<Vector3>();

    private Vector2        swipeStart;
    private bool           isSwiping;
    private Vector2        prevHeadPos;

    // State flags set by SnakeGameManager
    private bool           isStopped        = false;
    private bool           isEatingLocked   = false;

    // ─────────────────────────────────────────
    //  PRIVATE — hex texture
    // ─────────────────────────────────────────
    private Texture2D hexTex;
    private Material  bodyMat;
    private Color[]   cachedScaleColors;
    private int       cachedHexCols, cachedHexRows;
    private float     cachedGroutBrightness, cachedGroutThickness;

    // ─────────────────────────────────────────
    //  PRIVATE — tongue
    // ─────────────────────────────────────────
    private LineRenderer tongueLeft;
    private LineRenderer tongueRight;
    private enum TongueState { Rest, FlickOut, Hold, FlickIn }
    private TongueState tongueState  = TongueState.Rest;
    private float       tongueTimer  = 0f;
    private float       tongueExtend = 0f;

    // ─────────────────────────────────────────
    //  PUBLIC API (called by SnakeGameManager)
    // ─────────────────────────────────────────

    /// <summary>Called by SnakeGameManager.LoadFromGameData() to start the snake.</summary>
    public void Initialize()
    {
        isStopped      = false;
        isEatingLocked = false;
    }

    /// <summary>Ball was eaten correctly — just disable it, no attachment.</summary>
    public void AddBallToBody(GameObject ball)
    {
        if (ball != null) ball.SetActive(false);
        isEatingLocked = false;
    }

    /// <summary>Unlock eating after a wrong-ball blink cycle.</summary>
    public void UnlockEating() => isEatingLocked = false;

    /// <summary>Freeze snake on game over / win.</summary>
    public void StopSnake()
    {
        isStopped = true;
    }

    /// <summary>Blink the snake's LineRenderer 4 times (wrong-ball feedback).</summary>
    public IEnumerator BlinkSnake()
    {
        for (int i = 0; i < 4; i++)
        {
            if (line != null) line.enabled = false;
            if (headRenderer != null) headRenderer.enabled = false;
            yield return new WaitForSeconds(0.12f);

            if (line != null) line.enabled = true;
            if (headRenderer != null && headSprite != null) headRenderer.enabled = true;
            yield return new WaitForSeconds(0.12f);
        }
        UnlockEating();
    }

    // ─────────────────────────────────────────
    //  LIFECYCLE
    // ─────────────────────────────────────────
    void Awake()
    {
        GrabComponents();
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;

        if (!Application.isPlaying)
        {
            if (line != null) line.positionCount = 0;
            if (previewInEditor) BuildPreview();
        }
        else
        {
            direction       = transform.up.normalized;
            if (direction == Vector2.zero) direction = Vector2.up;
            moveDirection   = direction;
            targetDirection = direction;
            queuedDirection = direction;
            turnQueued      = false;
            prevHeadPos     = transform.position;

            InitBodyPoints(transform.position, direction);
            RebuildHexTexture();
            ApplyVisuals();
            SetupHeadChild();
            SetupTongue();
        }
    }

    void Update()
    {
        if (Application.isPlaying)
            RuntimeUpdate();
#if UNITY_EDITOR
        else if (previewInEditor)
            BuildPreview();
#endif
    }

    void FixedUpdate() { /* kinematic — movement handled in Update */ }

    // ─────────────────────────────────────────
    //  RUNTIME
    // ─────────────────────────────────────────
    void RuntimeUpdate()
    {
        if (isStopped) return;

        HandleSwipeInput();
        HandleKeyboardInput();
        TryApplyQueuedTurn();

        wiggle += Time.deltaTime * wiggleSpeed;

        if (HexSettingsChanged()) RebuildHexTexture();

        MoveSnake();
        CheckWallTriggers();
        MoveBody();
        DrawLine();
        ApplyVisuals();
        UpdateHeadChild();
        UpdateTongue();
        CheckBallEating();

        prevHeadPos = transform.position;
    }

    // ─────────────────────────────────────────
    //  BALL EATING
    // ─────────────────────────────────────────

    /// <summary>
    /// Each frame, check for balls inside eatRadius around the head.
    /// On hit, report to SnakeGameManager — the ball is never attached,
    /// it gets disabled by AddBallToBody() after the manager validates it.
    /// </summary>
    void CheckBallEating()
    {
        if (isEatingLocked) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position, eatRadius, ballLayer);

        foreach (Collider2D hit in hits)
        {
            Ball ball = hit.GetComponent<Ball>();
            if (ball != null && !ball.isPotted)
            {
                ball.isPotted  = true;
                isEatingLocked = true;

                if (SnakeGameManager.Instance != null)
                    SnakeGameManager.Instance.BallEaten(hit.gameObject, ball.fragmentType);

                break;
            }
        }
    }

    // ─────────────────────────────────────────
    //  INPUT — SWIPE
    // ─────────────────────────────────────────
    void HandleSwipeInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)       { swipeStart = touch.position; isSwiping = true; }
            else if (touch.phase == TouchPhase.Ended && isSwiping)
            { ProcessSwipe(swipeStart, touch.position); isSwiping = false; }
        }

        if (Input.GetMouseButtonDown(0))               { swipeStart = Input.mousePosition; isSwiping = true; }
        if (Input.GetMouseButtonUp(0) && isSwiping)    { ProcessSwipe(swipeStart, Input.mousePosition); isSwiping = false; }
    }

    void ProcessSwipe(Vector2 start, Vector2 end)
    {
        Vector2 delta = end - start;
        if (delta.magnitude < minSwipeDistance) return;
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            RequestTurn(delta.x > 0 ? Vector2.right : Vector2.left);
        else
            RequestTurn(delta.y > 0 ? Vector2.up : Vector2.down);
    }

    // ─────────────────────────────────────────
    //  INPUT — KEYBOARD
    // ─────────────────────────────────────────
    void HandleKeyboardInput()
    {
        if (!allowKeyboardInput) return;
        if      (Input.GetKeyDown(KeyCode.LeftArrow)  || Input.GetKeyDown(KeyCode.A)) RequestTurn(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) RequestTurn(Vector2.right);
        else if (Input.GetKeyDown(KeyCode.UpArrow)    || Input.GetKeyDown(KeyCode.W)) RequestTurn(Vector2.up);
        else if (Input.GetKeyDown(KeyCode.DownArrow)  || Input.GetKeyDown(KeyCode.S)) RequestTurn(Vector2.down);
    }

    // ─────────────────────────────────────────
    //  TURN REQUEST / QUEUE
    // ─────────────────────────────────────────
    void RequestTurn(Vector2 newDir)
    {
        newDir = newDir.normalized;
        if (newDir == Vector2.zero) return;
        if (preventReverseTurn && IsOppositeDirection(direction, newDir)) return;

        ApplyTurn(newDir);
        turnQueued = false;
    }

    void TryApplyQueuedTurn()
    {
        if (!turnQueued) return;
        if (!useTurnPoints || !HasAnyTurnPoints() || IsAtTurnPoint())
        { ApplyTurn(queuedDirection); turnQueued = false; }
    }

    bool IsAtTurnPoint()
    {
        if (turnPoints == null) return false;
        Vector2 pos = transform.position;
        float   thr = Mathf.Max(0.001f, turnTriggerDistance);
        foreach (Transform tp in turnPoints)
            if (tp != null && Vector2.Distance(pos, tp.position) <= thr) return true;
        return false;
    }

    bool HasAnyTurnPoints()
    {
        if (turnPoints == null || turnPoints.Length == 0) return false;
        foreach (Transform tp in turnPoints) if (tp != null) return true;
        return false;
    }

    void ApplyTurn(Vector2 newDir)
    {
        if (newDir.sqrMagnitude < 0.0001f) return;
        direction       = direction.sqrMagnitude < 0.0001f ? newDir.normalized : direction.normalized;
        targetDirection = newDir.normalized;
        moveDirection   = direction;
    }

    bool IsOppositeDirection(Vector2 a, Vector2 b)
        => Vector2.Dot(a.normalized, b.normalized) < -0.999f;

    public void AutoTurn(Vector2 dir)
    {
        dir = dir.normalized;
        if (dir.sqrMagnitude < 0.0001f) return;

        direction = dir;
        targetDirection = dir;
        moveDirection = dir;
        turnQueued = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var trigger = other.GetComponent<SnakeWallTrigger>();
        if (trigger != null)
            AutoTurn(trigger.turnDirection);
    }


    // ─────────────────────────────────────────
    //  BOUNDARY WALL TRIGGERS
    // ─────────────────────────────────────────
    // ─────────────────────────────────────────
    //  BOUNDARY WALL TRIGGERS
    // ─────────────────────────────────────────
    // ─────────────────────────────────────────
    //  BOUNDARY WALL TRIGGERS
    // ─────────────────────────────────────────
    void CheckWallTriggers()
    {
        if (wallTriggers == null || wallTriggers.Count == 0) return;

        Vector2 cur = transform.position;
        bool triggered = false;

        foreach (WallTrigger w in wallTriggers)
        {
            float prevSide = w.axis == WallTrigger.Axis.X
                ? prevHeadPos.x - w.position
                : prevHeadPos.y - w.position;

            float curSide = w.axis == WallTrigger.Axis.X
                ? cur.x - w.position
                : cur.y - w.position;

            // Trigger only if we definitively crossed the line
            if (prevSide * curSide < 0f && w.turnTo.sqrMagnitude > 0.001f)
            {
                direction       = w.turnTo.normalized;
                targetDirection = direction;
                moveDirection   = direction;
                turnQueued      = false;

                // THE FIX: Push the snake 0.05 units back into the "safe" side it came from.
                // This eliminates the micro-stuttering float errors at the corners.
                float safeSideBump = Mathf.Sign(prevSide) * 0.05f;

                if (w.axis == WallTrigger.Axis.X) 
                    cur.x = w.position + safeSideBump;
                else 
                    cur.y = w.position + safeSideBump;
                
                triggered = true;
            }
        }

        // Only apply the transform change if a wall was actually hit
        if (triggered)
        {
            transform.position = cur;
        }
    }
    // ─────────────────────────────────────────
    //  SNAKE MOVEMENT
    // ─────────────────────────────────────────
    void MoveSnake()
    {
        if (targetDirection.sqrMagnitude > 0.0001f)
        {
            moveDirection = Vector2.Lerp(moveDirection, targetDirection,
                turnSmooth * Time.deltaTime).normalized;
            if (moveDirection.sqrMagnitude < 0.0001f)
                moveDirection = targetDirection.normalized;
        }

        direction = moveDirection.normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        transform.rotation  = Quaternion.Euler(0f, 0f,
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);
    }

    // ─────────────────────────────────────────
    //  HEXAGON TEXTURE
    // ─────────────────────────────────────────
    void RebuildHexTexture()
    {
        const int W = 256, H = 256;
        if (hexTex == null)
        {
            hexTex = new Texture2D(W, H, TextureFormat.RGBA32, false);
            hexTex.wrapMode   = TextureWrapMode.Repeat;
            hexTex.filterMode = FilterMode.Bilinear;
        }

        Color[] pixels = new Color[W * H];
        float cellW = 1f / hexColumns;
        float cellH = 1f / hexRows;

        for (int py = 0; py < H; py++)
        {
            float v = py / (float)H;
            for (int px = 0; px < W; px++)
            {
                float u    = px / (float)W;
                float colF = u / cellW;
                int   col  = Mathf.FloorToInt(colF);
                float colFr = colF - col;

                float vOff  = (col % 2 == 1) ? cellH * 0.5f : 0f;
                float rowF  = (v + vOff) / cellH;
                int   row   = Mathf.FloorToInt(rowF);
                float rowFr = rowF - row;

                bool inHex = IsInsideFlatTopHex(colFr, rowFr, groutThickness);

                int colorCount = (scaleColors != null && scaleColors.Length > 0) ? scaleColors.Length : 1;
                int idx        = ((col * 3 + row * 7) & 0x7FFFFFFF) % colorCount;
                Color c        = (scaleColors != null && scaleColors.Length > 0) ? scaleColors[idx] : Color.white;

                pixels[py * W + px] = inHex ? c
                    : new Color(c.r * groutBrightness, c.g * groutBrightness, c.b * groutBrightness, c.a);
            }
        }

        hexTex.SetPixels(pixels);
        hexTex.Apply();

        if (bodyMat == null) bodyMat = new Material(Shader.Find("Sprites/Default"));
        bodyMat.mainTexture = hexTex;
        line.material       = bodyMat;
        line.textureMode    = LineTextureMode.Tile;
        CacheHexSettings();
    }

    static bool IsInsideFlatTopHex(float lx, float ly, float grout)
    {
        float mg = grout * 0.5f;
        float x  = lx * 2f - 1f, y = ly * 2f - 1f;
        float ax = Mathf.Abs(x), ay = Mathf.Abs(y);
        float s3o2 = 0.866f;
        return ax <= (1f - mg) && ay <= (s3o2 - mg) && (ax * 0.5f + ay) <= (s3o2 - mg);
    }

    Gradient BuildFixedGradient()
    {
        Gradient g = new Gradient();
        g.mode = GradientMode.Fixed;
        int count = (scaleColors != null && scaleColors.Length > 0) ? Mathf.Min(scaleColors.Length, 8) : 1;
        GradientColorKey[] ckeys = new GradientColorKey[count];
        for (int i = 0; i < count; i++)
        {
            float t = count == 1 ? 0f : i / (float)(count - 1);
            ckeys[i] = new GradientColorKey(
                (scaleColors != null && scaleColors.Length > 0) ? scaleColors[i] : Color.white, t);
        }
        g.SetKeys(ckeys, new GradientAlphaKey[] {
            new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) });
        return g;
    }

    bool HexSettingsChanged()
    {
        if (cachedHexCols != hexColumns || cachedHexRows != hexRows) return true;
        if (!Mathf.Approximately(cachedGroutBrightness, groutBrightness)) return true;
        if (!Mathf.Approximately(cachedGroutThickness,  groutThickness))  return true;
        if (cachedScaleColors == null || cachedScaleColors.Length != (scaleColors?.Length ?? 0)) return true;
        if (scaleColors != null)
            for (int i = 0; i < scaleColors.Length; i++)
                if (cachedScaleColors[i] != scaleColors[i]) return true;
        return false;
    }

    void CacheHexSettings()
    {
        cachedHexCols = hexColumns; cachedHexRows = hexRows;
        cachedGroutBrightness = groutBrightness; cachedGroutThickness = groutThickness;
        if (scaleColors != null)
        {
            cachedScaleColors = new Color[scaleColors.Length];
            System.Array.Copy(scaleColors, cachedScaleColors, scaleColors.Length);
        }
    }

    // ─────────────────────────────────────────
    //  VISUALS
    // ─────────────────────────────────────────
    void ApplyVisuals()
    {
        line.numCornerVertices = 12;
        line.numCapVertices    = 12;
        line.textureMode       = LineTextureMode.Tile;
        line.widthCurve        = bodyWidth;
        line.widthMultiplier   = widthScale;
        line.colorGradient     = BuildFixedGradient();
        if (bodyMat != null) line.material = bodyMat;
    }

    // ─────────────────────────────────────────
    //  EDIT MODE PREVIEW
    // ─────────────────────────────────────────
    public void BuildPreview()
    {
        GrabComponents();
        Vector2 dir = previewDirection.normalized;
        if (dir == Vector2.zero) dir = Vector2.up;
        direction = dir; moveDirection = dir; targetDirection = dir;
        queuedDirection = dir; turnQueued = false;

        InitBodyPoints(transform.position, dir);
        for (int i = 0; i < bodyPoints.Count; i++)
            bodyPoints[i] = transform.position - (Vector3)(dir * spacing * i);

        RebuildHexTexture();
        ApplyVisuals();
        DrawLine();
        SetupHeadChild();

        if (tongueEnabled)
        { SetupTongue(); tongueExtend = 1f; DrawTongueForks(); SyncTongueStyle(); SetTongueVisible(true); }
        else
        { SetupTongue(); SetTongueVisible(false); }
    }

    public void ClearPreview()
    {
        GrabComponents();
        line.positionCount = 0;
        Transform h = transform.Find("__SnakeHead__");
        if (h != null) { var sr = h.GetComponent<SpriteRenderer>(); if (sr) sr.enabled = false; }
        SetTongueVisible(false);
    }

    // ─────────────────────────────────────────
    //  BODY SIMULATION
    // ─────────────────────────────────────────
    void InitBodyPoints(Vector3 origin, Vector2 dir)
    {
        bodyPoints.Clear();
        for (int i = 0; i < segments; i++) bodyPoints.Add(origin);
    }

    void MoveBody()
    {
        bodyPoints[0] = transform.position;
        for (int i = 1; i < bodyPoints.Count; i++)
        {
            Vector3 prev = bodyPoints[i - 1], cur = bodyPoints[i];
            if (Vector3.Distance(cur, prev) > spacing)
                bodyPoints[i] = Vector3.Lerp(cur, prev, 0.45f);
        }
    }

    void DrawLine()
    {
        line.positionCount = bodyPoints.Count;
        for (int i = 0; i < bodyPoints.Count; i++)
            line.SetPosition(i, bodyPoints[i]);
    }

    // ─────────────────────────────────────────
    //  HEAD SPRITE
    // ─────────────────────────────────────────
    void SetupHeadChild()
    {
        if (headRenderer == null)
        {
            Transform existing = transform.Find("__SnakeHead__");
            if (existing != null) headRenderer = existing.GetComponent<SpriteRenderer>();
            else
            {
                GameObject go = new GameObject("__SnakeHead__");
                go.transform.SetParent(transform, false);
                headRenderer = go.AddComponent<SpriteRenderer>();
                headRenderer.sortingOrder = 1;
            }
        }
        UpdateHeadChild();
    }

    void UpdateHeadChild()
    {
        if (headRenderer == null) return;
        headRenderer.sprite  = headSprite;
        headRenderer.enabled = (headSprite != null);
        headRenderer.color   = headColor;
        if (headSprite == null) return;

        Vector2 perp = new Vector2(-direction.y, direction.x);
        Vector3 off  = (Vector3)(direction * headOffset.y + perp * headOffset.x);
        headRenderer.transform.position   = transform.position + off;
        headRenderer.transform.rotation   = Quaternion.Euler(0f, 0f,
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f);
        headRenderer.transform.localScale = Vector3.one * headSize;
    }

    // ─────────────────────────────────────────
    //  TONGUE
    // ─────────────────────────────────────────
    void SetupTongue()
    {
        tongueLeft  = GetOrCreateTongueFork("__TongueL__");
        tongueRight = GetOrCreateTongueFork("__TongueR__");
    }

    LineRenderer GetOrCreateTongueFork(string childName)
    {
        Transform existing = transform.Find(childName);
        LineRenderer lr;
        if (existing != null)
        {
            lr = existing.GetComponent<LineRenderer>();
            if (lr == null) lr = existing.gameObject.AddComponent<LineRenderer>();
        }
        else
        {
            GameObject go = new GameObject(childName);
            go.transform.SetParent(transform, false);
            lr = go.AddComponent<LineRenderer>();
        }
        lr.positionCount = 2; lr.startWidth = tongueWidth; lr.endWidth = tongueWidth * 0.5f;
        lr.numCapVertices = 4; lr.numCornerVertices = 4;
        lr.useWorldSpace = true; lr.sortingOrder = 2;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = tongueColor; lr.endColor = tongueColor;
        return lr;
    }

    void UpdateTongue()
    {
        if (tongueLeft == null || tongueRight == null) SetupTongue();
        if (!tongueEnabled) { SetTongueVisible(false); return; }
        SetTongueVisible(true);

        tongueTimer += Time.deltaTime;
        switch (tongueState)
        {
            case TongueState.Rest:
                tongueExtend = 0f;
                if (tongueTimer >= tongueRestTime) ChangeState(TongueState.FlickOut);
                break;
            case TongueState.FlickOut:
                tongueExtend = Mathf.Clamp01(tongueTimer / Mathf.Max(tongueFlickOutTime, 0.001f));
                tongueExtend = 1f - (1f - tongueExtend) * (1f - tongueExtend);
                if (tongueTimer >= tongueFlickOutTime) { tongueExtend = 1f; ChangeState(TongueState.Hold); }
                break;
            case TongueState.Hold:
                tongueExtend = 1f;
                if (tongueTimer >= tongueHoldTime) ChangeState(TongueState.FlickIn);
                break;
            case TongueState.FlickIn:
                float t = Mathf.Clamp01(tongueTimer / Mathf.Max(tongueFlickInTime, 0.001f));
                tongueExtend = (1f - t) * (1f - t);
                if (tongueTimer >= tongueFlickInTime) { tongueExtend = 0f; ChangeState(TongueState.Rest); }
                break;
        }
        DrawTongueForks(); SyncTongueStyle();
    }

    void ChangeState(TongueState next) { tongueState = next; tongueTimer = 0f; }

    void DrawTongueForks()
    {
        Vector3 root = transform.position + (Vector3)(direction * tongueRootOffset);
        float   half = tongueForkAngle * 0.5f;
        SetFork(tongueLeft,  root, TongueTip(root, direction, -half));
        SetFork(tongueRight, root, TongueTip(root, direction,  half));
    }

    Vector3 TongueTip(Vector3 root, Vector2 dir, float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad), sin = Mathf.Sin(rad);
        Vector2 fd = new Vector2(dir.x * cos - dir.y * sin, dir.x * sin + dir.y * cos);
        return root + (Vector3)(fd * tongueLength * tongueExtend);
    }

    void SetFork(LineRenderer lr, Vector3 root, Vector3 tip)
    { if (lr == null) return; lr.SetPosition(0, root); lr.SetPosition(1, tip); }

    void SyncTongueStyle()
    {
        foreach (var lr in new[] { tongueLeft, tongueRight })
        {
            if (lr == null) continue;
            lr.startWidth = tongueWidth; lr.endWidth = tongueWidth * 0.5f;
            lr.startColor = tongueColor; lr.endColor = tongueColor;
        }
    }

    void SetTongueVisible(bool v)
    {
        if (tongueLeft  != null) tongueLeft.enabled  = v;
        if (tongueRight != null) tongueRight.enabled = v;
    }

    // ─────────────────────────────────────────
    //  GIZMOS — wall triggers + eat radius
    // ─────────────────────────────────────────
    void OnDrawGizmos()
    {
        // ── Eat radius (red wire sphere at head) ─
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, eatRadius);

        // ── Wall trigger lines ─────────────────
        if (wallTriggers == null) return;

        // Draw a line of length 20 units for each wall, centred on origin
        const float halfLen = 10f;

        foreach (WallTrigger w in wallTriggers)
        {
            // Default to cyan if color is fully transparent (i.e. not set)
            Color col = w.gizmoColor.a < 0.01f
                ? new Color(0f, 1f, 1f, 0.9f)
                : w.gizmoColor;

            Gizmos.color = col;

            if (w.axis == WallTrigger.Axis.X)
            {
                // Vertical line at x = w.position
                Vector3 top    = new Vector3(w.position,  halfLen, 0f);
                Vector3 bottom = new Vector3(w.position, -halfLen, 0f);
                Gizmos.DrawLine(top, bottom);

                // Arrow showing the turn direction
                Vector3 mid    = new Vector3(w.position, 0f, 0f);
                Vector3 arrow  = mid + (Vector3)(w.turnTo.normalized * 0.6f);
                Gizmos.DrawLine(mid, arrow);
            }
            else
            {
                // Horizontal line at y = w.position
                Vector3 left  = new Vector3(-halfLen, w.position, 0f);
                Vector3 right = new Vector3( halfLen, w.position, 0f);
                Gizmos.DrawLine(left, right);

                Vector3 mid   = new Vector3(0f, w.position, 0f);
                Vector3 arrow = mid + (Vector3)(w.turnTo.normalized * 0.6f);
                Gizmos.DrawLine(mid, arrow);
            }
        }
    }

    // ─────────────────────────────────────────
    //  EDITOR
    // ─────────────────────────────────────────
#if UNITY_EDITOR
    void OnValidate()
    {
        GrabComponents();
        if (line == null) return;
        line.widthCurve      = bodyWidth;
        line.widthMultiplier = widthScale;
        if (previewInEditor)
            UnityEditor.EditorApplication.delayCall += () => { if (this != null) BuildPreview(); };
        else
            ClearPreview();
    }
#endif

    void GrabComponents()
    {
        if (rb   == null) rb   = GetComponent<Rigidbody2D>();
        if (line == null) line = GetComponent<LineRenderer>();
    }
}

public class SnakeWallTrigger : MonoBehaviour
{
    public Vector2 turnDirection = Vector2.right;
}
