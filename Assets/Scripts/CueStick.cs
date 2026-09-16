using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CueStick : MonoBehaviour
{
    public GameObject cueBall;
    public float maxForce = 10f;
    public float minVelocityThreshold = 0.05f;
    public float cueOffset = 0f;
    public float lineLength = 5f;
    public float deflectionLength = 3f;
    public float maxPullBack = 1.5f;
    public float smoothSpeed = 10f;

    [Header("Aim Line")]
    public LineRenderer aimLineRenderer;
    public LineRenderer deflectionLineRenderer;
    public GameObject hitCirclePrefab;
    public LayerMask ballLayerMask;
    public float ballRadius = 0.19f;
    private GameObject hitCircleInstance;

    [Header("Mobile Controls")]
    public Slider powerSlider; // Assign in Inspector
    public GameObject powerSliderUI; // The slider GameObject
    public float shootThreshold = 0.1f; // Min power to shoot
    public float sliderTouchPadding = 80f;

    private bool isBallMoving = false;
    private bool isHitting = false;
    private bool hasShot = false;
    private float currentPullBack = 0f;
    private float pullBackAtRelease = 0f;
    private float hitSpeed = 0f;
    private Vector3 shootDirection;
    private Vector3 dirToMouse;
    private Vector3 baseCuePosition;

    // Aim rotation tracking
    private bool isAiming = false;
    private bool isPowerSliderDragging = false;
    private bool isMousePulling = false;
    private int aimingFingerId = -1;
    private Vector3 mousePullStart;
    private float currentAngle = 0f;

    private Rigidbody2D cueBallRb;
    private SpriteRenderer cueRenderer;

    bool UseMobileControls => Application.isMobilePlatform;

    void Start()
    {
        cueBallRb = cueBall.GetComponent<Rigidbody2D>();
        cueRenderer = GetComponent<SpriteRenderer>();
        dirToMouse = Vector3.right;
        shootDirection = Vector3.left;

        SetupLineRenderer(aimLineRenderer, Color.white, new Color(1f, 1f, 1f, 0.5f));
        SetupLineRenderer(deflectionLineRenderer, Color.white, new Color(1f, 1f, 1f, 0.3f));

        aimLineRenderer.enabled = false;
        deflectionLineRenderer.enabled = false;

        if (hitCirclePrefab != null)
        {
            hitCircleInstance = Instantiate(hitCirclePrefab);
            hitCircleInstance.SetActive(false);
        }

        // Setup power slider
        if (powerSlider != null)
        {
            powerSlider.value = 0f;
            powerSlider.onValueChanged.AddListener(OnPowerSliderChanged);
        }

        // Show slider only when ball stopped
        if (powerSliderUI != null)
            powerSliderUI.SetActive(false);
    }

    void Update()
    {
        // Wait for ball to stop
        if (isBallMoving)
        {
            if (cueBallRb.linearVelocity.magnitude < minVelocityThreshold)
            {
                cueBallRb.linearVelocity = Vector2.zero;
                cueBallRb.angularVelocity = 0f;
                isBallMoving = false;
                cueRenderer.enabled = true;
                isHitting = false;
                hasShot = false;
                currentPullBack = 0f;

                // Reset slider
                if (powerSlider != null) powerSlider.value = 0f;
                if (powerSliderUI != null) powerSliderUI.SetActive(UseMobileControls);
            }
            aimLineRenderer.enabled = false;
            deflectionLineRenderer.enabled = false;
            if (hitCircleInstance != null) hitCircleInstance.SetActive(false);
            return;
        }

        // Hit animation
        if (isHitting)
        {
            currentPullBack -= hitSpeed * Time.deltaTime;
            transform.position = baseCuePosition + dirToMouse * Mathf.Max(currentPullBack, 0f);

            if (!hasShot && currentPullBack <= 0f)
            {
                hasShot = true;
                cueBallRb.AddForce(
                    -dirToMouse * maxForce * (pullBackAtRelease / maxPullBack),
                    ForceMode2D.Impulse
                );
                isBallMoving = true;
                cueRenderer.enabled = false;
                aimLineRenderer.enabled = false;
                deflectionLineRenderer.enabled = false;
                if (powerSliderUI != null) powerSliderUI.SetActive(false);
            }
            return;
        }

        // Show slider only on mobile when ready.
        if (powerSliderUI != null && powerSliderUI.activeSelf != UseMobileControls)
            powerSliderUI.SetActive(UseMobileControls);

        // Handle aiming input
        HandleAimInput();

        // Update cue position based on current angle
        UpdateCueFromAngle();

        // Update aim line
        if (!isHitting && !isBallMoving)
            UpdateAimLine();
        else
        {
            aimLineRenderer.enabled = false;
            deflectionLineRenderer.enabled = false;
            if (hitCircleInstance != null) hitCircleInstance.SetActive(false);
        }

        if (UseMobileControls)
        {
            // Read slider power without moving the cue during UI drag.
            currentPullBack = powerSlider != null ?
                powerSlider.value * maxPullBack : 0f;
            transform.position = baseCuePosition;
        }
        else if (!isMousePulling && !isHitting)
        {
            currentPullBack = 0f;
            transform.position = baseCuePosition;
        }
    }

    void HandleAimInput()
    {
        if (UseMobileControls)
        {
            HandleMobileAimInput();
        }
        else
        {
            HandleMouseAimAndShootInput();
        }
    }

    void HandleMobileAimInput()
    {
        if (Input.touchCount <= 0)
        {
            aimingFingerId = -1;
            isPowerSliderDragging = false;
            isAiming = false;
            return;
        }

        bool sliderTouchActive = false;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch activeTouch = Input.GetTouch(i);
            if (IsTouchNearSlider(activeTouch.position))
            {
                sliderTouchActive = true;
                if (activeTouch.phase == TouchPhase.Began)
                    isPowerSliderDragging = true;
            }
        }

        if (sliderTouchActive || isPowerSliderDragging)
        {
            isAiming = false;
            aimingFingerId = -1;

            if (!sliderTouchActive)
                isPowerSliderDragging = false;

            return;
        }

        Touch touch;
        if (!TryGetAimingTouch(out touch))
        {
            isAiming = false;
            return;
        }

        if (touch.phase == TouchPhase.Began)
        {
            if (IsTouchNearSlider(touch.position))
                return;

            aimingFingerId = touch.fingerId;
            isAiming = true;
            RotateCueTowardScreenPoint(touch.position);
        }
        else if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && isAiming)
        {
            if (IsTouchNearSlider(touch.position))
            {
                isAiming = false;
                aimingFingerId = -1;
                return;
            }

            RotateCueTowardScreenPoint(touch.position);
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isAiming = false;
            aimingFingerId = -1;
        }
    }

    void HandleMouseAimAndShootInput()
    {
        if (IsPointerOverUI() && !isMousePulling) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        if (!isMousePulling)
        {
            dirToMouse = (mousePos - cueBall.transform.position).normalized;
            shootDirection = -dirToMouse;

            float angle = Mathf.Atan2(dirToMouse.y, dirToMouse.x) * Mathf.Rad2Deg;
            currentAngle = angle;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isMousePulling = true;
            mousePullStart = mousePos;
            currentPullBack = 0f;
        }

        if (Input.GetMouseButton(0) && isMousePulling)
        {
            float dragDistance = Vector3.Dot(mousePos - mousePullStart, dirToMouse);
            currentPullBack = Mathf.Clamp(dragDistance, 0f, maxPullBack);
            transform.position = baseCuePosition + dirToMouse * currentPullBack;
        }

        if (Input.GetMouseButtonUp(0) && isMousePulling)
        {
            isMousePulling = false;

            if (currentPullBack > shootThreshold)
            {
                StartShot(currentPullBack);
            }
            else
            {
                currentPullBack = 0f;
                transform.position = baseCuePosition;
            }
        }
    }

    bool TryGetAimingTouch(out Touch aimingTouch)
    {
        if (aimingFingerId != -1)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.fingerId == aimingFingerId)
                {
                    aimingTouch = touch;
                    return true;
                }
            }

            aimingFingerId = -1;
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began && !IsTouchNearSlider(touch.position))
            {
                aimingFingerId = touch.fingerId;
                aimingTouch = touch;
                return true;
            }
        }

        aimingTouch = default;
        return false;
    }

    void RotateCueTowardScreenPoint(Vector2 screenPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;

        Vector3 aimDirection = worldPos - cueBall.transform.position;
        if (aimDirection.sqrMagnitude < 0.0001f) return;

        dirToMouse = aimDirection.normalized;
        currentAngle = Mathf.Atan2(dirToMouse.y, dirToMouse.x) * Mathf.Rad2Deg;
        currentAngle = Mathf.Repeat(currentAngle, 360f);
        shootDirection = -dirToMouse;
    }

    void UpdateCueFromAngle()
    {
        // Update cue rotation
        transform.rotation = Quaternion.Euler(0, 0, currentAngle + 145f);

        // Update base position
        baseCuePosition = cueBall.transform.position + dirToMouse * cueOffset;
    }

    bool IsTouchNearSlider(Vector2 touchPos)
    {
        return IsTouchNearSlider(touchPos, sliderTouchPadding);
    }

    bool IsTouchNearSlider(Vector2 touchPos, float padding)
    {
        if (powerSlider == null) return false;

        RectTransform sliderRect = powerSlider.GetComponent<RectTransform>();
        if (sliderRect == null || !sliderRect.gameObject.activeInHierarchy) return false;

        Canvas canvas = sliderRect.GetComponentInParent<Canvas>();
        Camera uiCamera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay ?
            canvas.worldCamera : null;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            sliderRect, touchPos, uiCamera, out Vector2 localPoint);

        Rect paddedRect = sliderRect.rect;
        paddedRect.xMin -= padding;
        paddedRect.xMax += padding;
        paddedRect.yMin -= padding;
        paddedRect.yMax += padding;

        return paddedRect.Contains(localPoint);
    }

    bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    return true;
            }

            return false;
        }

        return EventSystem.current.IsPointerOverGameObject();
    }

    void OnPowerSliderChanged(float value)
    {
        // Store power only. The cue moves when the slider is released to shoot.
        currentPullBack = value * maxPullBack;

        // If slider released at value > threshold — shoot!
        // (Handled by slider pointer up event)
    }

    // Called by EventTrigger on Slider — PointerUp
    public void OnSliderReleased()
    {
        isPowerSliderDragging = false;

        if (!UseMobileControls || powerSlider == null) return;
        float power = powerSlider.value;

        if (power > shootThreshold && !isBallMoving && !isHitting)
        {
            float sliderPullBack = power * maxPullBack;
            powerSlider.value = 0f;
            StartShot(sliderPullBack);
        }
        else
        {
            // Not enough power — reset
            powerSlider.value = 0f;
            currentPullBack = 0f;
            transform.position = baseCuePosition;
        }
    }

    void StartShot(float pullBack)
    {
        pullBackAtRelease = Mathf.Clamp(pullBack, 0f, maxPullBack);
        hitSpeed = pullBackAtRelease / 0.08f;
        currentPullBack = pullBackAtRelease;
        isHitting = true;
        hasShot = false;
        transform.position = baseCuePosition + dirToMouse * currentPullBack;
    }

    void SetupLineRenderer(LineRenderer lr, Color startColor, Color endColor)
    {
        if (lr == null) return;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = startColor;
        lr.endColor = endColor;
        lr.useWorldSpace = true;
        lr.sortingLayerName = "Default";
        lr.sortingOrder = 10;
        lr.startWidth = 0.06f;
        lr.endWidth = 0.06f;
    }

    void UpdateAimLine()
    {
        Vector3 startPos = cueBall.transform.position;
        Vector2 dir2D = new Vector2(-dirToMouse.x, -dirToMouse.y).normalized;

        RaycastHit2D[] hits = new RaycastHit2D[10];
        int hitCount = Physics2D.CircleCast(
            startPos,
            ballRadius,
            dir2D,
            new ContactFilter2D() { layerMask = ballLayerMask, useLayerMask = true, useTriggers = false },
            hits,
            lineLength
        );

        RaycastHit2D validHit = default;
        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider != null &&
                hits[i].collider.gameObject != cueBall &&
                hits[i].distance > 0.05f)
            {
                validHit = hits[i];
                break;
            }
        }

       if (validHit.collider != null)
{
    // Declare BOTH variables here at top
    Vector3 cueBallAtImpact = new Vector3(
        validHit.centroid.x,
        validHit.centroid.y,
        0f
    );

    Vector3 contactPoint = new Vector3(
        validHit.point.x,
        validHit.point.y,
        0f
    );

    DrawDottedLine(aimLineRenderer, startPos, cueBallAtImpact,
        Color.white, new Color(1f, 1f, 1f, 0.5f));

    if (hitCircleInstance != null)
    {
        hitCircleInstance.SetActive(true);
        hitCircleInstance.transform.position = cueBallAtImpact;
    }

    if (validHit.collider.CompareTag("Wall"))
    {
        Vector3 reflectDir = Vector2.Reflect(dir2D, validHit.normal).normalized;
        DrawDottedLine(deflectionLineRenderer,
            cueBallAtImpact,
            cueBallAtImpact + reflectDir * deflectionLength,
            Color.white, new Color(1f, 1f, 1f, 0.3f));
    }
    else
    {
        // Exact physics-accurate deflection
        Vector3 hitBallCenter = validHit.collider.transform.position;

        // This is EXACTLY where physics will send the hit ball
        Vector3 deflectDir = (hitBallCenter - cueBallAtImpact).normalized;

        // Start FROM hit ball center — not contact point
        Vector3 deflectEnd = hitBallCenter + deflectDir * deflectionLength;

        DrawDottedLine(
            deflectionLineRenderer,
            hitBallCenter,
            deflectEnd,
            Color.white,
            new Color(1f, 1f, 1f, 0.3f)
        );
    }
    }
    }

    void DrawDottedLine(LineRenderer lr, Vector3 start, Vector3 end,
        Color startColor, Color endColor)
    {
        if (lr == null) return;
        float totalDist = Vector3.Distance(start, end);
        if (totalDist < 0.01f) { lr.enabled = false; return; }

        float dotSize = 0.05f;
        float gapSize = 0.1f;
        List<Vector3> positions = new List<Vector3>();
        AnimationCurve widthCurve = new AnimationCurve();
        Vector3 dir = (end - start).normalized;

        float dist = 0f;
        bool drawing = true;

        while (dist < totalDist)
        {
            float t = dist / totalDist;
            positions.Add(start + dir * dist);
            widthCurve.AddKey(new Keyframe(t, drawing ? 0.06f : 0f));
            dist += drawing ? dotSize : gapSize;
            drawing = !drawing;
        }

        positions.Add(end);
        widthCurve.AddKey(new Keyframe(1f, 0f));

        lr.positionCount = positions.Count;
        lr.SetPositions(positions.ToArray());
        lr.widthCurve = widthCurve;
        lr.widthMultiplier = 1f;
        lr.startColor = startColor;
        lr.endColor = endColor;
        lr.enabled = true;
    }

    public bool IsBallMoving() => isBallMoving;

    Vector3 GetInputPosition()
    {
        Vector3 screenPos = Input.touchCount > 0 ?
            (Vector3)Input.GetTouch(0).position : Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;
        return worldPos;
    }

    bool GetInputBegan() =>
        Input.touchCount > 0 ?
        Input.GetTouch(0).phase == TouchPhase.Began :
        Input.GetMouseButtonDown(0);

    bool GetInputHeld() =>
        Input.touchCount > 0 ?
        (Input.GetTouch(0).phase == TouchPhase.Moved ||
        Input.GetTouch(0).phase == TouchPhase.Stationary) :
        Input.GetMouseButton(0);

    bool GetInputReleased() =>
        Input.touchCount > 0 ?
        Input.GetTouch(0).phase == TouchPhase.Ended :
        Input.GetMouseButtonUp(0);
}
