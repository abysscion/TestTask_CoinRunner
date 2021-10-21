using UnityEngine;

public class LimbAnimator : MonoBehaviour
{
    public float rotationSpeed = 150f;
    [Range(0, 180)]
    public float positiveAngleThreshold = 45f;
    [Range(-180, 0)]
    public float negativeAngleThreshold = -45f;
    public bool isStartRotationPositive;
    public bool participateInIdleAnimation;

    private Transform _tf;
    private Quaternion _defaultRotation;
    private Vector3 _defaultPosition;
    private float _cumulativeRotationAngle;
    private float _halfScale;
    private bool _isRotationPositive;
    private bool _isRunning;

    private Vector3 RotationPoint => _tf.position + _tf.up * _halfScale;

    private void Start()
    {
        _tf = transform;
        _halfScale = _tf.localScale.y / 2;
        _defaultRotation = _tf.localRotation;
        _defaultPosition = _tf.localPosition;
        _cumulativeRotationAngle = 0;
        _isRotationPositive = isStartRotationPositive;
    }

    private void Update()
    {
        ResolveAnimations();
    }

    public void PlayRunningAnimation()
    {
        _isRunning = true;
        ResetAnimation();
    }

    public void PlayIdleAnimation()
    {
        _isRunning = false;
        ResetAnimation();
    }

    private void ResolveAnimations()
    {
        ResolveRunningAnimation();
        ResolveIdleAnimation();
    }

    private void ResolveRunningAnimation()
    {
        if (!_isRunning)
            return;

        _tf.RotateAround(RotationPoint, _tf.right, CalculateRotation());
    }

    private void ResolveIdleAnimation()
    {
        if (_isRunning || !participateInIdleAnimation)
            return;

        _tf.RotateAround(RotationPoint, _tf.forward, CalculateRotation());
    }

    private void ResetAnimation()
    {
        _cumulativeRotationAngle = 0;
        _isRotationPositive = isStartRotationPositive;
        _tf.localRotation = _defaultRotation;
        _tf.localPosition = _defaultPosition;
    }

    private float CalculateRotation()
    {
        if (_cumulativeRotationAngle == positiveAngleThreshold || _cumulativeRotationAngle == negativeAngleThreshold)
            _isRotationPositive = !_isRotationPositive;

        var rotationValue = rotationSpeed * Time.deltaTime;

        rotationValue = _isRotationPositive ? rotationValue : -rotationValue;
        _cumulativeRotationAngle += rotationValue;
        _cumulativeRotationAngle = Mathf.Clamp(_cumulativeRotationAngle, negativeAngleThreshold, positiveAngleThreshold);

        return rotationValue;
    }
}
