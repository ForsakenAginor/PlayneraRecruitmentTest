using UnityEngine;

public class SceneScroller : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _minCameraPosition;
    [SerializeField] private float _maxCameraPosition;
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private float _slideBorder = 0.05f;
    [SerializeField] private float _carryingSlideSpeed = 0.5f;
    [SerializeField] private DraggableThing _draggable;

    private IPlayerInput _input;
    private Vector2 _inputRotation;
    private int _inputPosition;
    private ScrollingMode _scrollingMode = ScrollingMode.Common;
    private int _width;
    private int _minWidth;
    private int _maxWidth;

    private enum ScrollingMode
    {
        Common,
        Carrying,
    }

    private void Awake()
    {
        _input = new MobilePlayerInput();
        _width = Screen.width;
        _minWidth = (int)(_slideBorder * _width);
        _maxWidth = (int)((1 - _slideBorder) * _width);

        _draggable.DraggingStarted += OnDraggingStarted;
        _draggable.DraggingEnded += OnDraggingEnded;
    }

    private void FixedUpdate()
    {
        if (_scrollingMode == ScrollingMode.Carrying)
        {
            _inputPosition = (int)_input.GetPointerPosition().x;

            if (_inputPosition < _minWidth)            
                MoveCamera(-_carryingSlideSpeed);            
            else if (_inputPosition > _maxWidth)
                MoveCamera(_carryingSlideSpeed);            

            return;
        }

        _inputRotation = _input.GetRotation();

        if (_inputRotation == Vector2.zero)
            return;

        MoveCamera(-_inputRotation.x);
    }

    private void OnDestroy()
    {
        _draggable.DraggingStarted -= OnDraggingStarted;
        _draggable.DraggingEnded -= OnDraggingEnded;
    }

    private void OnDraggingEnded()
    {
        _scrollingMode = ScrollingMode.Common;
    }

    private void OnDraggingStarted()
    {
        _scrollingMode = ScrollingMode.Carrying;
    }

    private void MoveCamera(float deltaX)
    {
        float nextX = _camera.position.x + deltaX;
        nextX = Mathf.Clamp(nextX, _minCameraPosition, _maxCameraPosition);
        Vector3 target = new Vector3(nextX, _camera.position.y, _camera.position.z);
        _camera.position = Vector3.MoveTowards(_camera.position, target, Time.deltaTime * _scrollSpeed);
    }
}