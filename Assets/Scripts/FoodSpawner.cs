using UnityEngine;

public class SpawnObjectOnSpace : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;
    // Reference to the cube transform
    public Transform spawnAreaTransform;
    private Bounds spawnArea;

    void Start()
    {
        spawnArea = GetTransformBounds(transform.Find("SpawnArea"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnPrefab();
        }
    }

    void SpawnPrefab()
    {
        int randomIndex = Random.Range(0, prefabsToSpawn.Length);

        // Generate a random position within the cube bounds
        Vector3 randomPosition = GetRandomPositionInBounds(spawnArea);
        Instantiate(prefabsToSpawn[randomIndex], randomPosition , Quaternion.identity);
    }


    Bounds GetTransformBounds(Transform objTransform)
    {
        // Get the bounds of the object (in local space)
        Renderer renderer = objTransform.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds;
        }
        else
        {
            Debug.LogError("Object does not have a Renderer component.");
            return new Bounds();
        }
    }

    Vector3 GetRandomPositionInBounds(Bounds bounds)
    {
        // Calculate a random position within the bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, randomY, randomZ);
    }

}
