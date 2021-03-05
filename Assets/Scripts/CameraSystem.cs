using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _target;

    public Camera Camera { get => _camera; }

    private Vector3 _offset;

    //Variables
    private Vector2 _zoomRange = new Vector2(2f, 5f);
    private float _zoomStrength = 8f;
    private float _rotationSensitivity = 3f;
    private float _followSmoothTime = 0.2f;

    private float _targetDistance;
    private float _currentDistance;
    private Vector3 _followVelocityDamp;
    private float _pivotAngleX;
    private Vector3 _offsetNormalized;

    private void Start()
    {
        _offset = this.transform.position - _target.position;
        _targetDistance = _zoomRange.x + (_zoomRange.y - _zoomRange.x) / 2f;
        _currentDistance = _targetDistance;
        _offsetNormalized = (_camera.transform.position - this.transform.position).normalized;
        this.transform.parent = null;
    }

    private void Update()
    {
        SetCameraDistance();
        SmoothFollowTarget();
    }

    private void SetCameraDistance()
    {
        _currentDistance = Mathf.Lerp(_currentDistance, _targetDistance, Time.deltaTime * _zoomStrength * 4f);
        _currentDistance = Mathf.Clamp(_currentDistance, _zoomRange.x, _zoomRange.y);

        Ray ray = new Ray(this.transform.position, this.transform.rotation * _offsetNormalized);
        RaycastHit rayHit;
        float distance = Physics.SphereCast(ray, 0.1f, out rayHit, _currentDistance) ? rayHit.distance : _currentDistance;

        _camera.transform.localPosition = _offsetNormalized * distance;
    }
    private void SmoothFollowTarget()
    {
        if (_target)
            this.transform.position = Vector3.SmoothDamp(this.transform.position, _target.transform.position + _offset, ref _followVelocityDamp, _followSmoothTime);
    }

    public void RotatePivot(Vector3 mouseInput)
    {
        mouseInput *= _rotationSensitivity;
        _pivotAngleX = Mathf.Clamp(_pivotAngleX + mouseInput.x, -65, 65);

        this.transform.rotation = Quaternion.Euler(_pivotAngleX, this.transform.localEulerAngles.y + mouseInput.y, 0);
    }

    public void Zoom(float amount)
    {
        //immediately set current distance, when camera is blocked this will immediately take over the blocking distance and zoom instead of doing nothing
        _currentDistance = (_camera.transform.position - this.transform.position).magnitude;

        _targetDistance = _currentDistance - (amount * _zoomStrength);
    }
}