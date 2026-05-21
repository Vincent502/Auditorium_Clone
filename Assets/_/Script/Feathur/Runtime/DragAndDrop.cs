using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    #region Unity API
    private void Start()
    {
        _click = InputSystem.actions.FindAction("Attack");
        _cam   = Camera.main;
    }

    private void Update()
    {
        if (_click == null) return;

        Vector2 mouseWorld = GetMouseWorldPosition();

        if (_click.WasPressedThisFrame())
            OnPress(mouseWorld);

        if (_click.IsPressed())
            OnHold(mouseWorld);

        if (_click.WasReleasedThisFrame())
            OnRelease();

        UpdateFeedback(mouseWorld);
    }

#endregion


    #region main API

    private Vector2 GetMouseWorldPosition()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        float t = (0f - ray.origin.z) / ray.direction.z;
        return ray.origin + ray.direction * t;
    }

    private void OnPress(Vector2 mouseWorld)
    {
        if (_target != null) return;

        
        foreach (Interactable interactable in FindObjectsByType<Interactable>(FindObjectsSortMode.None))
        {
            if (interactable.IsOnEdge(mouseWorld))
            {
                _target = interactable;
                _target.BeginResize();
                return;
            }
        }

        
        Interactable hit = Raycast();
        if (hit == null) return;

        _target     = hit;
        _dragOffset = _target.transform.root.position - (Vector3)mouseWorld;
        _target.BeginDrag();
    }

    private void OnHold(Vector2 mouseWorld)
    {
        if (_target == null) return;

        if (_target.IsDragging)
            _target.transform.root.position = (Vector3)mouseWorld + _dragOffset;
        else if (_target.IsResizing)
            _target.Resize(mouseWorld);
    }

    private void OnRelease()
    {
        if (_target == null) return;

        if (_target.IsDragging)  _target.EndDrag();
        if (_target.IsResizing)  _target.EndResize();

        _target = null;
    }

    private void UpdateFeedback(Vector2 mouseWorld)
    {
        if (_target != null) return;

        foreach (Interactable interactable in FindObjectsByType<Interactable>(FindObjectsSortMode.None))
            interactable.UpdateHoverFeedback(mouseWorld);
    }

    private Interactable Raycast()
    {
        Vector2 worldPoint = GetMouseWorldPosition();
        RaycastHit2D hit   = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider == null) return null;

        return hit.collider.GetComponentInParent<Interactable>()
            ?? hit.collider.GetComponent<Interactable>();
    }

#endregion


    #region Private & Protttected

    private InputAction _click;
    private Camera _cam;

    private Interactable _target;
    private Vector3 _dragOffset;

    #endregion
}
