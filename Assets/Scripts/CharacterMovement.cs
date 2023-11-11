using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float jumpSpeed;

    [SerializeField]
    private GameObject floor;

    private bool _isGrounded;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandlePlaneMovement();
        HandleJumpMovement();
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == floor.name)
        {
            _isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == floor.name)
        {
            _isGrounded = false;
        }
    }

    private void HandlePlaneMovement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput == 0 && verticalInput == 0) return;
        
        var movement = new Vector3(horizontalInput, 0, verticalInput);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        transform.Translate(movement * (moveSpeed * Time.deltaTime), Space.World);
        ReduceVerticalMovement();
    }

    private void HandleJumpMovement()
    {
        var jumpInput = Input.GetButtonDown("Jump");

        if (!jumpInput || !_isGrounded) return;
        var movement = new Vector3(0, 1, 0);
        transform.Translate(movement * (jumpSpeed * Time.deltaTime), Space.World);
    }

    private void ReduceVerticalMovement()
    {
        //if (transform.y >= 100) {
          //  transform.y -= transform.y * 1.10f;
        //}
    }

}
