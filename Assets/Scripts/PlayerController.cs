using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform roadTf;

    private Transform _tf;
    private Camera _cam;
    private float _roadXOffset; //offset from world center (0,0,0)
    private float _roadWidth;

    private void Start()
    {
        _cam = Camera.main;
        _tf = this.transform;
        _roadWidth = roadTf.localScale.x;
        _roadXOffset = roadTf.position.x - _roadWidth / 2;
    }

    private void Update()
    {
        Debug.Log(_cam.ScreenToViewportPoint(Input.mousePosition));
        AdjustPlayerPosition();
    }

    private void AdjustPlayerPosition()
    {
        if (!Input.GetMouseButton(0))
            return;

        var mViewportPos = _cam.ScreenToViewportPoint(Input.mousePosition);
        var newPos = _tf.position;

        mViewportPos.x = Mathf.Clamp(mViewportPos.x, 0f, 1f);
        newPos.x = ViewportToRoadPoint(mViewportPos.x);
        _tf.position = newPos;
    }

    private float ViewportToRoadPoint(float viewPortX)
    {
        return _roadWidth * viewPortX + _roadXOffset;
    }
}
