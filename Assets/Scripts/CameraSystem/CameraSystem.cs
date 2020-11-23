using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    //add option for collision groups (camera dont get blocked by certain stuff)

    //Temporary
    private UnityEngine.UI.Text _debugText;

    [Header("Essentials")]
    [SerializeField] private Transform _followTarget;
    [SerializeField] private Transform _camera;

    //Variables
    private Vector2 _cameraZoomRange = new Vector2(2f, 5f);
    private float _cameraZoomStrength = 6f;
    private float _cameraSensitivity = 3f;

    private float _targetDistance;
    private float _currentDistance;
    private Vector3 _followVelocityDamp;
    private float _angleX;
    private Vector3 _offsetNormalized;

    private void Start()
    {
        _targetDistance = _cameraZoomRange.x + (_cameraZoomRange.y - _cameraZoomRange.x) / 2f;
        _currentDistance = _targetDistance;
        _offsetNormalized = (_camera.position - this.transform.position).normalized;

        _debugText = GameObject.Find("DebugText").GetComponent<UnityEngine.UI.Text>();
    }

    private void Update()
    {
        SetCameraDistance();
        SmoothFollowTarget();

        _debugText.text = $"Zoom current: {_currentDistance} / Zoom target: {_targetDistance}";
    }

    private void SetCameraDistance()
    {
        _currentDistance = Mathf.Lerp(_currentDistance, _targetDistance, Time.deltaTime * _cameraZoomStrength * 4f);

        Ray ray = new Ray(this.transform.position, this.transform.rotation * _offsetNormalized);
        RaycastHit rayHit;
        float distance = Physics.SphereCast(ray, 0.1f, out rayHit, _currentDistance) ? rayHit.distance : _currentDistance;

        _camera.localPosition = _offsetNormalized * Mathf.Clamp(distance, _cameraZoomRange.x, _cameraZoomRange.y);
    }
    private void SmoothFollowTarget()
    {
        this.transform.position = Vector3.SmoothDamp(this.transform.position, _followTarget.transform.position, ref _followVelocityDamp, 0.2f);
    }

    public void RotatePivot(Vector3 mouseInput)
    {
        mouseInput *= _cameraSensitivity;
        _angleX = Mathf.Clamp(_angleX + mouseInput.x, -35, 65);

        this.transform.rotation = Quaternion.Euler(_angleX, this.transform.localEulerAngles.y + mouseInput.y, 0);
    }

    public void Zoom(float amount)
    {
        if(amount > 0)//Zoom in when camera is blocked means we have to set maxDistance to the current position
            _currentDistance = -_camera.localPosition.z;

        _targetDistance = Mathf.Clamp(_currentDistance - (amount * _cameraZoomStrength), _cameraZoomRange.x, _cameraZoomRange.y);
    }

}
