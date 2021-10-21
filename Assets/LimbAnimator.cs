using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class LimbAnimator : MonoBehaviour
{
    public float rotationSpeed = 150f;
    [Range(0, 180)]
    public float positiveAngleThreshold = 45f;
    [Range(-180, 0)]
    public float negativeAngleThreshold = -45f;
    [FormerlySerializedAs("Restoring to default rotation time")]
    public float restorationTime = 0.25f;
    public bool isStartRotationPositive;

    private Coroutine _restoringAnimationCoroutine;
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
        ResolveRotation();
    }

    public void PlayAnimation()
    {
        _isRunning = true;
        if (_restoringAnimationCoroutine != null)
        {
            StopCoroutine(_restoringAnimationCoroutine);
            ResetAnimation();
        }
    }

    public void StopAnimation()
    {
        _isRunning = false;
        _restoringAnimationCoroutine = StartCoroutine(AnimateRestoringDefaultRotation());
        _cumulativeRotationAngle = 0;
        _isRotationPositive = isStartRotationPositive;
    }

    private void ResolveRotation()
    {
        if (!_isRunning)
            return;

        if (_cumulativeRotationAngle == positiveAngleThreshold || _cumulativeRotationAngle == negativeAngleThreshold)
            _isRotationPositive = !_isRotationPositive;
        
        var rotationValue = rotationSpeed * Time.deltaTime;

        rotationValue = _isRotationPositive ? rotationValue : -rotationValue;
        _cumulativeRotationAngle += rotationValue;
        _cumulativeRotationAngle = Mathf.Clamp(_cumulativeRotationAngle, negativeAngleThreshold, positiveAngleThreshold);

        _tf.RotateAround(RotationPoint, _tf.right, rotationValue);
    }

    private void ResetAnimation()
    {
        _cumulativeRotationAngle = 0;
        _isRotationPositive = isStartRotationPositive;
        _tf.localRotation = _defaultRotation;
        _tf.localPosition = _defaultPosition;
    }

    private IEnumerator AnimateRestoringDefaultRotation()
    {
        var startTime = Time.unscaledTime;
        var startRot = _tf.localRotation;
        var startPos = _tf.localPosition;
        var currentTime = 0f;

        while ((Time.unscaledTime - startTime) < restorationTime)
        {
            currentTime += Time.unscaledDeltaTime;

            var step = currentTime / restorationTime;
            var thisFrameRotation = new Quaternion()
            {
                eulerAngles = new Vector3()
                {
                    x = Mathf.LerpAngle(startRot.eulerAngles.x, _defaultRotation.eulerAngles.x, step),
                    y = Mathf.LerpAngle(startRot.eulerAngles.y, _defaultRotation.eulerAngles.y, step),
                    z = Mathf.LerpAngle(startRot.eulerAngles.z, _defaultRotation.eulerAngles.z, step)
                }
            };
            var thisFramePosition = new Vector3()
            {
                x = Mathf.Lerp(startPos.x, _defaultPosition.x, step),
                y = Mathf.Lerp(startPos.y, _defaultPosition.y, step),
                z = Mathf.Lerp(startPos.z, _defaultPosition.z, step),
            };

            _tf.localRotation = thisFrameRotation;
            _tf.localPosition = thisFramePosition;

            yield return null;
        }

        _tf.localRotation = _defaultRotation;
        _tf.localPosition = _defaultPosition;

        yield return null;
    }
}
