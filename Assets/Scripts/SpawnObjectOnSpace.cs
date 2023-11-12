using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SpawnObjectOnSpace : MonoBehaviour
{
    public GameObject[] platformsToSpawn;
    public GameObject[] obstaclesToSpawn;
    public float spawnWaitMin = 3f;
    public float spawnWaitMax = 5f;
    public float spawnRatio = 0.25f;
    
    // Reference to the cube transform
    private Bounds _spawnArea;
    private float _nextSpawnIn, _lastSpawnTime = -1;
    private bool _isSpawnActive;
    private List<GameObject> _spawned;
    
    [SerializeField] private Transform waterObject;

    void Start()
    {
        _isSpawnActive = false;
        _spawned = new List<GameObject>();
        _spawnArea = GetTransformBounds(transform.Find("SpawnArea"));
    }

    void Update()
    {
        if (!_isSpawnActive) return;
        
        var currentTime = Time.time;
        if (!(currentTime - _lastSpawnTime > _nextSpawnIn)) return;
        
        _lastSpawnTime = currentTime;
        _nextSpawnIn = Random.Range(spawnWaitMin, spawnWaitMax);

        SpawnPrefab();
    }

    public void StartSpawning()
    {
        _isSpawnActive = true;
    }

    public void StopSpawning()
    {
        _isSpawnActive = false;
        //CleanSpawnedObjects();
    }

    public void ClearSpawning()
    {
        CleanSpawnedObjects();
    }

    private void SpawnPrefab()
    {
        var subgroup = SelectSubgroup();
        var randomIndex = Random.Range(0, subgroup.Length);

        // Generate a random position within the cube bounds
        var randomPosition = GetRandomPositionInBounds(_spawnArea);
        var objectSpawned = Instantiate(subgroup[randomIndex], randomPosition, Quaternion.identity);
        _spawned.Add(objectSpawned);
        // We should avoid to GetComponent in the update function it is very expensive
        objectSpawned.GetComponent<Floating>().waterObject = waterObject;
    }
    
    private GameObject[] SelectSubgroup()
    {
        double randomValue = Random.value;
        return randomValue <= spawnRatio ? platformsToSpawn : obstaclesToSpawn;
    }

    private void CleanSpawnedObjects()
    {
        if (_spawned == null) return;
        foreach (var obj in _spawned)
        {
            Destroy(obj);
        }
    }
    
    private static Bounds GetTransformBounds(Component component)
    {
        // Get the bounds of the object (in local space)
        var renderer = component.GetComponent<Renderer>();
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

    private static Vector3 GetRandomPositionInBounds(Bounds bounds)
    {
        // Calculate a random position within the bounds
        var randomX = Random.Range(bounds.min.x, bounds.max.x);
        var randomY = Random.Range(bounds.min.y, bounds.max.y);
        var randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, randomY, randomZ);
    }
}
