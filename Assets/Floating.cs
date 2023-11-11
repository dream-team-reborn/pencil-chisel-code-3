using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public float floatingForceFactor = 0.5f; // Adjust this factor to control the strength of the floating force
    [SerializeField] private Transform waterObject;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            // Calculate the submerged volume
            float submergedVolume = CalculateSubmergedVolume(other);
Debug.Log("Variable Value: " + submergedVolume);

            // Apply a buoyancy force based on the submerged volume
            ApplyFloatingForce(submergedVolume);
        }
    }

    private float CalculateSubmergedVolume(Collider other)
    {
        // Get the y-coordinate of the other object
        float waterLevel = waterObject.position.y;

        // Calculate the volume of the submerged portion
        float submergedVolume = 0f;

        // Get the bounds of the object in world space
        Bounds bounds = GetComponent<Collider>().bounds;

        // Calculate the water level in local coordinates
        float localWaterLevel = transform.InverseTransformPoint(new Vector3(0f, waterLevel, 0f)).y;

        // If the water level is above the object, the object is fully submerged
        if (localWaterLevel > bounds.max.y)
        {
            submergedVolume = GetComponent<Collider>().bounds.size.x * GetComponent<Collider>().bounds.size.y * GetComponent<Collider>().bounds.size.z;
        }
        // If the water level is below the object, the object is not submerged
        else if (localWaterLevel < bounds.min.y)
        {
            submergedVolume = 0f;
        }
        else
        { 
            // Calculate the submerged volume using the water level and the object's bounds
            float submergedHeight = localWaterLevel - bounds.min.y;
            submergedVolume = submergedHeight * GetComponent<Collider>().bounds.size.x * GetComponent<Collider>().bounds.size.z;
        }

        return submergedVolume;
    }

    private void ApplyFloatingForce(float submergedVolume)
    {
        // Apply a buoyancy force based on the submerged volume
        float buoyancyForce = submergedVolume * floatingForceFactor;
        GetComponent<Rigidbody>().AddForce(Vector3.up * buoyancyForce, ForceMode.Force);
    }
}
