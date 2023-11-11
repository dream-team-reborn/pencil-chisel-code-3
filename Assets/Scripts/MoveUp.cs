using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f; // Speed of the movement
    [SerializeField]
    private float startingY = -2.1f;
    [SerializeField]
    private float maxY = 0.6f;

    private bool isMoving = false;

    void Start()
    {
        ResetPosition();
    }

    void Update()
    {
        if (isMoving)
        {
            float newY = transform.position.y + speed * Time.deltaTime;

            if (newY < maxY)
            {
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
        }
    }

    public void StartMovement()
    {
        isMoving = true;
    }

    public void StopMovement()
    {
        isMoving = false;
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(transform.position.x, startingY, transform.position.z);
    }
}
