using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRot : MonoBehaviour
{
    public CameraSystem TestCamSys;
    public Transform Mesh;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        //this.transform.Rotate(Vector3.up * 25f * Time.deltaTime);

        Vector3 velocity = _rigidbody.velocity;


        Vector3 movementVertical = Vector3.ProjectOnPlane(TestCamSys.transform.forward, Vector3.up).normalized * Input.GetAxis("Vertical");
        Vector3 movementHorizontal = Vector3.ProjectOnPlane(TestCamSys.transform.right, Vector3.up).normalized * Input.GetAxis("Horizontal");
        Vector3 movement = movementVertical + movementHorizontal;
        movement.y = 0;

        if (movement.sqrMagnitude > 0.001f)
        {
            velocity += movement * 0.035f * Time.deltaTime * 500f;
            SmoothRotateTowards(velocity);
        }

        _rigidbody.velocity = velocity;
    }


    private void SmoothRotateTowards(Vector3 forward)
    {

        forward = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(forward, Vector3.up);
        Mesh.transform.rotation = Quaternion.Slerp(Mesh.transform.rotation, targetRotation, 7.5f * Time.deltaTime * 5f);
    }
}
