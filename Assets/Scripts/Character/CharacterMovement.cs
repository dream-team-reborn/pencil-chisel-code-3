using System;
using System.Collections;
using System.Collections.Generic;
using CharacterMovements;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private float jumpSpeed;

    [SerializeField] private float slidingGravityIncrease;

    private Rigidbody _rigidbody;
    private bool _isGrounded, _isSliding;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleGravityChanges();
        HandlePlaneMovement();
        HandleJumpMovement();
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleSurfaceOnCollisionEnter(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        HandleSurfaceOnCollisionExit(collision);
    }

    private void HandlePlaneMovement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput == 0 && verticalInput == 0) return;

        var movement = new Vector3(horizontalInput, 0, verticalInput);
        var movementDelta = movement * (moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        transform.Translate(movementDelta, Space.World);
    }

    private void HandleGravityChanges()
    {
        var gravity = Physics.gravity;
        if (!_isSliding && gravity.y < -9.8f)
        {
            Physics.gravity = new Vector3(0, -9.8f, 0);
            return;
        }

        Physics.gravity = new Vector3(0, gravity.y * slidingGravityIncrease, 0);
    }

    private void HandleJumpMovement()
    {
        var jumpInput = Input.GetButtonDown("Jump");

        if (!jumpInput) return;

        var movement = new Vector3(0, jumpSpeed, 0);

        if (_isSliding)
        {
            _rigidbody.AddForce(movement);
        }

        if (_isGrounded)
        {
            _rigidbody.AddForce(movement);
        }
    }

    private void HandleSurfaceOnCollisionEnter(Collision collision)
    {
        var surface = collision.gameObject.GetComponent<ISurface>();
        if (surface == null) return;

        switch (surface.GetSurfaceType())
        {
            case SurfaceType.Ground:
                _isGrounded = true;
                break;

            case SurfaceType.Sliding:
                _isSliding = true;
                break;
            
            default:
                return;
        }
    }

    private void HandleSurfaceOnCollisionExit(Collision collision)
    {
        var surface = collision.gameObject.GetComponent<ISurface>();
        if (surface == null) return;

        switch (surface.GetSurfaceType())
        {
            case SurfaceType.Ground:
                _isGrounded = false;
                break;

            case SurfaceType.Sliding:
                _isSliding = false;
                break;
            
            default:
                return;
        }
    }
}