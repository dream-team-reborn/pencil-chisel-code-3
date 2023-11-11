using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 10f;
    public float friction = 0.9f; // Adjust this value for the desired friction effect
    public float slidingDownwardForce = 10f; // Adjust this value for the desired friction effect

    private Rigidbody rb;
    public bool isSliding = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Make sure the Rigidbody has constraints to freeze rotation along certain axes
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Get user input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction based on user input
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        Vector3 moveDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * movement;

        // Apply force to the Rigidbody based on user input
        rb.AddForce(moveDirection * moveSpeed);

        // Check if the player is touching the cup
        if (isSliding)
        {
            rb.velocity *= friction; // Apply friction to simulate sliding

            Vector3 playerPosition = transform.position;
            Vector3 CenterVector = - playerPosition.normalized;

            // Apply downward force to simulate sliding down
            rb.AddForce(CenterVector * slidingDownwardForce);
        }

        // Rotate the player to face the movement direction
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        }
    }

        void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("SlidingSurface")) // Adjust the tag accordingly
        {
            isSliding = true;
        }
    }

    // Called when the player stops colliding with the sliding surface
    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("SlidingSurface")) // Adjust the tag accordingly
        {
            isSliding = false;
        }
    }
}
