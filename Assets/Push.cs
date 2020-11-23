using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    private Rigidbody _rb;

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            _rb.AddForce(this.transform.forward * 1, ForceMode.Impulse);
        }
    }
}
