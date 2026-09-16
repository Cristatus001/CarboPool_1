using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Snake2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveInterval = 0.15f;
    public float gridSize = 1f;

    [Header("Feel")]
    [Range(0f, 1f)]
    public float smoothing = 0.85f;
    public float undulationStrength = 0.06f;
    public float undulationSpeed = 6f;

    [Header("Body Appearance")]
    public float headWidth = 0.92f;
    public float tailWidth = 0.3f;
    public int startLength = 8;
    public Color headColor = new Color(0.18f, 0.72f, 0.35f);
    public Color tailColor = new Color(0.08f, 0.38f, 0.18f);

    private LineRenderer _lr;
    private Vector2 _dir = Vector2.right;
    private Vector2 _nextDir = Vector2.right;
    private float _moveTimer;
    private float _interpTimer;
    private int _length;
    private bool _alive = true;

    private List<Vector2> _grid = new();
    private List<Vector2> _visual = new();
    private Vector2 _prevHead;

    void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        SetupLineRenderer();
    }

    void Start()
    {
        _length = startLength;
        Vector2 startPos = Vector2.zero;
        for (int i = 0; i < _length; i++)
        {
            Vector2 p = startPos - Vector2.right * i * gridSize;
            _grid.Add(p);
            _visual.Add(p);
        }
        _prevHead = _grid[0];
        _lr.positionCount = _length;
    }

    void SetupLineRenderer()
    {
        _lr.useWorldSpace = true;
        _lr.loop = false;
        _lr.numCornerVertices = 8;
        _lr.numCapVertices = 8;
        _lr.textureMode = LineTextureMode.Stretch;

        AnimationCurve widthCurve = new AnimationCurve(
            new Keyframe(0f, 1f),
            new Keyframe(0.12f, 1.08f),
            new Keyframe(0.5f, 0.72f),
            new Keyframe(1f, 0.22f)
        );
        _lr.widthCurve = widthCurve;
        _lr.widthMultiplier = headWidth;

        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(headColor, 0f),
                new GradientColorKey(Color.Lerp(headColor, tailColor, 0.5f), 0.45f),
                new GradientColorKey(tailColor, 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0.9f, 0.8f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        _lr.colorGradient = grad;

        _lr.material = new Material(Shader.Find("Sprites/Default"));
        _lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _lr.receiveShadows = false;
    }

    void Update()
    {
        if (!_alive) return;

        HandleInput();

        _moveTimer += Time.deltaTime;
        _interpTimer += Time.deltaTime;

        float t = Mathf.SmoothStep(0f, 1f, _interpTimer / moveInterval);

        UpdateVisualPositions(t);
        DrawBody(t);

        if (_moveTimer >= moveInterval)
        {
            _moveTimer -= moveInterval;
            _interpTimer = 0f;
            _prevHead = _grid[0];
            Step();
            PulseHead();
        }
    }

    void HandleInput()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))    && _dir != Vector2.down)  _nextDir = Vector2.up;
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))  && _dir != Vector2.up)    _nextDir = Vector2.down;
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))  && _dir != Vector2.right) _nextDir = Vector2.left;
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && _dir != Vector2.left)  _nextDir = Vector2.right;
    }

    void Step()
    {
        _dir = _nextDir;
        Vector2 newHead = _grid[0] + _dir * gridSize;

        _grid.Insert(0, newHead);
        _visual.Insert(0, _prevHead);

        if (_grid.Count > _length)
        {
            _grid.RemoveAt(_grid.Count - 1);
            _visual.RemoveAt(_visual.Count - 1);
        }

        transform.position = (Vector3)newHead;
        CheckSelfCollision(newHead);
    }

    void UpdateVisualPositions(float t)
    {
        if (_visual.Count == 0) return;
        _visual[0] = Vector2.Lerp(_prevHead, _grid[0], t);
    }

    void DrawBody(float t)
    {
        int count = Mathf.Min(_length, _visual.Count);
        _lr.positionCount = count;

        float time = Time.time * undulationSpeed;

        for (int i = 0; i < count; i++)
        {
            Vector2 pos = i == 0 ? _visual[0] : _visual[i];

            float phase = i * 0.45f - time;
            float wave = Mathf.Sin(phase) * undulationStrength;
            Vector2 perp = new Vector2(-_dir.y, _dir.x);
            Vector3 worldPos = (Vector3)(pos + perp * wave);

            _lr.SetPosition(i, worldPos);
        }
    }

    void CheckSelfCollision(Vector2 head)
    {
        for (int i = 4; i < _grid.Count; i++)
        {
            if (Vector2.Distance(head, _grid[i]) < gridSize * 0.4f)
            {
                Die();
                return;
            }
        }
    }

    void PulseHead()
    {
        StopAllCoroutines();
        StartCoroutine(HeadPulseRoutine());
    }

    System.Collections.IEnumerator HeadPulseRoutine()
    {
        float dur = 0.08f;
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            float s = 1f + Mathf.Sin(t / dur * Mathf.PI) * 0.18f;
            transform.localScale = Vector3.one * s;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

    void Die()
    {
        _alive = false;
        StartCoroutine(DeathRoutine());
    }

    System.Collections.IEnumerator DeathRoutine()
    {
        float dur = 0.5f;
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / dur);
            Gradient g = _lr.colorGradient;
            GradientAlphaKey[] keys = g.alphaKeys;
            for (int i = 0; i < keys.Length; i++) keys[i].alpha *= alpha;
            g.alphaKeys = keys;
            _lr.colorGradient = g;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    public void Grow(int amount = 1)
    {
        _length += amount;
        StartCoroutine(EatPulseRoutine());
    }

    System.Collections.IEnumerator EatPulseRoutine()
    {
        float dur = 0.12f;
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            float s = 1f + Mathf.Sin(t / dur * Mathf.PI) * 0.28f;
            transform.localScale = Vector3.one * s;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }
}