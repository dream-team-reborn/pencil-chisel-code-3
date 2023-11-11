using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SpawnObjectOnSpace : MonoBehaviour
{
    public GameObject[] platformsToSpawn;
    public GameObject[] obstaclesToSpawn;
    public float platformOverObstaclesPercentage = 0.1f;
    public float platformSpawnWaitMin = 3f;
    public float platformSpawnWaitMax = 5f;
    public float obstaclesSpawnWaitMin = 0.1f;
    public float obstaclesSpawnWaitMax = 0.5f;
    // Reference to the cube transform
    public Transform spawnAreaTransform;
    private Bounds spawnArea;
    private float lastSpawnTime = -1; 
    private float nextSpawnIn = 0f;

    public enum FoodType { PLATFORM, OBSTACLE }

    private Dictionary<FoodType, GameObject[]> foodMap;

    [SerializeField] private Transform waterObject;

    void Start()
    {
        foodMap =   new Dictionary<FoodType, GameObject[]> 
            { { FoodType.PLATFORM, platformsToSpawn }, { FoodType.OBSTACLE, obstaclesToSpawn } };
        spawnArea = GetTransformBounds(transform.Find("SpawnArea"));
    }

    void Update()
    {
        float currentTime = Time.time;
        if ((currentTime - lastSpawnTime) > nextSpawnIn)
        {
            lastSpawnTime = currentTime;
            switch (SpawnPrefab()) 
            {
                case FoodType.PLATFORM: nextSpawnIn = Random.Range(platformSpawnWaitMin, platformSpawnWaitMax); break;
                case FoodType.OBSTACLE: nextSpawnIn = Random.Range(obstaclesSpawnWaitMin, obstaclesSpawnWaitMax); break;
            }
        }
    }

    FoodType SpawnPrefab()
    {
        FoodType foodTypeSelected = SelectFoodType();

        GameObject[] platformsSelection = foodMap[foodTypeSelected];
        if (platformsSelection != null)
        {
            int randomIndex = Random.Range(0, platformsSelection.Length - 1);

            // Generate a random position within the cube bounds
            Vector3 randomPosition = GetRandomPositionInBounds(spawnArea);
            Debug.Log(randomPosition);
            Object objectSpawned = Instantiate(platformsSelection[randomIndex], randomPosition, Quaternion.identity);
            objectSpawned.GetComponent<Floating>().waterObject = waterObject;
        }

        return foodTypeSelected;
    }

    FoodType SelectFoodType()
    {
        double randomValue = UnityEngine.Random.value;
        return randomValue <= platformOverObstaclesPercentage ? FoodType.PLATFORM : FoodType.OBSTACLE;
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
