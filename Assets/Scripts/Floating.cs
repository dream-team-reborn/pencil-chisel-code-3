using UnityEngine;

public class Floating : MonoBehaviour
{
    public float velocityDump = 0.9f;

    [SerializeField] private Transform waterObject;

    private void Update()
    {
        float waterLevel = waterObject.position.y;
        Bounds bounds = GetComponent<Collider>().bounds;

        float submergedVolume = CalculateSubmergedVolume(waterLevel);

        if (submergedVolume > 0)
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            ApplyFloatingForce(rb, submergedVolume);
            dampVelocity(rb);
        }
        
    }

    private float CalculateSubmergedVolume(float waterLevel)
    {
        float submergedVolume = 0f;
        Bounds bounds = GetComponent<Collider>().bounds;
        if (waterLevel > bounds.max.y)
        {
            submergedVolume = GetComponent<Collider>().bounds.size.x * GetComponent<Collider>().bounds.size.y * GetComponent<Collider>().bounds.size.z;
        }
        else if (waterLevel < bounds.min.y)
        {
            submergedVolume = 0f;
        }
        else
        { 
            float submergedHeight = waterLevel - bounds.min.y;
            submergedVolume = submergedHeight * GetComponent<Collider>().bounds.size.x * GetComponent<Collider>().bounds.size.z;
        }

        return submergedVolume;
    }

    private void dampVelocity(Rigidbody rb)
    {
        rb.velocity *= (1f - velocityDump);
    }

    private void ApplyFloatingForce(Rigidbody rb, float submergedVolume)
    {
        float gravityValue = 9.81f;
        float buoyancyForce = submergedVolume * gravityValue;

        rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Force);    
    }
}
