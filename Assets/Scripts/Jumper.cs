using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private float _jumpForce;

    private Rigidbody _rigidbody;
    private bool _isGrounded;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isGrounded == true)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce);
            _isGrounded = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Road _road))
        {
            _isGrounded = true;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out JumpForce jumpForce))
        {
            _jumpForce = jumpForce._force;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out JumpForce jumpForce))
        {
            _jumpForce = jumpForce._exitForce;
        }
    }
}
