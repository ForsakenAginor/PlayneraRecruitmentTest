using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(DraggableThing))]
public class Gravity : MonoBehaviour
{
    [SerializeField] private float _fallingSpeed = 5;
    [SerializeField] private float _raycastOriginHeight = 1.5f;
    [SerializeField] private LayerMask _spotsLayerMask;

    private DraggableThing _thing;
    private Vector3 _target;
    private bool _isFalling;

    private void Awake()
    {
        _thing = GetComponent<DraggableThing>();

        _thing.DraggingStarted += OnDraggingStarted;
        _thing.DraggingEnded += OnDraggingEnded;
    }

    private void FixedUpdate()
    {
        if (_isFalling == false)
            return;

        if (transform.position == _target)
        {
            _isFalling = false;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * _fallingSpeed);
    }

    private void OnDestroy()
    {
        _thing.DraggingStarted -= OnDraggingStarted;
        _thing.DraggingEnded -= OnDraggingEnded;
    }

    private void OnDraggingStarted()
    {
        _isFalling = false;
    }

    private void OnDraggingEnded()
    {
        _isFalling = true;
        SetTargetFallingPosition();
    }

    private void SetTargetFallingPosition()
    {
        Vector2 raycastOrigin = new Vector2(transform.position.x, _raycastOriginHeight);
        float distance = 5;
        var hits = Physics2D.RaycastAll(raycastOrigin, Vector2.down, distance, _spotsLayerMask);

        //if noone spots found, like if we throw it out the window
        if (hits.Count() == 0)
        {
            _target = new Vector3(transform.position.x, -10, 0);
            return;
        }

        List<Spot> spots = new List<Spot>();

        foreach (var hit in hits)
        {
            var collider = hit.collider as BoxCollider2D;
            float center = collider.bounds.center.y;
            float max = center + collider.size.y / 2;
            float min = center - collider.size.y / 2;
            Spot spot = new Spot(min, max);
            spots.Add(spot);
        }

        float y = transform.position.y;
        bool inSpot = false;

        //fix item at spot
        foreach (var spot in spots)
        {
            if (spot.InRange(y))
            {
                inSpot = true;
                _target = new Vector3(transform.position.x, y, 0);
            }
        }

        //falling to first spot
        if (inSpot == false)
        {
            float targetY = spots.Where(o => o.Max < y).Select(o => o.Max).Max();
            _target = new Vector3(transform.position.x, targetY, 0);
        }
    }

    private struct Spot
    {
        private float _min;
        private float _max;

        public Spot(float min, float max)
        {
            _min = min;
            _max = max;
        }

        public float Min => _min;

        public float Max => _max;

        public bool InRange(float value)
        {
            return value >= _min && value <= _max;
        }
    }
}
