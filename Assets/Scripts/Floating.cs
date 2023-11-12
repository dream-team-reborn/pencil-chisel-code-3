using UnityEngine;

public class Floating : MonoBehaviour
{
    public float velocityDrag = 0.01f;
    public float angularDrag = 0.008f;
    public float floatHeightPercentage = 0.1f;
    public float maxSubmergedVolume = 5.0f;
    public float maxFloatingTime = 10f;

    public Transform waterObject;

    private float currentFloatingTime = 0.0f;
    private Material mat;
    private Vector3 startingScale;

    [SerializeField] 
    float burnStateMultiplier = 0.5f;
    [SerializeField]
    float scaleMultiplier = 0.25f;

    private void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        startingScale = transform.localScale;
    }

    private void Update()
    {
        float waterLevel = waterObject.position.y;
        Bounds bounds = GetComponent<Collider>().bounds;

        float submergedVolume = CalculateSubmergedVolume(waterLevel);

        currentFloatingTime += Time.deltaTime;
        SetBurnState();
        SetScale();

        if (submergedVolume > 0/* && !isFloatingTimeExpired*/)
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            ApplyFloatingForce(rb, submergedVolume);
            dampVelocity(rb);
            dampRotation(rb);
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
            float submergedHeight = waterLevel - (bounds.min.y + (bounds.max.y - bounds.min.y) * floatHeightPercentage);
            submergedVolume = submergedHeight * GetComponent<Collider>().bounds.size.x * GetComponent<Collider>().bounds.size.z;
        }

        return submergedVolume;
    }

    private void dampVelocity(Rigidbody rb)
    {
        rb.velocity *= (1f - velocityDrag);
    }

    private void dampRotation(Rigidbody rb)
    {
        rb.angularVelocity *= (1f - angularDrag);
    }

    private void ApplyFloatingForce(Rigidbody rb, float submergedVolume)
    {
        float gravityValue = 9.81f;
        float buoyancyForce = submergedVolume * gravityValue;

        rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Force);    
    }

    private void SetBurnState()
    {
        float burnState = Mathf.Min(currentFloatingTime * burnStateMultiplier / maxFloatingTime, 1.0f);
        mat.SetFloat("_Burn_State", burnState);
    }

    private void SetScale()
    {
        float scaleState = 1 - Mathf.Min(currentFloatingTime * scaleMultiplier / maxFloatingTime, 1.0f);

        if(scaleState <= 0)
        {
            Destroy (gameObject);
        }
        else
        {
            transform.localScale = startingScale * scaleState;
        }
    }
}
