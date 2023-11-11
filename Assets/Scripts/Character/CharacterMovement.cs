using System.Collections.Generic;
using CharacterMovements;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private float jumpSpeed;

    [SerializeField] private float damageJumpForce;

    // [SerializeField] private float slidingGravityIncrease;

    private Character _character;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private HashSet<int> _groundCollision;
    private bool _isGrounded, _isOnOil;
    
    // Start is called before the first frame update
    void Start()
    {
        _groundCollision = new HashSet<int>();
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // HandleGravityChanges();
        HandlePlaneMovement();
        HandleJumpMovement();
        HandleOilDamageMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleSurfaceEnter(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        HandleSurfaceExit(other.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleSurfaceEnter(collision.gameObject);
    }

    void OnCollisionExit(Collision collision) 
    {
        HandleSurfaceExit(collision.gameObject);
    }

    private void HandlePlaneMovement() 
    {
        var axes = LookupAxes();
        var horizontalInput = Input.GetAxis(axes.Item1);
        var verticalInput = Input.GetAxis(axes.Item2);

        if (horizontalInput == 0 && verticalInput == 0)
        {
            _animator.Play("Idle");
            return;
        }
        _animator.Play("Run");

        var movement = new Vector3(horizontalInput, 0, verticalInput);
        var movementDelta = movement * (moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.09f);
        transform.Translate(movementDelta, Space.World);
    }

    // private void HandleGravityChanges()
    // {
    //     var gravity = Physics.gravity;
    //     if (!_isSliding && gravity.y < -9.8f)
    //     {
    //         Physics.gravity = new Vector3(0, -9.8f, 0);
    //         return;
    //     }
    //
    //     Physics.gravity = new Vector3(0, gravity.y * slidingGravityIncrease, 0);
    // }

    private void HandleJumpMovement()
    {
        var axis = LookupJump();
        var jumpInput = Input.GetButtonDown(axis);

        if (!jumpInput || !_isGrounded) return;

        var movement = new Vector3(0, jumpSpeed, 0);
        _rigidbody.AddForce(movement);
    }

    private void HandleOilDamageMovement()
    {
        if (!_isOnOil) return;

        var force = new Vector3(0, damageJumpForce, 0);
        _rigidbody.AddForce(force);
    }

    private void HandleSurfaceEnter(GameObject gameObj)
    {
        var surface = gameObj.GetComponent<ISurface>();
        if (surface == null) return;

        switch (surface.GetSurfaceType())
        {
            case SurfaceType.Ground:
                _groundCollision.Add(gameObj.GetInstanceID());
                _isGrounded = true;
                break;

            // case SurfaceType.Sliding:
            //     _isSliding = true;
            //     break;

            case SurfaceType.Oil:
                _isOnOil = true;
                break;

            default:
                return;
        }
    }

    private void HandleSurfaceExit(GameObject gameObj)
    {
        var surface = gameObj.GetComponent<ISurface>();
        if (surface == null) return;

        switch (surface.GetSurfaceType())
        {
            case SurfaceType.Ground:
                _groundCollision.Remove(gameObj.GetInstanceID());
                _isGrounded = _groundCollision.Count != 0;
                break;

            // case SurfaceType.Sliding:
            //     _isSliding = false;
            //     break;

            case SurfaceType.Oil:
                _isOnOil = false;
                break;

            default:
                return;
        }
    }

    private (string, string) LookupAxes()
    {
        return ("Horizontal" + _character.PlayerID, "Vertical" + _character.PlayerID);
    }

    private string LookupJump()
    {
        return "Jump" + _character.PlayerID;
    }
}