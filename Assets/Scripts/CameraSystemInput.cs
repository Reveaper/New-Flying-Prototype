using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystemInput : MonoBehaviour
{
    private CameraSystem _cameraSystem;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _cameraSystem = this.GetComponent<CameraSystem>();
    }

    private void Update()
    {
        RotateCamera();
        AdjustZoom();
    }

    private void RotateCamera()
    {
        Vector3 mouseInput = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        _cameraSystem.RotatePivot(mouseInput);
    }

    private void AdjustZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            _cameraSystem.Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }


}
