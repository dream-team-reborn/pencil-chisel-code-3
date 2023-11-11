using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed;

    [SerializeField] private float jumpSpeed;

    [SerializeField] private float slidingGravityIncrease;
    
    [SerializeField] private GameObject flooringSurface;

    [SerializeField] private GameObject slidingSurface;

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
        if (collision.gameObject.name == flooringSurface.name)
        {
            _isGrounded = true;
        }
        
        if (collision.gameObject.name == slidingSurface.name)
        {
            _isSliding = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == flooringSurface.name)
        {
            _isGrounded = false;
        }
        
        if (collision.gameObject.name == slidingSurface.name)
        {
            _isSliding = false;
        }
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
        if ((!_isSliding || _isGrounded) && gravity.y < -9.8f)
        {
            Physics.gravity = new Vector3(0, -9.8f, 0);
            return;
        }

        Physics.gravity = new Vector3(0, gravity.y * slidingGravityIncrease, 0);
    }

    private void HandleJumpMovement()
    {
        var jumpInput = Input.GetButtonDown("Jump");

        if (!jumpInput || !_isGrounded) return;
        var movement = new Vector3(0, jumpSpeed, 0);
        _rigidbody.AddForce(movement);
    }
}
