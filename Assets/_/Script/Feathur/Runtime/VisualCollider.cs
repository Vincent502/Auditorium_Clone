using UnityEngine;

public class VisualCollider : MonoBehaviour
{
    #region Public variable
    public enum OutlineShape { Circle, Quad }

    #endregion


    #region Unity API
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;

        circleCollider = GetComponent<CircleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        RefreshOutline();
        CacheState();
    }

    void Update()
    {
        if (IsDirty())
        {
            RefreshOutline();
            CacheState();
        }
    }

    #endregion


    #region Main API
    bool IsDirty()
    {
        if (shape != _lastShape || vertexCount != _lastVertexCount) return true;

        if (shape == OutlineShape.Circle && circleCollider != null)
            return circleCollider.radius != _lastRadius || circleCollider.offset != _lastCircleOffset;

        if (shape == OutlineShape.Quad && boxCollider != null)
            return boxCollider.size != _lastBoxSize || boxCollider.offset != _lastBoxOffset;

        return false;
    }

    void CacheState()
    {
        _lastShape = shape;
        _lastVertexCount = vertexCount;

        if (circleCollider != null)
        {
            _lastRadius = circleCollider.radius;
            _lastCircleOffset = circleCollider.offset;
        }

        if (boxCollider != null)
        {
            _lastBoxSize = boxCollider.size;
            _lastBoxOffset = boxCollider.offset;
        }
    }

    void RefreshOutline()
    {
        if (shape == OutlineShape.Circle)
            DrawCircleOutline();
        else
            DrawQuadOutline();
    }

    void DrawCircleOutline()
    {
        lineRenderer.positionCount = vertexCount;

        Vector3[] points = new Vector3[vertexCount];

        float radius = circleCollider.radius;
        Vector2 offset = circleCollider.offset;

        for (int i = 0; i < vertexCount; i++)
        {
            float angle = i * 2 * Mathf.PI / vertexCount;
            float x = Mathf.Cos(angle) * radius + offset.x;
            float y = Mathf.Sin(angle) * radius + offset.y;
            points[i] = new Vector3(x, y, 0f);
        }

        lineRenderer.SetPositions(points);
    }

    void DrawQuadOutline()
    {
        lineRenderer.positionCount = 4;

        Vector2 size = boxCollider.size * 0.5f;
        Vector2 offset = boxCollider.offset;

        Vector3[] points = new Vector3[4]
        {
            new Vector3(-size.x + offset.x,  size.y + offset.y, 0f),
            new Vector3( size.x + offset.x,  size.y + offset.y, 0f),
            new Vector3( size.x + offset.x, -size.y + offset.y, 0f),
            new Vector3(-size.x + offset.x, -size.y + offset.y, 0f),
        };

        lineRenderer.SetPositions(points);
    }

    #endregion


    #region Private & protected

    [Header("Shape Settings")]
    [Tooltip("Choose the outline shape to match the collider.")]
    [SerializeField] private OutlineShape shape = OutlineShape.Circle;

    [Header("Line Settings")]
    [Tooltip("The higher this number, the smoother the circle will be. (Circle only)")]
    [SerializeField] private int vertexCount = 40;

    private CircleCollider2D circleCollider;
    private BoxCollider2D boxCollider;
    private LineRenderer lineRenderer;

    private OutlineShape _lastShape;
    private int _lastVertexCount;
    private float _lastRadius;
    private Vector2 _lastCircleOffset;
    private Vector2 _lastBoxSize;
    private Vector2 _lastBoxOffset;

    #endregion
}