using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    string _radiusShaderKey = "_Radius";
    [SerializeField] private float speed = 1.0f; // Speed of the movement
    [SerializeField] private float startingY = -2.1f;
    [SerializeField] private float maxY = 0.6f;

    private bool isMoving = false;

    [SerializeField] Material oilMaryMaterial;

    [SerializeField] float _radiusStep = 0.01f;

    [SerializeField] private AnimationCurve _animationCurve;

    private void Awake()
    {
        oilMaryMaterial = GetComponent<MeshRenderer>().materials[0];
        oilMaryMaterial.SetFloat(_radiusShaderKey, 0.09f);
    }

    void Start()
    {
        ResetPosition();
    }

    void Update()
    {
        if (!isMoving) return;
        var newY = transform.position.y + speed * Time.deltaTime;

        if (newY > maxY) return;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        var multiplier = _animationCurve.Evaluate(Normalize(newY, startingY, maxY));
        var oldShaderRadius = oilMaryMaterial.GetFloat(_radiusShaderKey);
        oilMaryMaterial.SetFloat(_radiusShaderKey, oldShaderRadius + _radiusStep * multiplier * Time.deltaTime);
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
        oilMaryMaterial.SetFloat(_radiusShaderKey, 0.09f);
        transform.position = new Vector3(transform.position.x, startingY, transform.position.z);
    }

    private static float Normalize(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }
}