using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // Drag your prefab here in the Inspector
    public int gridWidth = 5;       // Number of objects on X axis
    public int gridHeight = 5;      // Number of objects on Z axis
    public float spacing = 2.0f;    // Distance between objects

    void Start()
    {
        SpawnGrid();
    }

    void SpawnGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                // Calculate position based on loop index and spacing
                Vector3 spawnPos = new Vector3(x * spacing, 0, z * spacing);
                
                // Spawn the object at the calculated position
                Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
            }
        }
    }
}
