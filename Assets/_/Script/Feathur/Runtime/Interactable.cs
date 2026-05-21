using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class Interactable : MonoBehaviour
{
    #region Public Variable

    public bool IsDragging  { get; private set; }
    public bool IsResizing  { get; private set; }

    #endregion


    #region Unity API

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _line     = GetComponent<LineRenderer>();
    }

    #endregion


    #region PPublic API

    public bool IsOnEdge(Vector2 mouseWorld)
    {
        Vector2 center       = (Vector2)transform.position + _collider.offset;
        float dist           = Vector2.Distance(mouseWorld, center);
        float effectiveTol   = Mathf.Max(tolerance, _collider.radius * 0.1f);
        return Mathf.Abs(dist - _collider.radius) <= effectiveTol;
    }

    public bool IsInside(Vector2 mouseWorld)
    {
        Vector2 center = (Vector2)transform.position + _collider.offset;
        return Vector2.Distance(mouseWorld, center) < _collider.radius;
    }

    public void BeginDrag()
    {
        IsDragging        = true;
        _originalLayer    = transform.root.gameObject.layer;
        _originalChildLayer = gameObject.layer;
        SetLayerRecursive(transform.root.gameObject, LayerMask.NameToLayer(draggingLayer));
    }

    public void EndDrag()
    {
        IsDragging = false;
        transform.root.gameObject.layer = _originalLayer;
        gameObject.layer                = _originalChildLayer;
        SetLineColor(normalColor);
    }

    public void BeginResize()
    {
        IsResizing = true;
        SetLineColor(resizingColor);
    }

    public void Resize(Vector2 mouseWorld)
    {
        Vector2 center   = (Vector2)transform.position + _collider.offset;
        _collider.radius = Mathf.Max(1f, Vector2.Distance(mouseWorld, center));
    }

    public void EndResize()
    {
        IsResizing = false;
        SetLineColor(normalColor);
    }

    public void UpdateHoverFeedback(Vector2 mouseWorld)
    {
        if (IsDragging || IsResizing) return;
        SetLineColor(IsOnEdge(mouseWorld) ? hoverColor : normalColor);
    }

    private void SetLineColor(Color color)
    {
        _line.startColor = color;
        _line.endColor   = color;
    }

    private void SetLayerRecursive(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform child in go.transform)
            SetLayerRecursive(child.gameObject, layer);
    }

#endregion


    #region Private & Protected

    [Header("Resize Settings")]
    [Tooltip("Clickable thickness of the outline to trigger resize.")]
    [SerializeField] private float tolerance = 0.15f;

    [Header("Color Feedback")]
    [SerializeField] private Color normalColor   = Color.white;
    [SerializeField] private Color hoverColor    = Color.yellow;
    [SerializeField] private Color resizingColor = Color.green;

    [Header("Drag Settings")]
    [Tooltip("Layer assigned to this object and its children while dragged.")]
    [SerializeField] private string draggingLayer = "Dragging";

    private CircleCollider2D _collider;
    private LineRenderer _line;
    private int _originalLayer;
    private int _originalChildLayer;

    #endregion
}
