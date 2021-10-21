using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform roadTf;
    [Tooltip("Measures actual coefficient by which swipe length multiplies.")]
    [Range(0.1f, 5.0f)]
    public float strokeSensitivity = 2f;
    public float moveSpeed = 15f;

    public System.Action<int> OnCoinPickedUp;
    public System.Action OnFinishReached;
    public System.Action OnLMBReleased;
    public System.Action OnLMBPressed;

    private Transform _tf;
    private Camera _cam;
    private float _lastMouseViewportPosX;
    private float _roadXOffset; //offset from world center (0,0,0)
    private float _roadWidth;
    private bool _inputBlocked;
    private bool _shouldMove;
    private int _coinsCounter;

    private float MouseViewportPointX => Mathf.Clamp(_cam.ScreenToViewportPoint(Input.mousePosition).x, 0.1f, 0.9f); //clamped inside viewport because we are supposed to play it not on PC actually
    public int CoinsCounter => _coinsCounter;

    private void Start()
    {
        _cam = Camera.main;
        _tf = this.transform;
        _roadWidth = roadTf.localScale.x;
        _roadXOffset = roadTf.position.x - _roadWidth / 2;
    }

    private void Update()
    {
        HandleInput();
        AdjustXPosition();
        MoveForward();
    }

    public void PickUpCoin()
    {
        _coinsCounter++;
        OnCoinPickedUp?.Invoke(CoinsCounter);
    }

    public void ReachFinish()
    {
        _inputBlocked = true;
        _shouldMove = false;
        OnFinishReached?.Invoke();
    }

    private void HandleInput()
    {
        if (_inputBlocked)
            return;

        HandleLMB_Down();
        HandleLMB_Up();
    }

    private void HandleLMB_Down()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        _shouldMove = true;
        _lastMouseViewportPosX = MouseViewportPointX;
        OnLMBPressed?.Invoke();
    }

    private void HandleLMB_Up()
    {
        if (!Input.GetMouseButtonUp(0))
            return;

        _shouldMove = false;
        OnLMBReleased?.Invoke();
    }

    private void AdjustXPosition()
    {
        if (!_shouldMove)
            return;
        
        var newPos = _tf.position;
        var swipeLength = (MouseViewportPointX - _lastMouseViewportPosX) * strokeSensitivity;
        var currentViewportPosX = RoadToViewportPoint(_tf.position.x);

        newPos.x = ViewportToRoadPoint(Mathf.Clamp(currentViewportPosX + swipeLength, 0.1f, 0.9f)); //clamped inside viewport because we are supposed to play it not on PC actually
        _tf.position = newPos;
        _lastMouseViewportPosX = MouseViewportPointX;
    }

    private void MoveForward()
    {
        if (!_shouldMove)
            return;

        _tf.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private float ViewportToRoadPoint(float viewportPosX)
    {
        return _roadWidth * viewportPosX + _roadXOffset;
    }

    private float RoadToViewportPoint(float onRoadPosX)
    {
        return (_tf.position.x  - _roadXOffset) / _roadWidth;
    }
}
