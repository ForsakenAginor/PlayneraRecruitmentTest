using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableThing : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private bool _isDragging = false;
    private Vector2 _lastPointerPosition;

    public event Action DraggingStarted;
    public event Action DraggingEnded;

    private void FixedUpdate()
    {
        if(_isDragging)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(_lastPointerPosition);
            Vector3 newPosition = new Vector3(position.x, position.y, transform.position.z);
            transform.position = newPosition;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
        DraggingStarted?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _lastPointerPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
        DraggingEnded?.Invoke();
    }
}